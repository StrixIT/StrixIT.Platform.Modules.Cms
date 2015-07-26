#region Apache License

//-----------------------------------------------------------------------
// <copyright file="PageRegistrator.cs" company="StrixIT">
// Copyright 2015 StrixIT. Author R.G. Schurgers MA MSc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

#endregion Apache License

using StrixIT.Platform.Core;
using StrixIT.Platform.Web;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StrixIT.Platform.Modules.Cms
{
    public class PageRegistrator : IPageRegistrator
    {
        #region Private Fields

        private static ConcurrentBag<ContentLocator> _contentLocators = new ConcurrentBag<ContentLocator>();
        private static object _lockObject = new object();
        private static ConcurrentDictionary<string, string> _pageLocations = new ConcurrentDictionary<string, string>();

        #endregion Private Fields

        #region Public Properties

        public IList<ContentLocator> ContentLocators
        {
            get
            {
                return _contentLocators.ToList();
            }
        }

        #endregion Public Properties

        #region Public Methods

        public bool IsLocationRegistered(string location)
        {
            return _pageLocations.Any(l => l.Key == location.ToLower());
        }

        public void LocatePages()
        {
            var folders = Directory.GetDirectories(StrixPlatform.Environment.MapPath("Views")).Where(d => d.ToLower() != "shared");

            foreach (var folder in folders)
            {
                if (Directory.GetFiles(folder, "index.cshtml").Any())
                {
                    var url = folder;

                    if (url.Contains("\\index.cshtml"))
                    {
                        url = url.Replace("\\index.cshtml", string.Empty);
                    }

                    url = url.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                    _pageLocations.GetOrAdd(url.ToLower(), folder);
                }
            }
        }

        public void RegisterAllPages(HttpContextBase httpContext, RouteData routeData)
        {
            foreach (var location in _pageLocations)
            {
                this.RenderPage(httpContext, routeData, location.Key);
            }
        }

        public void RegisterPage(HttpContextBase httpContext, RouteData routeData, string pagePath)
        {
            this.RenderPage(httpContext, routeData, pagePath);
        }

        public ContentLocator RegisterUrl(HtmlHelper helper, DisplayOptions options)
        {
            var view = helper.ViewContext.View as RazorView;
            string viewPath = view.ViewPath.ToLower();

            if (viewPath.Contains("/index.cshtml"))
            {
                viewPath = viewPath.Replace("/index.cshtml", string.Empty);
            }

            var pageUrl = viewPath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
            var locator = _contentLocators.FirstOrDefault(l => l.PageUrl == pageUrl && (options.Url != null ? options.Url.ToLower() == l.ContentUrl : l.ContentUrl == null));

            if (locator == null)
            {
                locator = new ContentLocator { ContentUrl = options.Url != null ? options.Url.ToLower() : null, PageUrl = pageUrl.ToLower() };
                _contentLocators.Add(locator);
            }

            return locator;
        }

        #endregion Public Methods

        #region Private Methods

        private void RenderPage(HttpContextBase httpContext, RouteData routeData, string pagePath)
        {
            var location = _pageLocations.Where(p => p.Key == pagePath).Select(p => p.Value).FirstOrDefault();

            if (location == null)
            {
                // If no page is found for this location, rerun LocatePages to pick up any newly
                // added pages.
                lock (_lockObject)
                {
                    this.LocatePages();

                    // Try to find the location once more.
                    location = _pageLocations.Where(p => p.Key == pagePath).Select(p => p.Value).FirstOrDefault();

                    if (location == null)
                    {
                        return;
                    }
                }
            }

            if (!_contentLocators.Any(l => l.PageUrl == pagePath))
            {
                // If no content locator for the page to register exists, render the page to create them.
                lock (_lockObject)
                {
                    if (!_contentLocators.Any(l => l.PageUrl == pagePath))
                    {
                        var viewPath = "~/" + Web.Helpers.GetVirtualPath(Path.Combine(location, "index.cshtml"));
                        var requestContext = new RequestContext(httpContext, routeData);
                        var controller = DependencyInjector.Get(typeof(PageController)) as Controller;
                        controller.ControllerContext = new ControllerContext(requestContext, controller);
                        MvcExtensions.RenderViewToString(controller.ControllerContext, viewPath);
                    }
                }
            }
        }

        #endregion Private Methods
    }
}
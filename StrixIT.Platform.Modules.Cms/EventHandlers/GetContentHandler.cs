#region Apache License

//-----------------------------------------------------------------------
// <copyright file="GetContentHandler.cs" company="StrixIT">
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
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace StrixIT.Platform.Modules.Cms
{
    public class GetContentHandler : IHandlePlatformEvent<GetContentEvent>
    {
        #region Private Fields

        private IPageRegistrator _registrator;

        #endregion Private Fields

        #region Public Constructors

        public GetContentHandler(IPageRegistrator registrator)
        {
            _registrator = registrator;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Handle(GetContentEvent args)
        {
            var locator = _registrator.RegisterUrl(args.Helper, args.Options);
            var url = (string)args.Helper.ViewContext.RequestContext.RouteData.Values[CmsConstants.URL] ?? locator.PageUrl;
            url = url.ToLower();
            var itemUrl = (string)args.Helper.ViewContext.RequestContext.RouteData.Values[CmsConstants.DETAILURL] ?? args.Options.Url;
            itemUrl = itemUrl != null ? itemUrl.ToLower() : null;

            this.SetBasicItems(args.Helper, args.Options);
            locator = this.GetLocator(locator.ContentTypeName, url, itemUrl == url && args.Options.Url != null ? args.Options.Url : itemUrl);
            this.SetActionItems(locator, args.Helper, args.Options, url, itemUrl);

            var htmlAttributes = !string.IsNullOrWhiteSpace(args.Options.Url) ? new { url = args.Options.Url } : null;
            args.Result = args.Helper.Action(args.Options.Action, args.Options.Controller, htmlAttributes);
        }

        #endregion Public Methods

        #region Private Methods

        private ContentLocator GetLocator(string contentTypeName, string controller, string url)
        {
            if (url != null)
            {
                url = url.ToLower();
            }

            var locators = _registrator.ContentLocators.Where(l => l.PageUrl.ToLower() == controller.ToLower() && (contentTypeName == null || l.ContentTypeName == contentTypeName)).ToList();

            if (!locators.Any())
            {
                return null;
            }

            return locators.Count > 1 ? controller == url ? locators.FirstOrDefault(l => l.PageUrl == url) : locators.FirstOrDefault(l => (url == null && l.ContentUrl == null) || l.ContentUrl == url) : locators.FirstOrDefault();
        }

        private void SetActionItems(ContentLocator locator, HtmlHelper helper, DisplayOptions options, string url, string itemUrl)
        {
            if (locator != null)
            {
                var isList = string.IsNullOrWhiteSpace(locator.ContentUrl);
                var items = helper.ViewContext.HttpContext.Items;

                if (isList && !string.IsNullOrWhiteSpace(itemUrl))
                {
                    if (itemUrl == url)
                    {
                        items[CmsConstants.DISPLAYTYPE] = CmsConstants.DISPLAYTYPELIST;
                    }
                    else
                    {
                        items[CmsConstants.ITEMURL] = itemUrl;
                    }
                }
                else
                {
                    if (options.HasChildPages && itemUrl != locator.PageUrl)
                    {
                        items[CmsConstants.CHILDURL] = itemUrl;
                    }
                }
            }
        }

        private void SetBasicItems(HtmlHelper helper, DisplayOptions options)
        {
            helper.ViewContext.HttpContext.Items[CmsConstants.MODULE] = null;
            helper.ViewContext.HttpContext.Items[CmsConstants.ITEMPAGEURL] = null;
            helper.ViewContext.HttpContext.Items[CmsConstants.DISPLAYTYPE] = null;
            helper.ViewContext.HttpContext.Items[CmsConstants.ITEMURL] = null;
            helper.ViewContext.HttpContext.Items[CmsConstants.CHILDURL] = null;

            if (!string.IsNullOrWhiteSpace(options.Module) && helper.ViewContext.RouteData.Values[MvcConstants.AREA] == null)
            {
                helper.ViewContext.HttpContext.Items[CmsConstants.MODULE] = options.Module;
            }

            if (!string.IsNullOrWhiteSpace(options.ItemPageUrl))
            {
                helper.ViewContext.HttpContext.Items[CmsConstants.ITEMPAGEURL] = options.ItemPageUrl;
            }
        }

        #endregion Private Methods
    }
}
#region Apache License

//-----------------------------------------------------------------------
// <copyright file="PageControllerHandler.cs" company="StrixIT">
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
using System.Web.Routing;

namespace StrixIT.Platform.Modules.Cms.EventHandlers
{
    public class PageControllerHandler : IHandlePlatformEvent<GetControllerEvent>
    {
        #region Public Methods

        public void Handle(GetControllerEvent args)
        {
            if (args.Stage == ControllerResolveStage.NotFound)
            {
                // Construct the required variables.
                var culture = StrixPlatform.CurrentCultureCode.ToLower();
                var controller = ((string)args.RequestContext.RouteData.Values[MvcConstants.CONTROLLER]).ToLower();
                var path = args.RequestContext.HttpContext.Request.Path.ToLower().Replace(string.Format("/{0}/", culture), string.Empty).Replace("/index", string.Empty);
                var url = path.Substring(path.LastIndexOf("/") + 1);

                // Get the content locator for the page and prepare the HttpContext.
                var location = this.GetPageUrl(args.RequestContext, path, false);

                // Create the page controller.
                var controllerType = typeof(PageController);
                args.RequestContext.RouteData.Values[MvcConstants.CONTROLLER] = CmsConstants.PAGE;
                args.RequestContext.RouteData.Values[MvcConstants.ACTION] = CmsConstants.RENDERPAGE;
                args.RequestContext.RouteData.Values[CmsConstants.URL] = location;
                args.RequestContext.RouteData.Values[CmsConstants.DETAILURL] = url;
                var pageController = args.DefaultControllerFactoryFunc(args.RequestContext, controllerType);
                args.Controller = pageController;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private string GetPageUrl(RequestContext requestContext, string path, bool retry)
        {
            string matchedUrl = null;

            foreach (var pathPart in path.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (PageRegistration.IsLocationRegistered(pathPart))
                {
                    matchedUrl = pathPart;
                    break;
                }
            }

            if (matchedUrl == null && !retry)
            {
                PageRegistration.LocatePages();
                matchedUrl = this.GetPageUrl(requestContext, path, true);
            }

            if (!retry)
            {
                PageRegistration.RegisterPage(requestContext.HttpContext, requestContext.RouteData, matchedUrl);
            }

            return matchedUrl;
        }

        #endregion Private Methods
    }
}
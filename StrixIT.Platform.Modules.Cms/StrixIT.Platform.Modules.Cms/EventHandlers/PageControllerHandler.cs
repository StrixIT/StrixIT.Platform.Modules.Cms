//-----------------------------------------------------------------------
// <copyright file="PageControllerHandler.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Web.Routing;
using StrixIT.Platform.Core;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms.EventHandlers
{
    public class PageControllerHandler : IHandlePlatformEvent<GetControllerEvent>
    {
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
    }
}
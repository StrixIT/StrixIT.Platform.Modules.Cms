//-----------------------------------------------------------------------
// <copyright file="PageRegistration.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    public static class PageRegistration
    {
        private static IPageRegistrator _registrator;

        /// <summary>
        /// Gets or sets the page registrator.
        /// </summary>
        public static IPageRegistrator Registrator
        {
            get
            {
                if (_registrator != null)
                {
                    return _registrator;
                }

                return new PageRegistrator();
            }
            set
            {
                _registrator = value;
            }
        }

        /// <summary>
        /// Gets the list of loaded content locators.
        /// </summary>
        public static IList<ContentLocator> ContentLocators
        {
            get
            {
                return Registrator.ContentLocators;
            }
        }

        /// <summary>
        /// Locates all custom page files for use when registering pages.
        /// </summary>
        public static void LocatePages()
        {
            Registrator.LocatePages();
        }

        /// <summary>
        /// Gets a value indicating whether a custom file page is registered for a url.
        /// </summary>
        /// <param name="location">The url to check the file page registration for</param>
        /// <returns>True if a file page is registered for the url, false otherwise</returns>
        public static bool IsLocationRegistered(string location)
        {
            return Registrator.IsLocationRegistered(location);
        }

        /// <summary>
        /// Registers all pages for which a file page is located with the Cms.
        /// </summary>
        /// <param name="httpContext">The HttpContext to use for rendering the page during the registration process</param>
        /// <param name="routeData">The RouteData to use for rendering the page during the registration process</param>
        public static void RegisterAllPages(HttpContextBase httpContext, RouteData routeData)
        {
            Registrator.RegisterAllPages(httpContext, routeData);
        }

        /// <summary>
        /// Registers a page for which a file page is located with the Cms.
        /// </summary>
        /// <param name="httpContext">The HttpContext to use for rendering the page during the registration process</param>
        /// <param name="routeData">The RouteData to use for rendering the page during the registration process</param>
        /// <param name="pagePath">The file page path to register</param>
        public static void RegisterPage(HttpContextBase httpContext, RouteData routeData, string pagePath)
        {
            Registrator.RegisterPage(httpContext, routeData, pagePath);
        }

        /// <summary>
        /// Registers custom content used on a custom page.
        /// </summary>
        /// <param name="helper">The html helper to use</param>
        /// <param name="options">The options of the content </param>
        /// <returns>The content locator for this url</returns>
        public static ContentLocator RegisterUrl(HtmlHelper helper, DisplayOptions options)
        {
            return Registrator.RegisterUrl(helper, options);
        }
    }
}
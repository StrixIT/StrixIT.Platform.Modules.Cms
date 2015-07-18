//-----------------------------------------------------------------------
// <copyright file="IPageRegistrator.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// An interface for registering custom pages.
    /// </summary>
    public interface IPageRegistrator
    {
        /// <summary>
        /// Gets the list of loaded content locators.
        /// </summary>
        IList<ContentLocator> ContentLocators { get; }

        /// <summary>
        /// Locates all custom page files for use when registering pages.
        /// </summary>
        void LocatePages();

        /// <summary>
        /// Gets a value indicating whether a custom file page is registered for a url.
        /// </summary>
        /// <param name="location">The url to check the file page registration for</param>
        /// <returns>True if a file page is registered for the url, false otherwise</returns>
        bool IsLocationRegistered(string location);

        /// <summary>
        /// Registers all pages for which a file page is located with the Cms.
        /// </summary>
        /// <param name="httpContext">The HttpContext to use for rendering the page during the registration process</param>
        /// <param name="routeData">The RouteData to use for rendering the page during the registration process</param>
        void RegisterAllPages(HttpContextBase httpContext, RouteData routeData);

        /// <summary>
        /// Registers a page for which a file page is located with the Cms.
        /// </summary>
        /// <param name="httpContext">The HttpContext to use for rendering the page during the registration process</param>
        /// <param name="routeData">The RouteData to use for rendering the page during the registration process</param>
        /// <param name="pagePath">The file page path to register</param>
        void RegisterPage(HttpContextBase httpContext, RouteData routeData, string pagePath);

        /// <summary>
        /// Registers custom content used on a custom page.
        /// </summary>
        /// <param name="helper">The html helper to use</param>
        /// <param name="options">The options of the content </param>
        /// <returns>The content locator for this url</returns>
        ContentLocator RegisterUrl(HtmlHelper helper, DisplayOptions options);
    }
}
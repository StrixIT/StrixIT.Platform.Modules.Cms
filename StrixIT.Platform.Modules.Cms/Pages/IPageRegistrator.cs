﻿#region Apache License

//-----------------------------------------------------------------------
// <copyright file="IPageRegistrator.cs" company="StrixIT">
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

using StrixIT.Platform.Web;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// An interface for registering custom pages.
    /// </summary>
    public interface IPageRegistrator
    {
        #region Public Properties

        /// <summary>
        /// Gets the list of loaded content locators.
        /// </summary>
        IList<ContentLocator> ContentLocators { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets a value indicating whether a custom file page is registered for a url.
        /// </summary>
        /// <param name="location">The url to check the file page registration for</param>
        /// <returns>True if a file page is registered for the url, false otherwise</returns>
        bool IsLocationRegistered(string location);

        /// <summary>
        /// Locates all custom page files for use when registering pages.
        /// </summary>
        void LocatePages();

        /// <summary>
        /// Registers all pages for which a file page is located with the Cms.
        /// </summary>
        /// <param name="httpContext">
        /// The HttpContext to use for rendering the page during the registration process
        /// </param>
        /// <param name="routeData">
        /// The RouteData to use for rendering the page during the registration process
        /// </param>
        void RegisterAllPages(HttpContextBase httpContext, RouteData routeData);

        /// <summary>
        /// Registers a page for which a file page is located with the Cms.
        /// </summary>
        /// <param name="httpContext">
        /// The HttpContext to use for rendering the page during the registration process
        /// </param>
        /// <param name="routeData">
        /// The RouteData to use for rendering the page during the registration process
        /// </param>
        /// <param name="pagePath">The file page path to register</param>
        void RegisterPage(HttpContextBase httpContext, RouteData routeData, string pagePath);

        /// <summary>
        /// Registers custom content used on a custom page.
        /// </summary>
        /// <param name="helper">The html helper to use</param>
        /// <param name="options">The options of the content</param>
        /// <returns>The content locator for this url</returns>
        ContentLocator RegisterUrl(HtmlHelper helper, DisplayOptions options);

        #endregion Public Methods
    }
}
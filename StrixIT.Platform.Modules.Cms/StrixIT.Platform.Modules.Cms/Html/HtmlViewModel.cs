//-----------------------------------------------------------------------
// <copyright file="HtmlViewModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Web.Mvc;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The view model for Html.
    /// </summary>
    public class HtmlViewModel : EntityViewModel
    {
        /// <summary>
        /// Gets or sets the Html body.
        /// </summary>
        [AllowHtml]
        public string Body { get; set; }
    }
}
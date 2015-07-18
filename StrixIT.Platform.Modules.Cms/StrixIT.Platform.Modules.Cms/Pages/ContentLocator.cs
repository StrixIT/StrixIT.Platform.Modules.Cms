//-----------------------------------------------------------------------
// <copyright file="ContentLocator.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A class to store content locations.
    /// </summary>
    public class ContentLocator
    {
        /// <summary>
        ///  Gets or sets the name of the type of the content.
        /// </summary>
        public string ContentTypeName { get; set; }

        /// <summary>
        ///  Gets or sets the url of the page the content is located on.
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        ///  Gets or sets the url of the content.
        /// </summary>
        public string ContentUrl { get; set; }
    }
}
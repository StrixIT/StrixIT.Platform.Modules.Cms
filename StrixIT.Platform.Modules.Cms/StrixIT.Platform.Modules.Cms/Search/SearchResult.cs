//-----------------------------------------------------------------------
// <copyright file="SearchResult.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A class to collect entity search results.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResult" /> class.
        /// </summary>
        public SearchResult()
        {
            this.Data = new List<SearchItem>();
            this.Locators = new List<ContentLocator>();
        }

        /// <summary>
        ///  Gets or sets the total amount of content items in the data store.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the retrieved content items.
        /// </summary>
        public IList<SearchItem> Data { get; set; }

        /// <summary>
        /// Gets or sets the content locators registered by the platform.
        /// </summary>
        public IList<ContentLocator> Locators { get; set; }
    }
}
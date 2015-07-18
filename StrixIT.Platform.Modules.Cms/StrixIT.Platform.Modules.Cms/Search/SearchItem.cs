//-----------------------------------------------------------------------
// <copyright file="SearchItem.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A class for an entity search result.
    /// </summary>
    public class SearchItem
    {
        /// <summary>
        /// Gets or sets the content entity id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the content entity url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the content name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the full type name of the content.
        /// </summary>
        public string TypeName { get; set; }
    }
}
//-----------------------------------------------------------------------
// <copyright file="ISearchService.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface for the search service, which searches all registered content types.
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Get a result containing a list of entities that match the search criteria.
        /// </summary>
        /// <param name="options">The filter to use when searching</param>
        /// <returns>The search results</returns>
        SearchResult Search(FilterOptions options);
    }
}
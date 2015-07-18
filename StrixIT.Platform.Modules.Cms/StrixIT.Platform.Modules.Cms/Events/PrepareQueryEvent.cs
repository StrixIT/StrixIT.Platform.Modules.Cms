//-----------------------------------------------------------------------
// <copyright file="PrepareQueryEvent.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Linq;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// An event to allow modifying a query before it is executed against the datasource.
    /// </summary>
    public class PrepareQueryEvent : IPlatformEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrepareQueryEvent" /> class.
        /// </summary>
        /// <param name="query">The current query</param>
        /// <param name="filter">The current filter options</param>
        public PrepareQueryEvent(IQueryable query, FilterOptions filter) : this(query, filter, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrepareQueryEvent" /> class.
        /// </summary>
        /// <param name="query">The current query</param>
        /// <param name="filter">The current filter options</param>
        /// <param name="page">True if the query results should be paged, false otherwise</param>
        public PrepareQueryEvent(IQueryable query, FilterOptions filter, bool page)
        {
            this.Query = query;
            this.Filter = filter;
            this.Page = page;
        }

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        public IQueryable Query { get; set; }

        /// <summary>
        /// Gets the filter options.
        /// </summary>
        public FilterOptions Filter { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the query is to be paged.
        /// </summary>
        public bool Page { get; private set; }
    }
}
#region Apache License

//-----------------------------------------------------------------------
// <copyright file="SearchResult.cs" company="StrixIT">
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

using System.Collections.Generic;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A class to collect entity search results.
    /// </summary>
    public class SearchResult
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResult"/> class.
        /// </summary>
        public SearchResult()
        {
            this.Data = new List<SearchItem>();
            this.Locators = new List<ContentLocator>();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the retrieved content items.
        /// </summary>
        public IList<SearchItem> Data { get; set; }

        /// <summary>
        /// Gets or sets the content locators registered by the platform.
        /// </summary>
        public IList<ContentLocator> Locators { get; set; }

        /// <summary>
        /// Gets or sets the total amount of content items in the data store.
        /// </summary>
        public int Total { get; set; }

        #endregion Public Properties
    }
}
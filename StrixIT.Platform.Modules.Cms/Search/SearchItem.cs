#region Apache License

//-----------------------------------------------------------------------
// <copyright file="SearchItem.cs" company="StrixIT">
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

using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A class for an entity search result.
    /// </summary>
    public class SearchItem
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the content entity id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the content name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the full type name of the content.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the content entity url.
        /// </summary>
        public string Url { get; set; }

        #endregion Public Properties
    }
}
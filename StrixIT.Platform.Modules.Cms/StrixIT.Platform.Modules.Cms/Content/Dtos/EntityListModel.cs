#region Apache License
//-----------------------------------------------------------------------
// <copyright file="EntityListModel.cs" company="StrixIT">
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
#endregion

using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The base view model for all content list pages.
    /// </summary>
    public abstract class EntityListModel : AuditViewModel
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
        /// Gets or sets the date and time of the last comment for this content.
        /// </summary>
        public DateTime? LastCommentDate { get; set; }

        /// <summary>
        /// Gets or sets the number of comments for this content.
        /// </summary>
        public int NumberOfComments { get; set; }

        /// <summary>
        /// Gets or sets the content sort order.
        /// </summary>
        public int? SortOrder { get; set; }
    }
}
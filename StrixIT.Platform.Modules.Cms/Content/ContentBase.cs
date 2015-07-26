#region Apache License
//-----------------------------------------------------------------------
// <copyright file="ContentBase.cs" company="StrixIT">
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
using System.ComponentModel.DataAnnotations;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The base class for all Cms content.
    /// </summary>
    public abstract class ContentBase : ValidationBase, IAudit
    {
        /// <summary>
        /// Gets or sets the id of this content.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the content culture.
        /// </summary>
        [StrixRequired]
        [StringLength(5)]
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the content version number.
        /// </summary>
        [StrixRequired]
        public int VersionNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the Entity.
        /// </summary>
        [StrixRequired]
        [StringLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the log message for this content version.
        /// </summary>
        [StringLength(1000)]
        public string VersionLog { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this content version is the currently active one. When a version is restored, this flag is
        /// set on the version restored instead of creating a new version with all the previous values.
        /// </summary>
        public bool IsCurrentVersion { get; set; }

        #region Comments

        /// <summary>
        /// Gets or sets the number of comments for this content.
        /// </summary>
        public int NumberOfComments { get; set; }

        /// <summary>
        /// Gets or sets the date of the last comment for this content.
        /// </summary>
        [StrixNotDefault]
        public DateTime? LastCommentDate { get; set; }

        #endregion

        #region Audit

        /// <summary>
        /// Gets or sets the id of the user who created this content.
        /// </summary>
        [StrixRequiredWithMembershipAttribute]
        public Guid CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the user who created this content.
        /// </summary>
        public UserData CreatedByUser { get; set; }

        /// <summary>
        /// Gets or sets the date and time this content was created.
        /// </summary>
        [StrixRequired]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who last updated this content.
        /// </summary>
        [StrixRequiredWithMembershipAttribute]
        public Guid UpdatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the user who last updated this content.
        /// </summary>
        public UserData UpdatedByUser { get; set; }

        /// <summary>
        /// Gets or sets the date and time this content was last updated.
        /// </summary>
        [StrixRequired]
        public DateTime UpdatedOn { get; set; }

        #endregion

        #region Editing

        /// <summary>
        /// Gets or sets the id of the user who published this content, when the publish date is reached.
        /// </summary>
        [StrixNotDefaultWithMembership]
        public Guid? PublishedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the user who published this content.
        /// </summary>
        public UserData PublishedByUser { get; set; }

        /// <summary>
        /// Gets or sets the date and time this content version was published. If NULL, the content is not published yet. If it is in the future,
        /// the entity is not yet available for viewing.
        /// </summary>
        [StrixNotDefault]
        public DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who deleted this content, if the content is deleted.
        /// </summary>
        [StrixNotDefaultWithMembership]
        public Guid? DeletedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the user who deleted this content.
        /// </summary>
        public UserData DeletedByUser { get; set; }

        /// <summary>
        /// Gets or sets the date and time this content was deleted.
        /// </summary>
        [StrixNotDefault]
        public DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who locked this content for editing, if the content is locked.
        /// </summary>
        [StrixNotDefaultWithMembership]
        public Guid? LockedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the user who locked this content for editing.
        /// </summary>
        public UserData LockedByUser { get; set; }

        /// <summary>
        /// Gets or sets the date and time this content was locked for editing.
        /// </summary>
        [StrixNotDefault]
        public DateTime? LockedOn { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets the sort order of this content. Sort order is global per content type.
        /// </summary>
        public int SortOrder { get; set; }
    }
}

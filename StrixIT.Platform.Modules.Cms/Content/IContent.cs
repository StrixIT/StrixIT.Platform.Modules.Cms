#region Apache License

//-----------------------------------------------------------------------
// <copyright file="IContent.cs" company="StrixIT">
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

using StrixIT.Platform.Core;
using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface for cms content.
    /// </summary>
    public interface IContent : IAudit
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the content culture.
        /// </summary>
        string Culture { get; set; }

        /// <summary>
        /// Gets or sets the content entity.
        /// </summary>
        PlatformEntity Entity { get; set; }

        /// <summary>
        /// Gets or sets the content entity id.
        /// </summary>
        Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this content version is the currently active
        /// one. When a version is restored, this flag is set on the version restored instead of
        /// creating a new version with all the previous values.
        /// </summary>
        bool IsCurrentVersion { get; set; }

        /// <summary>
        /// Gets or sets the name of the Entity.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the sort order of this content. Sort order is global per content type.
        /// </summary>
        int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the log message for this content version.
        /// </summary>
        string VersionLog { get; set; }

        /// <summary>
        /// Gets or sets the content version number.
        /// </summary>
        int VersionNumber { get; set; }

        #endregion Public Properties

        #region Comments

        /// <summary>
        /// Gets or sets the date of the last comment for this content.
        /// </summary>
        DateTime? LastCommentDate { get; set; }

        /// <summary>
        /// Gets or sets the number of comments for this content.
        /// </summary>
        int NumberOfComments { get; set; }

        #endregion Comments

        #region Editing

        /// <summary>
        /// Gets or sets the user who deleted this content.
        /// </summary>
        UserData DeletedByUser { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who deleted this entity, if the entity is deleted.
        /// </summary>
        Guid? DeletedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time this entity was deleted.
        /// </summary>
        DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Gets or sets the user who locked this content for editing.
        /// </summary>
        UserData LockedByUser { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who locked this content for editing, if the content is locked.
        /// </summary>
        Guid? LockedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time this content was locked for editing.
        /// </summary>
        DateTime? LockedOn { get; set; }

        /// <summary>
        /// Gets or sets the user who published this content.
        /// </summary>
        UserData PublishedByUser { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who published this content, when the publish date is reached.
        /// </summary>
        Guid? PublishedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time this content version was published. If NULL, the content
        /// is not published yet. If it is in the future, the entity is not yet available for viewing.
        /// </summary>
        DateTime? PublishedOn { get; set; }

        #endregion Editing
    }
}
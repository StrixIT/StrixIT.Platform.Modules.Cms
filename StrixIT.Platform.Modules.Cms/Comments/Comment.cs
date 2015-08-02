#region Apache License

//-----------------------------------------------------------------------
// <copyright file="Comment.cs" company="StrixIT">
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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A comment that can be added to an entity.
    /// </summary>
    public class Comment
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the child comments of this comment.
        /// </summary>
        public virtual ICollection<Comment> Children { get; set; }

        /// <summary>
        /// Gets or sets the status of this comment.
        /// </summary>
        public CommentStatus CommentStatus { get; set; }

        /// <summary>
        /// Gets or sets the user who created this comment.
        /// </summary>
        public UserData CreatedByUser { get; set; }

        /// <summary>
        /// Gets or sets the Id of the User creating this Comment.
        /// </summary>
        [StrixRequiredWithMembershipAttribute]
        public Guid CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time this comment was created on.
        /// </summary>
        [StrixRequired]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the entity this comment is for.
        /// </summary>
        public PlatformEntity Entity { get; set; }

        /// <summary>
        /// Gets or sets the culture of the entity this comment is for.
        /// </summary>
        [StrixRequired]
        [StringLength(5)]
        public string EntityCulture { get; set; }

        /// <summary>
        /// Gets or sets the id of the entity this comment is for.
        /// </summary>
        [StrixRequired]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the version number of the entity this comment is for.
        /// </summary>
        [StrixRequired]
        public int EntityVersion { get; set; }

        /// <summary>
        /// Gets or sets the comment id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the parent of this comment.
        /// </summary>
        public virtual Comment Parent { get; set; }

        /// <summary>
        /// Gets or sets the id of the parent of this comment.
        /// </summary>
        [StrixNotDefault]
        public long? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        [StrixRequired]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the user who last updated this comment.
        /// </summary>
        public UserData UpdatedByUser { get; set; }

        /// <summary>
        /// Gets or sets the Id of the user who last updated this Comment.
        /// </summary>
        [StrixRequiredWithMembershipAttribute]
        public Guid UpdatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time this comment was updated on.
        /// </summary>
        [StrixRequired]
        public DateTime UpdatedOn { get; set; }

        #endregion Public Properties
    }
}
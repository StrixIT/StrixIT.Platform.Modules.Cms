#region Apache License
//-----------------------------------------------------------------------
// <copyright file="CommentViewModel.cs" company="StrixIT">
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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The view model for comments.
    /// </summary>
    public class CommentViewModel
    {
        /// <summary>
        /// Gets or sets the comment Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the id of the entity type the comment is for.
        /// </summary>
        public Guid EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the id of the entity the comment is for.
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the parent Id of this comment.
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the the culture of the entity this comment is for.
        /// </summary>
        public string EntityCulture { get; set; }

        /// <summary>
        /// Gets or sets the version of the entity this comment is for.
        /// </summary>
        public int EntityVersion { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who created this version.
        /// </summary>
        public Guid CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets name of the user who created this comment.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets email of the user who created this comment.
        /// </summary>
        public string CreatedByEmail { get; set; }

        /// <summary>
        /// Gets or sets the date and time this comment was created.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who last updated this version.
        /// </summary>
        public Guid UpdatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets name of the user who last updated this comment.
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time this comment was last updated.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        [StrixRequired]
        [AllowHtml]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the child comments of this comment.
        /// </summary>
        public ICollection<CommentViewModel> ChildComments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can edit this comment.
        /// </summary>
        public bool CanEdit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can delete this comment.
        /// </summary>
        public bool CanDelete { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can respond to this comment.
        /// </summary>
        public bool CanRespond { get; set; }
    }
}
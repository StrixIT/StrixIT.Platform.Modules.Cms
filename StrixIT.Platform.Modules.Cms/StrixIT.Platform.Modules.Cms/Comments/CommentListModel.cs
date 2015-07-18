//-----------------------------------------------------------------------
// <copyright file="CommentListModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using StrixIT.Platform.Modules.Cms;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The list model for comments.
    /// </summary>
    public class CommentListModel
    {
        /// <summary>
        /// Gets or sets the comment id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the key of the entity the comment is for.
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the culture id of the entity the comment is for.
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the current version number of the entity the comment is for.
        /// </summary>
        public int VersionNumber { get; set; }

        /// <summary>
        /// Gets or sets the list of child comments of this comment.
        /// </summary>
        public IList<CommentViewModel> ChildComments { get; set; }
    }
}
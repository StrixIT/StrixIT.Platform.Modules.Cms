//-----------------------------------------------------------------------
// <copyright file="EntityListModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
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
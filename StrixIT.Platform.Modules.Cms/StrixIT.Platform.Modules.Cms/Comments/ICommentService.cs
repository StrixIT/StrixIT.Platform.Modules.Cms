//-----------------------------------------------------------------------
// <copyright file="ICommentService.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The comment service interface.
    /// </summary>
    public interface ICommentService
    {
        CommentListModel GetComments(Guid entityId);

        /// <summary>
        /// Gets a list of comments for an entity.
        /// </summary>
        /// <param name="entityId">The id of the entity to get the comments for</param>
        /// <param name="culture">The content culture to get the comments for. If omitted, the current culture will be used</param>
        /// <returns>The comment list</returns>
        CommentListModel GetComments(Guid entityId, string culture);

        /// <summary>
        /// Saves a comment.
        /// </summary>
        /// <param name="model">The model to save</param>
        /// <returns>The saved view model</returns>
        CommentViewModel SaveComment(CommentViewModel model);

        /// <summary>
        /// Deletes a comment.
        /// </summary>
        /// <param name="model">The model to delete</param>
        /// <returns>True if deletion was successful, false otherwise</returns>
        bool DeleteComment(CommentViewModel model);
    }
}
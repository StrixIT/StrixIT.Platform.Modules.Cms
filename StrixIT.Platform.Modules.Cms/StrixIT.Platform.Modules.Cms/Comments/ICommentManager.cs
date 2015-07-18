//-----------------------------------------------------------------------
// <copyright file="ICommentManager.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface for the platform comment manager.
    /// </summary>
    public interface ICommentManager
    {
        IList<Comment> GetComments(Guid entityId);

        /// <summary>
        /// Gets all comments for an entity
        /// </summary>
        /// <param name="entityId">The id of the entity to get the comments for</param>
        /// <param name="culture">The culture of the entity to get the comments for</param>
        /// <returns>The comments</returns>
        IList<Comment> GetComments(Guid entityId, string culture);

        /// <summary>
        /// Adds a new comment for an entity.
        /// </summary>
        /// <param name="entityTypeId">The id of the entity type to add the comment for</param>
        /// <param name="comment">The comment to add</param>
        /// <returns>The added comment</returns>
        Comment AddComment(Guid entityTypeId, Comment comment);

        /// <summary>
        /// Updates a comment for an entity.
        /// </summary>
        /// <param name="comment">The comment to update</param>
        /// <returns>True if the comment was updated successfully, false otherwise</returns>
        Comment EditComment(Comment comment);

        /// <summary>
        /// Deletes a comment from an entity.
        /// </summary>
        /// <param name="comment">The comment</param>
        /// <param name="entityTypeId">The id of the entity type the comment is for</param>
        /// <returns>True if the comment was deleted successfully, false otherwise</returns>
        bool DeleteComment(Comment comment, Guid entityTypeId);

        /// <summary>
        /// Gets a value indicating whether the comment with the specified id has child comments.
        /// </summary>
        /// <param name="commentId">The id of the comments to check the children for</param>
        /// <returns>True if the comment has children, false otherwise</returns>
        bool HasChildComments(long commentId);
    }
}
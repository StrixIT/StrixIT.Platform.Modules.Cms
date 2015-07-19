#region Apache License
//-----------------------------------------------------------------------
// <copyright file="ICommentService.cs" company="StrixIT">
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
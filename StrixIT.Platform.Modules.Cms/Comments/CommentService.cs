﻿#region Apache License

//-----------------------------------------------------------------------
// <copyright file="CommentService.cs" company="StrixIT">
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
using System.Linq;

namespace StrixIT.Platform.Modules.Cms
{
    public class CommentService : ICommentService
    {
        #region Private Fields

        private ICommentManager _commentManager;
        private IPlatformDataSource _dataSource;

        #endregion Private Fields

        #region Public Constructors

        public CommentService(IPlatformDataSource dataSource, ICommentManager commentManager)
        {
            this._dataSource = dataSource;
            this._commentManager = commentManager;
        }

        #endregion Public Constructors

        #region Public Methods

        public bool DeleteComment(CommentViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            var result = this._commentManager.DeleteComment(model.Map<Comment>(), model.EntityTypeId);
            this._dataSource.SaveChanges();
            return result;
        }

        public CommentListModel GetComments(Guid entityId)
        {
            return this.GetComments(entityId, null);
        }

        public CommentListModel GetComments(Guid entityId, string culture)
        {
            var comments = this._commentManager.GetComments(entityId, culture);
            CommentListModel list = new CommentListModel();
            list.EntityId = entityId;
            list.Culture = culture;
            list.ChildComments = this.CreateCommentTree(comments, null);
            return list;
        }

        public CommentViewModel SaveComment(CommentViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            Comment comment;

            if (model.Id == 0)
            {
                comment = this._commentManager.AddComment(model.EntityTypeId, model.Map<Comment>());
            }
            else
            {
                comment = this._commentManager.EditComment(model.Map<Comment>());
            }

            this._dataSource.SaveChanges();

            var userId = StrixPlatform.User.Id;
            var isAdmin = StrixPlatform.User.IsAdministrator;
            var hasChildren = this._commentManager.HasChildComments(comment.Id);

            model = comment.Map<CommentViewModel>();

            model.CanEdit = !hasChildren && (userId == comment.CreatedByUserId || isAdmin);
            model.CanDelete = !hasChildren && (userId == comment.CreatedByUserId || isAdmin);
            model.CanRespond = userId != Guid.Empty;
            return model;
        }

        #endregion Public Methods

        #region Private Methods

        private IList<CommentViewModel> CreateCommentTree(ICollection<Comment> comments, long? parentId)
        {
            List<CommentViewModel> list = new List<CommentViewModel>();
            var userId = StrixPlatform.User.Id;
            var isAdmin = StrixPlatform.User.IsAdministrator;

            foreach (Comment comment in comments.Where(co => parentId.HasValue ? co.ParentId == parentId.Value : co.ParentId == null))
            {
                CommentViewModel commentModel = comment.Map<CommentViewModel>();
                IList<Comment> subComments = comments.Where(co => co.ParentId == comment.Id).ToList();
                commentModel.CanEdit = subComments.Count == 0 && (userId == commentModel.CreatedByUserId || isAdmin);
                commentModel.CanDelete = subComments.Count == 0 && (userId == commentModel.CreatedByUserId || isAdmin);
                commentModel.CanRespond = userId != Guid.Empty;

                if (subComments.Count > 0)
                {
                    commentModel.ChildComments = this.CreateCommentTree(comments, comment.Id);
                }

                list.Add(commentModel);
            }

            return list;
        }

        #endregion Private Methods
    }
}
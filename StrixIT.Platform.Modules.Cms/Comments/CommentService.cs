#region Apache License

//-----------------------------------------------------------------------
// <copyright file="CommentService.cs" company="StrixIT">
// Copyright 2015 StrixIT. Author R.G. Schurgers MA MSc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use file except in compliance with the License.
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
        private IUserContext _user;

        #endregion Private Fields

        #region Public Constructors

        public CommentService(IPlatformDataSource dataSource, ICommentManager commentManager, IUserContext user)
        {
            _dataSource = dataSource;
            _commentManager = commentManager;
            _user = user;
        }

        #endregion Public Constructors

        #region Public Methods

        public bool DeleteComment(CommentViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            var result = _commentManager.DeleteComment(model.Map<Comment>(), model.EntityTypeId);
            _dataSource.SaveChanges();
            return result;
        }

        public CommentListModel GetComments(Guid entityId)
        {
            return GetComments(entityId, null);
        }

        public CommentListModel GetComments(Guid entityId, string culture)
        {
            var comments = _commentManager.GetComments(entityId, culture);
            CommentListModel list = new CommentListModel();
            list.EntityId = entityId;
            list.Culture = culture;
            list.ChildComments = CreateCommentTree(comments, null);
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
                comment = _commentManager.AddComment(model.EntityTypeId, model.Map<Comment>());
            }
            else
            {
                comment = _commentManager.EditComment(model.Map<Comment>());
            }

            _dataSource.SaveChanges();

            var userId = _user.Id;
            var isAdmin = _user.IsAdministrator;
            var hasChildren = _commentManager.HasChildComments(comment.Id);

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
            var userId = _user.Id;
            var isAdmin = _user.IsAdministrator;

            foreach (Comment comment in comments.Where(co => parentId.HasValue ? co.ParentId == parentId.Value : co.ParentId == null))
            {
                CommentViewModel commentModel = comment.Map<CommentViewModel>();
                IList<Comment> subComments = comments.Where(co => co.ParentId == comment.Id).ToList();
                commentModel.CanEdit = subComments.Count == 0 && (userId == commentModel.CreatedByUserId || isAdmin);
                commentModel.CanDelete = subComments.Count == 0 && (userId == commentModel.CreatedByUserId || isAdmin);
                commentModel.CanRespond = userId != Guid.Empty;

                if (subComments.Count > 0)
                {
                    commentModel.ChildComments = CreateCommentTree(comments, comment.Id);
                }

                list.Add(commentModel);
            }

            return list;
        }

        #endregion Private Methods
    }
}
//-----------------------------------------------------------------------
// <copyright file="CommentService.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class CommentService : ICommentService
    {
        private IPlatformDataSource _dataSource;

        private ICommentManager _commentManager;

        public CommentService(IPlatformDataSource dataSource, ICommentManager commentManager)
        {
            this._dataSource = dataSource;
            this._commentManager = commentManager;
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
    }
}
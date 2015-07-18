//-----------------------------------------------------------------------
// <copyright file="CommentManager.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using StructureMap;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class CommentManager : ICommentManager
    {
        private const string GETCOMMENTQUERY = "EntityId.Equals(@0) AND Culture.ToLower().Equals(@1) And IsCurrentVersion";

        private IPlatformDataSource _dataSource;

        public CommentManager(IPlatformDataSource dataSource)
        {
            this._dataSource = dataSource;
        }

        public IList<Comment> GetComments(Guid entityId)
        {
            return this.GetComments(entityId, null);
        }

        public IList<Comment> GetComments(Guid entityId, string culture)
        {
            culture = culture ?? StrixPlatform.CurrentCultureCode;
            var query = this._dataSource.Query<Comment>().Where(co => co.EntityId == entityId && (culture == null || co.EntityCulture.ToLower() == culture.ToLower()));
            return query.ToList();
        }

        public Comment AddComment(Guid entityTypeId, Comment comment)
        {
            Type entityType = EntityHelper.GetEntityType(entityTypeId);

            if (!EntityHelper.IsServiceActive(entityType, EntityServiceActions.AllowNestedComments))
            {
                comment.ParentId = null;
            }

            if (EntityHelper.IsServiceActive(entityType, EntityServiceActions.ModerateComments))
            {
                comment.CommentStatus = CommentStatus.New;
            }
            else
            {
                comment.CommentStatus = CommentStatus.Approved;
            }

            if (string.IsNullOrWhiteSpace(comment.EntityCulture))
            {
                comment.EntityCulture = StrixPlatform.CurrentCultureCode;
            }

            var content = this._dataSource.Query(entityType).Where(GETCOMMENTQUERY, comment.EntityId, comment.EntityCulture.ToLower()).GetFirst() as IContent;

            if (comment.EntityVersion == 0)
            {
                comment.EntityVersion = content.VersionNumber;
            }

            comment.CreatedByUserId = StrixPlatform.User.Id;
            comment.CreatedOn = DateTime.Now;
            comment.UpdatedByUserId = StrixPlatform.User.Id;
            comment.UpdatedOn = comment.CreatedOn;
            var result = this._dataSource.Save(comment);

            if (result != null)
            {
                var numberOfComments = this._dataSource.Query<Comment>().Where(c => c.EntityId == comment.EntityId && c.EntityCulture.ToLower() == comment.EntityCulture.ToLower()).Count() + 1;
                content.NumberOfComments = numberOfComments;
                content.LastCommentDate = DateTime.Now;
            }

            return result;
        }

        public Comment EditComment(Comment comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException("comment");
            }

            var theComment = this._dataSource.Query<Comment>().FirstOrDefault(c => c.Id == comment.Id);

            if (theComment == null)
            {
                Logger.Log(string.Format("No comment found with id {0}", comment.Id), LogLevel.Error);
                return null;
            }

            theComment.Text = comment.Text;
            theComment.UpdatedByUserId = StrixPlatform.User.Id;
            theComment.UpdatedOn = DateTime.Now;
            return theComment;
        }

        public bool DeleteComment(Comment comment, Guid entityTypeId)
        {
            if (comment == null)
            {
                throw new ArgumentNullException("comment");
            }

            var theComment = this._dataSource.Query<Comment>().FirstOrDefault(c => c.Id == comment.Id);

            if (theComment == null)
            {
                Logger.Log(string.Format("No comment found with id {0}", comment.Id), LogLevel.Error);
                return false;
            }

            Type entityType = EntityHelper.GetEntityType(entityTypeId);
            var content = this._dataSource.Query(entityType).Where(GETCOMMENTQUERY, comment.EntityId, comment.EntityCulture.ToLower()).GetFirst() as IContent;

            if (content == null)
            {
                Logger.Log(string.Format("No content found with id {0}", comment.EntityId), LogLevel.Error);
                return false;
            }

            // Update the number of comments and the last comment date on the entity the comment is for.
            var query = this._dataSource.Query<Comment>().Where(c => c.EntityId == comment.EntityId && c.EntityCulture.ToLower() == comment.EntityCulture.ToLower() && c.Id != comment.Id).OrderByDescending(c => c.CreatedOn);
            content.NumberOfComments = query.Count();
            Comment lastComment = query.FirstOrDefault();

            if (lastComment != null)
            {
                content.LastCommentDate = lastComment.CreatedOn;
            }
            else
            {
                content.LastCommentDate = null;
            }

            this._dataSource.Delete(theComment);
            return true;
        }

        public bool HasChildComments(long commentId)
        {
            return this._dataSource.Query<Comment>().Any(c => c.ParentId.HasValue && c.ParentId.Value == commentId);
        }
    }
}
#region Apache License

//-----------------------------------------------------------------------
// <copyright file="CommentManager.cs" company="StrixIT">
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
using StrixIT.Platform.Core.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace StrixIT.Platform.Modules.Cms
{
    public class CommentManager : ICommentManager
    {
        #region Private Fields

        private const string GETCOMMENTQUERY = "EntityId.Equals(@0) AND Culture.ToLower().Equals(@1) And IsCurrentVersion";

        private IPlatformDataSource _dataSource;
        private IEntityHelper _entityHelper;
        private IEnvironment _environment;

        #endregion Private Fields

        #region Public Constructors

        public CommentManager(IPlatformDataSource dataSource, IEnvironment environment, IEntityHelper entityHelper)
        {
            _dataSource = dataSource;
            _environment = environment;
            _entityHelper = entityHelper;
        }

        #endregion Public Constructors

        #region Public Methods

        public Comment AddComment(Guid entityTypeId, Comment comment)
        {
            Type entityType = _entityHelper.GetEntityType(entityTypeId);

            if (!_entityHelper.IsServiceActive(entityType, EntityServiceActions.AllowNestedComments))
            {
                comment.ParentId = null;
            }

            if (_entityHelper.IsServiceActive(entityType, EntityServiceActions.ModerateComments))
            {
                comment.CommentStatus = CommentStatus.New;
            }
            else
            {
                comment.CommentStatus = CommentStatus.Approved;
            }

            if (string.IsNullOrWhiteSpace(comment.EntityCulture))
            {
                comment.EntityCulture = _environment.Cultures.CurrentCultureCode;
            }

            var content = _dataSource.Query(entityType).Where(GETCOMMENTQUERY, comment.EntityId, comment.EntityCulture.ToLower()).GetFirst() as IContent;

            if (comment.EntityVersion == 0)
            {
                comment.EntityVersion = content.VersionNumber;
            }

            comment.CreatedByUserId = _environment.User.Id;
            comment.CreatedOn = DateTime.Now;
            comment.UpdatedByUserId = _environment.User.Id;
            comment.UpdatedOn = comment.CreatedOn;
            var result = _dataSource.Save(comment);

            if (result != null)
            {
                var numberOfComments = _dataSource.Query<Comment>().Where(c => c.EntityId == comment.EntityId && c.EntityCulture.ToLower() == comment.EntityCulture.ToLower()).Count() + 1;
                content.NumberOfComments = numberOfComments;
                content.LastCommentDate = DateTime.Now;
            }

            return result;
        }

        public bool DeleteComment(Comment comment, Guid entityTypeId)
        {
            if (comment == null)
            {
                throw new ArgumentNullException("comment");
            }

            var theComment = _dataSource.Query<Comment>().FirstOrDefault(c => c.Id == comment.Id);

            if (theComment == null)
            {
                Logger.Log(string.Format("No comment found with id {0}", comment.Id), LogLevel.Error);
                return false;
            }

            Type entityType = _entityHelper.GetEntityType(entityTypeId);
            var content = _dataSource.Query(entityType).Where(GETCOMMENTQUERY, comment.EntityId, comment.EntityCulture.ToLower()).GetFirst() as IContent;

            if (content == null)
            {
                Logger.Log(string.Format("No content found with id {0}", comment.EntityId), LogLevel.Error);
                return false;
            }

            // Update the number of comments and the last comment date on the entity the comment is for.
            var query = _dataSource.Query<Comment>().Where(c => c.EntityId == comment.EntityId && c.EntityCulture.ToLower() == comment.EntityCulture.ToLower() && c.Id != comment.Id).OrderByDescending(c => c.CreatedOn);
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

            _dataSource.Delete(theComment);
            return true;
        }

        public Comment EditComment(Comment comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException("comment");
            }

            var theComment = _dataSource.Query<Comment>().FirstOrDefault(c => c.Id == comment.Id);

            if (theComment == null)
            {
                Logger.Log(string.Format("No comment found with id {0}", comment.Id), LogLevel.Error);
                return null;
            }

            theComment.Text = comment.Text;
            theComment.UpdatedByUserId = _environment.User.Id;
            theComment.UpdatedOn = DateTime.Now;
            return theComment;
        }

        public IList<Comment> GetComments(Guid entityId)
        {
            return GetComments(entityId, null);
        }

        public IList<Comment> GetComments(Guid entityId, string culture)
        {
            culture = culture ?? _environment.Cultures.CurrentCultureCode;
            var query = _dataSource.Query<Comment>().Where(co => co.EntityId == entityId && (culture == null || co.EntityCulture.ToLower() == culture.ToLower()));
            return query.ToList();
        }

        public bool HasChildComments(long commentId)
        {
            return _dataSource.Query<Comment>().Any(c => c.ParentId.HasValue && c.ParentId.Value == commentId);
        }

        #endregion Public Methods
    }
}
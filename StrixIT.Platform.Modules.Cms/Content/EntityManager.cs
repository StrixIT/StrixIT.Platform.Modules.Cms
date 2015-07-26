#region Apache License

//-----------------------------------------------------------------------
// <copyright file="EntityManager.cs" company="StrixIT">
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
using StrixIT.Platform.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace StrixIT.Platform.Modules.Cms
{
    public class EntityManager : ObjectManager, IEntityManager
    {
        #region Private Fields

        private static readonly string[] PropertiesToIgnoreForVersioning = new string[] { "UpdatedByUserId", "UpdatedOn", "PublishedOn", "NumberOfComments", "LastCommentDate", "VersionLog", "SortOrder", "DeletedByUserId", "DeletedOn" };
        private ICacheService _cache;

        #endregion Private Fields

        #region Public Constructors

        public EntityManager(IPlatformDataSource dataSource, ICacheService cache) : base(dataSource)
        {
            this._cache = cache;
        }

        #endregion Public Constructors

        #region IsNameAvailable

        public bool IsNameAvailable(Type contentType, string name, Guid id)
        {
            return this.IsNameAvailable(contentType, name, id, null);
        }

        public bool IsNameAvailable(Type contentType, string name, Guid id, string culture)
        {
            if (!typeof(IContent).IsAssignableFrom(contentType))
            {
                throw new NotSupportedException("CheckName is supported for types that implement IContent only.");
            }

            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (EntityHelper.IsServiceActive(contentType, EntityServiceActions.AllowNonUniqueNames))
            {
                return true;
            }

            if (culture == null)
            {
                culture = StrixPlatform.CurrentCultureCode;
            }

            return !this.DataSource.Query(contentType).Where("Name.ToLower().Equals(@0) AND Culture.ToLower().Equals(@1) AND !EntityId.Equals(@2)", name.ToLower(), culture.ToLower(), id).Any();
        }

        #endregion IsNameAvailable

        #region Get

        public override object Get(Type objectType, object id)
        {
            return this.Get(objectType, id, null);
        }

        public override object Get(Type objectType, object id, string includes)
        {
            if (typeof(IContent).IsAssignableFrom(objectType))
            {
                if (id != null && !(id is Guid))
                {
                    throw new InvalidOperationException("An IContent entity is requested, but the key supplied is not a Guid");
                }

                var key = (Guid?)id;
                return this.Get(objectType, key, null, null, null, includes);
            }
            else
            {
                return base.Get(objectType, id, includes);
            }
        }

        public T Get<T>(Guid id) where T : class, IContent
        {
            return this.Get(typeof(T), id, null, null, null, null) as T;
        }

        public T Get<T>(Guid id, string culture) where T : class, IContent
        {
            return this.Get(typeof(T), id, null, culture, null, null) as T;
        }

        public T Get<T>(Guid id, string culture, int versionNumber) where T : class, IContent
        {
            return this.Get(typeof(T), id, null, culture, versionNumber, null) as T;
        }

        public T Get<T>(Guid id, string culture, string relationsToInclude) where T : class, IContent
        {
            return this.Get(typeof(T), id, null, culture, null, relationsToInclude) as T;
        }

        public T Get<T>(Guid id, string culture, int versionNumber, string relationsToInclude) where T : class, IContent
        {
            return this.Get(typeof(T), id, null, culture, versionNumber, relationsToInclude) as T;
        }

        public virtual IContent Get(Type entityType, Guid id, string culture, int versionNumber, string relationsToInclude)
        {
            return this.Get(entityType, id, null, culture, versionNumber, relationsToInclude);
        }

        public T Get<T>(string url) where T : class, IContent
        {
            return this.Get(typeof(T), null, url, null, null, null) as T;
        }

        public T Get<T>(string url, string culture) where T : class, IContent
        {
            return this.Get(typeof(T), null, url, culture, null, null) as T;
        }

        public T Get<T>(string url, string culture, int versionNumber) where T : class, IContent
        {
            return this.Get(typeof(T), null, url, culture, versionNumber, null) as T;
        }

        public T Get<T>(string url, string culture, string relationsToInclude) where T : class, IContent
        {
            return this.Get(typeof(T), null, url, culture, null, relationsToInclude) as T;
        }

        public T Get<T>(string url, string culture, int versionNumber, string relationsToInclude) where T : class, IContent
        {
            return this.Get(typeof(T), null, url, culture, versionNumber, relationsToInclude) as T;
        }

        public virtual IContent Get(Type entityType, string url, string culture, int versionNumber, string relationsToInclude)
        {
            return this.Get(entityType, null, url, culture, versionNumber, relationsToInclude);
        }

        public string[] GetAvailableCultures(Type entityType, Guid entityId)
        {
            return this.Query(entityType).Where("EntityId.Equals(@0) AND IsCurrentVersion", entityId).Select("Culture").Cast<string>().ToArray();
        }

        public IList GetExistingManyToManyRelations(object entity, string propertyName)
        {
            return this.DataSource.GetExistingManyToManyRelations(entity, propertyName);
        }

        public IList<SelectRelationDto<Guid>> GetLookup<TEntity>() where TEntity : class, IContent
        {
            var cacheKey = string.Format(CmsConstants.LOOKUPPERCULTURE, typeof(TEntity).Name);
            string culture = StrixPlatform.CurrentCultureCode;
            var list = this._cache[cacheKey] as IDictionary<string, List<SelectRelationDto<Guid>>>;

            if (list == null)
            {
                list = new Dictionary<string, List<SelectRelationDto<Guid>>>();
            }

            if (!list.ContainsKey(culture))
            {
                list.Add(culture, this.QueryCurrent<TEntity>().OrderBy(e => e.Name).Select(e => new SelectRelationDto<Guid> { EntityId = e.EntityId, Name = e.Name }).ToList());
                this._cache[cacheKey] = list;
            }

            return list[culture].ToList();
        }

        public string[] GetManyToManyRelations(Type entityType)
        {
            return this.DataSource.GetManyToManyRelations(entityType);
        }

        public int GetNextSortOrder<T>() where T : class, IContent
        {
            return this.GetNextSortOrder(typeof(T));
        }

        #endregion Get

        #region Query

        public override IQueryable<T> Query<T>(string includes)
        {
            var query = base.Query<T>(includes);

            if (typeof(IContent).IsAssignableFrom(typeof(T)))
            {
                query = query.Where("!DeletedOn.HasValue");
            }

            return query;
        }

        public override IQueryable Query(Type objectType, string includes)
        {
            return this.Query(objectType, includes, null, false);
        }

        public IQueryable<T> QueryByTag<T>(string tag) where T : class, IContent
        {
            return this.QueryByTag<T>(tag, null);
        }

        public IQueryable<T> QueryByTag<T>(string tag, string includes) where T : class, IContent
        {
            return this.Query<T>(tag, includes, true);
        }

        public IQueryable<T> QueryCurrent<T>() where T : class, IContent
        {
            return this.QueryCurrent<T>(null);
        }

        public IQueryable<T> QueryCurrent<T>(string includes) where T : class, IContent
        {
            return this.Query<T>(null, includes, true);
        }

        #endregion Query

        #region Save

        public override T Save<T>(T entity)
        {
            var entityType = entity.GetType();

            if (!typeof(IContent).IsAssignableFrom(entityType))
            {
                return base.Save(entity);
            }

            return this.Save(entity as IContent, null) as T;
        }

        public virtual T Save<T>(T entity, string includes) where T : class, IContent
        {
            bool save = false;
            var entityType = entity.GetType();
            var content = entity as IContent;

            var isNew = content.EntityId == Guid.Empty || !this.DataSource.Query(entityType).Where("EntityId.Equals(@0)", content.EntityId).Any();
            var currentUserId = StrixPlatform.User.Id;
            IContent theContent = null;

            if (isNew)
            {
                var fixedUrl = content.Entity.Url;
                var sortOrder = content.SortOrder;
                var newEntity = CreateEntity(entityType, currentUserId);
                theContent = this.CreateNewContent(entityType, content, newEntity, currentUserId);
                MapContent(entityType, content, theContent);

                if (sortOrder == 0)
                {
                    theContent.SortOrder = this.GetNextSortOrder(entityType);
                }

                theContent.Entity.Url = UrlHelpers.CreateUniqueUrl(this.DataSource.Query(entityType), !string.IsNullOrWhiteSpace(fixedUrl) ? fixedUrl : theContent.Name, theContent.EntityId, "Entity.Url", "EntityId");
                save = true;
            }
            else
            {
                if (includes == null || !includes.ToLower().Contains("entity"))
                {
                    if (string.IsNullOrWhiteSpace(includes))
                    {
                        includes = "Entity";
                    }
                    else
                    {
                        includes = includes + ", Entity";
                    }
                }

                // Check whether there is content for this culture already.
                theContent = this.Get(entityType, content.EntityId, content.Culture, 0, includes);

                // Check whether the current user is allowed to edit this item.
                CheckCanEditOrDelete(theContent);

                if (theContent == null)
                {
                    theContent = this.TranslateContent(entityType, content);
                    save = true;
                }
                else
                {
                    var publishedDate = content.PublishedOn;

                    MapContent(entityType, content, theContent);

                    if (publishedDate.HasValue)
                    {
                        theContent.PublishedOn = publishedDate;
                    }

                    theContent.UpdatedByUserId = StrixPlatform.User.Id;
                    theContent.UpdatedOn = DateTime.Now;

                    if (EntityHelper.IsServiceActive(entityType, EntityServiceActions.UpdatePaths))
                    {
                        theContent.Entity.Url = UrlHelpers.CreateUniqueUrl(this.DataSource.Query(entityType), theContent.Name, theContent.EntityId, "Entity.Url", "EntityId");
                    }

                    var modifiedProperties = this.DataSource.GetModifiedPropertyValues(theContent).Where(p => !PropertiesToIgnoreForVersioning.Contains(p.PropertyName)).ToArray();

                    if (!modifiedProperties.IsEmpty())
                    {
                        theContent = this.CreateNewVersion(entityType, theContent, modifiedProperties);
                        save = true;
                    }
                    else
                    {
                        return theContent as T;
                    }
                }
            }

            if (!theContent.IsValid)
            {
                throw new StrixValidationException();
            }

            // Todo: remove when implemented publishing.
            theContent.PublishedOn = theContent.CreatedOn;
            theContent.PublishedByUserId = theContent.CreatedByUserId;

            if (save)
            {
                this.DataSource.Save(theContent);
            }

            return theContent as T;
        }

        #endregion Save

        #region Delete

        public override void Delete<T>(T entity)
        {
            var entityType = entity.GetType();

            if (!typeof(IContent).IsAssignableFrom(entityType))
            {
                base.Delete(entity);
            }
            else
            {
                var content = entity as IContent;
                this.Delete(typeof(T), content.Id, content.Culture, content.VersionNumber, null);
            }
        }

        public void Delete<T>(Guid id) where T : class, IContent
        {
            this.Delete(typeof(T), id, null, 0, null);
        }

        public void Delete<T>(Guid id, string culture) where T : class, IContent
        {
            this.Delete(typeof(T), id, culture, 0, null);
        }

        public void Delete<T>(Guid id, string culture, int versionNumber) where T : class, IContent
        {
            this.Delete(typeof(T), id, culture, versionNumber, null);
        }

        public void Delete<T>(Guid id, string culture, int versionNumber, string log) where T : class, IContent
        {
            this.Delete(typeof(T), id, culture, versionNumber, log);
        }

        public void Delete(Type entityType, Guid id, string culture, int versionNumber, string log)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException("entityType");
            }

            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id");
            }

            bool trashActive = EntityHelper.IsServiceActive(entityType, EntityServiceActions.Trashbin);
            IQueryable query = this.DataSource.Query(entityType, "Entity").Where("EntityId.Equals(@0)", id);
            var list = query.GetList();
            var nonDeleted = list.AsQueryable().Where("!DeletedOn.HasValue").Count();
            var deleteEntity = (!trashActive && string.IsNullOrWhiteSpace(culture))
                               || (nonDeleted == 0 && string.IsNullOrWhiteSpace(culture));

            if (deleteEntity)
            {
                this.DeleteEntityCompletely(query);
            }
            else
            {
                this.DeleteEntityContent(query, culture, versionNumber, trashActive);
            }

            this._cache.Delete(string.Format(CmsConstants.CONTENTPERCULTURE, entityType.Name));
            this._cache.Delete(string.Format(CmsConstants.LOOKUPPERCULTURE, entityType.Name));
        }

        #endregion Delete

        #region Protected Methods

        protected virtual int GetNextSortOrder(Type entityType)
        {
            if (!typeof(IContent).IsAssignableFrom(entityType))
            {
                throw new NotSupportedException("The entity type must implement IContent to use this method.");
            }

            string culture = StrixPlatform.CurrentCultureCode.ToLower();
            var lastSortOrder = this.Query(entityType).Where("IsCurrentVersion AND Culture.Equals(@0)", culture).Select("SortOrder").Cast<int?>().Max();
            return lastSortOrder.HasValue ? lastSortOrder.Value + 1 : 1;
        }

        protected virtual IQueryable<T> Query<T>(string tag, string includes, bool currentOnly) where T : class, IContent
        {
            var query = base.Query<T>(includes).Where(c => c.DeletedOn == null);

            if (!string.IsNullOrWhiteSpace(tag))
            {
                query = query.Where(c => c.Entity.Tags.Any(t => t.Name.ToLower() == tag.ToLower()));
            }

            if (currentOnly)
            {
                query = query.Where(c => c.Culture.ToLower() == StrixPlatform.CurrentCultureCode.ToLower() && c.IsCurrentVersion);
            }

            return query;
        }

        protected virtual IQueryable Query(Type entityType, string includes, string tag, bool currentOnly)
        {
            var query = base.Query(entityType, includes);

            if (typeof(IContent).IsAssignableFrom(entityType))
            {
                query = query.Where("!DeletedOn.HasValue");

                if (!string.IsNullOrWhiteSpace(tag))
                {
                    query = query.Where("Entity.Tags.Any(Name.ToLower().Equals(@0))", tag.ToLower());
                }

                if (currentOnly)
                {
                    query = query.Where("Culture.ToLower().Equals(@0) AND IsCurrentVersion", StrixPlatform.CurrentCultureCode.ToLower());
                }
            }

            return query;
        }

        #endregion Protected Methods

        #region Private Methods

        private static void CheckCanEditOrDelete(IContent content)
        {
            if (StrixPlatform.User.IsInRole(PlatformConstants.CONTRIBUTORROLE) && content.CreatedByUserId != StrixPlatform.User.Id)
            {
                throw new InvalidOperationException("A contributor can edit only his own content.");
            }
        }

        /// <summary>
        /// If the content has an entity id, check whether a platform entity with this id exists. If
        /// the content has no entity id or no platform entity with the id exists, create a new
        /// platform entity.
        /// </summary>
        /// <param name="entityType">The type of the entity to create</param>
        /// <param name="currentUserId">The id of the currently active user creating the entity</param>
        /// <returns>The new entity</returns>
        private static PlatformEntity CreateEntity(Type entityType, Guid currentUserId)
        {
            var entity = new PlatformEntity
            {
                Id = Guid.NewGuid(),
                EntityTypeId = EntityHelper.GetEntityTypeId(entityType),
                GroupId = StrixPlatform.User.GroupId,
                OwnerUserId = currentUserId
            };

            return entity;
        }

        /// <summary>
        /// Gets the file values for mapping content.
        /// </summary>
        /// <param name="entityType">The entity type of the content</param>
        /// <param name="first">The source content</param>
        /// <param name="second">The target content</param>
        /// <returns>A dictionary containing the file property names and values</returns>
        private static IDictionary<string, object> GetFileValues(Type entityType, object first, object second)
        {
            Dictionary<string, object> fileValues = new Dictionary<string, object>();
            var fileProperties = entityType.GetProperties().Where(p => p.GetAttribute<FileUploadAttribute>() != null).Select(p => p.Name).ToArray();

            foreach (var prop in fileProperties)
            {
                var firstVal = first.GetPropertyValue(prop);
                var secondVal = second.GetPropertyValue(prop);

                if (firstVal == null && secondVal != null)
                {
                    fileValues.Add(prop, secondVal);
                }
                else
                {
                    fileValues.Add(prop, null);
                }
            }

            return fileValues;
        }

        /// <summary>
        /// Gets the relation values for mapping content.
        /// </summary>
        /// <param name="entityType">The entity type of the content</param>
        /// <param name="first">The source content</param>
        /// <param name="second">The target content</param>
        /// <returns>A dictionary containing the relation property names and values</returns>
        private static IDictionary<string, object> GetRelationValues(Type entityType, object first, object second)
        {
            Dictionary<string, object> relationValues = new Dictionary<string, object>();

            var relationProperties = entityType.GetProperties().Where(p =>
            {
                if (typeof(IEnumerable).IsAssignableFrom(p.PropertyType) && !p.PropertyType.Equals(typeof(string)))
                {
                    var genericArguments = p.PropertyType.GenericTypeArguments;

                    if (genericArguments.Length == 1 && typeof(ValidationBase).IsAssignableFrom(genericArguments.First()))
                    {
                        return true;
                    }
                }

                return false;
            }).Select(p => p.Name).ToArray();

            foreach (var prop in relationProperties)
            {
                var firstVal = first.GetPropertyValue(prop) as IEnumerable<ValidationBase>;
                var secondVal = second.GetPropertyValue(prop);

                if (firstVal != null && firstVal.Any(v => !v.IsValid) && secondVal != null)
                {
                    relationValues.Add(prop, ((IEnumerable<ValidationBase>)secondVal).ToList());
                }
                else
                {
                    relationValues.Add(prop, null);
                }
            }

            return relationValues;
        }

        /// <summary>
        /// Maps one content to another, preserving all important properties that should not be overwritten.
        /// </summary>
        /// <param name="entityType">The type of the content to map</param>
        /// <param name="first">The source content</param>
        /// <param name="second">The target content</param>
        private static void MapContent(Type entityType, IContent first, IContent second)
        {
            Guid id = second.Id;
            Guid entityId = second.EntityId;
            PlatformEntity entity = second.Entity;
            string culture = second.Culture;
            bool isCurrent = second.IsCurrentVersion;
            int versionNumber = second.VersionNumber;
            Guid createdByUserId = second.CreatedByUserId;
            DateTime createdOn = second.CreatedOn;
            Guid updatedByUserId = second.UpdatedByUserId;
            DateTime updatedOn = second.UpdatedOn;
            Guid? publishedByUserId = second.PublishedByUserId;
            DateTime? publishedOn = second.PublishedOn;
            int numberOfComments = second.NumberOfComments;
            DateTime? lastCommentDate = second.LastCommentDate;
            IDictionary<string, object> fileValues = GetFileValues(entityType, first, second);
            IDictionary<string, object> relationValues = GetRelationValues(entityType, first, second);

            first.Map(entityType, second);

            second.Id = id;
            second.EntityId = entityId;
            second.Entity = entity;
            second.Culture = culture;
            second.IsCurrentVersion = isCurrent;
            second.VersionNumber = versionNumber;
            second.CreatedByUserId = createdByUserId;
            second.CreatedOn = createdOn;
            second.UpdatedByUserId = updatedByUserId;
            second.UpdatedOn = updatedOn;
            second.PublishedByUserId = publishedByUserId;
            second.PublishedOn = publishedOn;
            second.NumberOfComments = numberOfComments;
            second.LastCommentDate = lastCommentDate;

            if (fileValues.Count > 0)
            {
                int index = 0;

                foreach (var prop in fileValues.Keys)
                {
                    if (fileValues[prop] != null)
                    {
                        second.SetPropertyValue(prop, fileValues[prop]);
                    }

                    index++;
                }
            }

            if (relationValues.Count > 0)
            {
                int index = 0;

                foreach (var prop in relationValues.Keys)
                {
                    if (relationValues[prop] != null)
                    {
                        var collection = second.GetPropertyValue(prop);
                        var values = relationValues[prop];
                        var clearMethod = collection.GetType().GetMethod("Clear");
                        var addMethod = collection.GetType().GetMethod("Add");
                        clearMethod.Invoke(collection, null);

                        foreach (var item in (IEnumerable<ValidationBase>)values)
                        {
                            addMethod.Invoke(collection, new object[] { item });
                        }
                    }

                    index++;
                }
            }
        }

        /// <summary>
        /// Create new content for an entity.
        /// </summary>
        /// <param name="entityType">The entity type for which to create the content</param>
        /// <param name="content">The content to use as source</param>
        /// <param name="entity">The entity to create the content for</param>
        /// <param name="currentUserId">The id of the currently active user creating the content</param>
        /// <returns>The new content</returns>
        private IContent CreateNewContent(Type entityType, IContent content, PlatformEntity entity, Guid currentUserId)
        {
            var theContent = this.Get(entityType, null) as IContent;

            if (!string.IsNullOrWhiteSpace(content.Culture))
            {
                theContent.Culture = content.Culture;
            }

            theContent.Id = Guid.NewGuid();
            theContent.Name = content.Name;

            if (string.IsNullOrWhiteSpace(content.Entity.Url) && !string.IsNullOrWhiteSpace(content.Name))
            {
                theContent.Entity.Url = theContent.Name;
            }

            theContent.EntityId = entity.Id;
            theContent.Entity = entity;
            theContent.CreatedByUserId = currentUserId;
            theContent.CreatedOn = DateTime.Now;
            theContent.UpdatedByUserId = currentUserId;
            theContent.UpdatedOn = theContent.CreatedOn;
            return theContent;
        }

        /// <summary>
        /// If versioning is active, the content is not a draft and there are modified properties,
        /// create a new version for the content.
        /// </summary>
        /// <param name="contentType">The entity type for which to create the version</param>
        /// <param name="content">The content to use as source</param>
        /// <param name="modifiedPropertyValues">The modified property values</param>
        /// <returns>The content version</returns>
        private IContent CreateNewVersion(Type contentType, IContent content, IList<ModifiedPropertyValue> modifiedPropertyValues)
        {
            var isDraft = content.IsCurrentVersion && content.PublishedOn == null;

            if (EntityHelper.IsServiceActive(contentType, EntityServiceActions.AutomaticVersions) && !isDraft)
            {
                if (!modifiedPropertyValues.IsEmpty())
                {
                    // Get the new version number to use.
                    var versionQuery = this.DataSource.Query(contentType).Where("EntityId.Equals(@0) AND Culture.ToLower().Equals(@1) AND PublishedOn.HasValue", content.EntityId, content.Culture);
                    var versionNumber = versionQuery.Select("VersionNumber").Cast<int>().Max();

                    // Create a new version with a new id.
                    var theContent = content.Map(contentType) as IContent;
                    theContent.Id = Guid.NewGuid();
                    theContent.VersionNumber = versionNumber + 1;
                    theContent.IsCurrentVersion = true;

                    // Undo the changes made to the old version by ignoring the currently loaded
                    // version and fetching a fresh one from the database. Then, set the IsCurrent
                    // flag on the old version to false.
                    this.DataSource.Ignore(content);
                    content = this.Get(contentType, content.EntityId, content.Culture, content.VersionNumber, null);
                    content.IsCurrentVersion = false;
                    return theContent;
                }
            }

            return content;
        }

        /// <summary>
        /// Completely deletes an entity from the data source.
        /// </summary>
        /// <param name="query">The query for the entity content</param>
        private void DeleteEntityCompletely(IQueryable query)
        {
            var entry = query.GetFirst() as IContent;

            if (entry == null)
            {
                return;
            }

            // Check whether the current user is allowed to edit this item.
            CheckCanEditOrDelete(entry);

            var platformEntry = entry.GetPropertyValue("Entity") as PlatformEntity;
            var comments = this.DataSource.Query<Comment>().Where(c => c.EntityId == platformEntry.Id);
            this.DataSource.Delete(query);
            this.DataSource.Delete(comments);
            this.DataSource.Delete(platformEntry);
        }

        /// <summary>
        /// Deletes the content with the specified version number for the specified culture for an
        /// entity from the data source.
        /// </summary>
        /// <param name="query">The query for the entity content</param>
        /// <param name="culture">The culture to delete the content for</param>
        /// <param name="versionNumber">The version number of the content to delete</param>
        /// <param name="trashActive">True if the trash bin is active, false otherwise</param>
        private void DeleteEntityContent(IQueryable query, string culture, int versionNumber, bool trashActive)
        {
            if (!string.IsNullOrWhiteSpace(culture))
            {
                query = query.Where("Culture.Equals(@0)", culture);
            }

            if (versionNumber > 0)
            {
                query = query.Where("VersionNumber.Equals(@0)", versionNumber);
                var entry = query.GetFirst() as IContent;

                // Check whether the current user is allowed to edit this item.
                CheckCanEditOrDelete(entry);

                if (entry.DeletedOn.HasValue || !trashActive)
                {
                    this.DataSource.Delete(entry);
                }
                else
                {
                    entry.DeletedOn = DateTime.Now;
                    entry.DeletedByUserId = StrixPlatform.User.Id;
                }
            }
            else
            {
                foreach (var entry in query)
                {
                    var content = entry as IContent;

                    // Check whether the current user is allowed to edit this item.
                    CheckCanEditOrDelete(content);

                    if (content.DeletedOn.HasValue || !trashActive)
                    {
                        this.DataSource.Delete(entry);
                    }
                    else
                    {
                        content.DeletedOn = DateTime.Now;
                        content.DeletedByUserId = StrixPlatform.User.Id;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the content with the specified version number for the specified culture for an
        /// entity by its url or platform id, including the specified relations.
        /// </summary>
        /// <param name="entityType">The type of the entity to get the content for</param>
        /// <param name="id">The entity platform id</param>
        /// <param name="url">The entity url</param>
        /// <param name="culture">The culture to get the content for</param>
        /// <param name="versionNumber">The version number to get the content for</param>
        /// <param name="includes">The relations to include, separated by commas</param>
        /// <returns>The entity</returns>
        private IContent Get(Type entityType, Guid? id, string url, string culture = null, int? versionNumber = null, string includes = null)
        {
            IContent content;
            var query = base.Query(entityType, includes);

            if (id.HasValue)
            {
                query = query.Where("EntityId.Equals(@0)", id.Value);
            }
            else if (!string.IsNullOrWhiteSpace(url))
            {
                query = query.Where("Entity.Url.ToLower().Equals(@0)", url.ToLower());
            }
            else
            {
                content = Activator.CreateInstance(entityType) as IContent;
                content.Culture = StrixPlatform.CurrentCultureCode;
                content.VersionNumber = 1;
                content.IsCurrentVersion = true;
                content.Entity = new PlatformEntity();
                return content;
            }

            if (versionNumber.HasValue && versionNumber.Value > 0)
            {
                query = query.Where("VersionNumber == @0", versionNumber.Value);
            }
            else
            {
                query = query.Where("IsCurrentVersion");
            }

            culture = string.IsNullOrWhiteSpace(culture) ? StrixPlatform.CurrentCultureCode : culture;
            query = query.Where("Culture.ToLower().Equals(@0)", culture.ToLower());

            content = query.GetFirst() as IContent;

            if (content == null)
            {
                Logger.Log(string.Format("No entity found with {0} {1}", id.HasValue ? "id" : "url", id.HasValue ? id.Value.ToString() : url), LogLevel.Error);
                return null;
            }

            return content;
        }

        /// <summary>
        /// Creates a content translation for an entity.
        /// </summary>
        /// <param name="entityType">The entity type for which to create the translation</param>
        /// <param name="content">The content to use as source</param>
        /// <returns>The content translation</returns>
        private IContent TranslateContent(Type entityType, IContent content)
        {
            if (EntityHelper.IsServiceActive(entityType, EntityServiceActions.Translations))
            {
                // try to get the content for the default culture or any culture, copy it, and
                // update the culture.
                var query = this.DataSource.Query(entityType, "Entity").Where("EntityId.Equals(@0)", content.EntityId);
                var list = query.GetList();

                var theContent = list.AsQueryable().Where("Culture.ToLower().Equals(@0)", StrixPlatform.CurrentCultureCode.ToLower()).GetFirst() as IContent;

                if (theContent == null)
                {
                    theContent = list.AsQueryable().GetFirst() as IContent;
                }

                if (theContent != null)
                {
                    // Copy the content, then map the new values to the copy. Give the new version a
                    // new id and version number.
                    theContent = theContent.Map(entityType) as IContent;
                    var culture = content.Culture;
                    MapContent(entityType, content, theContent);
                    theContent.Id = Guid.NewGuid();
                    theContent.Culture = culture;
                    theContent.VersionNumber = 1;
                    theContent.IsCurrentVersion = true;
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Tried to save a content translation, but no content is available"));
                }

                return theContent as IContent;
            }
            else
            {
                throw new InvalidOperationException(string.Format("Tried to save content translated content for culture {0}, but translations are not enabled", content.Culture));
            }
        }

        #endregion Private Methods
    }
}
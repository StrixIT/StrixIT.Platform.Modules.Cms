#region Apache License
//-----------------------------------------------------------------------
// <copyright file="EntityService.cs" company="StrixIT">
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class EntityService<TModel> : ObjectService<Guid, TModel>, IEntityService<TModel> where TModel : EntityViewModel, new()
    {
        private ITaxonomyManager _taxonomyManager;
        private ICacheService _cache;

        public EntityService(IPlatformDataSource dataSource, IEntityManager entityManager, ITaxonomyManager taxonomyManager, ICacheService cache)
            : base(dataSource, entityManager)
        {
            this._taxonomyManager = taxonomyManager;
            this._cache = cache;
        }

        protected new IEntityManager Manager
        {
            get
            {
                return base.Manager as IEntityManager;
            }
        }

        protected ICacheService Cache
        {
            get
            {
                return this._cache;
            }
        }

        public override bool Exists(string name, Guid? id)
        {
            var map = EntityHelper.GetObjectMap(typeof(TModel));

            if (EntityHelper.IsServiceActive(map.ContentType, EntityServiceActions.AllowNonUniqueNames))
            {
                return false;
            }

            return base.Exists(name, id, "EntityId");
        }

        #region Get

        public Guid? GetId(string idOrUrl)
        {
            Guid id;

            if (Guid.TryParse(idOrUrl, out id))
            {
                return id;
            }

            var map = EntityHelper.GetObjectMap(typeof(TModel));
            return (Guid?)this.Manager.Query(map.ContentType).Where("Culture.ToLower().Equals(@0) AND Entity.Url.ToLower().Equals(@1)", StrixPlatform.CurrentCultureCode, idOrUrl).Select("EntityId").GetFirst();
        }

        public string GetUrl(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id guid is empty");
            }

            var map = EntityHelper.GetObjectMap(typeof(TModel));
            return (string)this.Manager.Query(map.ContentType).Where("EntityId.Equals(@0)", id).Select("Entity.Url").GetFirst();
        }

        public override TModel Get(Guid? id)
        {
            if (typeof(EntityViewModel).IsAssignableFrom(typeof(TModel)))
            {
                return this.Get(typeof(TModel), id, null, null, 0, null) as TModel;
            }

            return base.Get(id);
        }

        public TModel Get(Guid? id, string culture)
        {
            return this.Get(typeof(TModel), id, null, culture, 0, null) as TModel;
        }

        public TModel Get(Guid id, string culture, string relationsToInclude)
        {
            return this.Get(typeof(TModel), id, null, culture, 0, relationsToInclude) as TModel;
        }

        public TModel Get(Guid id, string culture, int versionNumber)
        {
            return this.Get(typeof(TModel), id, null, culture, versionNumber, null) as TModel;
        }

        public TModel Get(Guid id, string culture, int versionNumber, string relationsToInclude)
        {
            return this.Get(typeof(TModel), id, null, culture, versionNumber, relationsToInclude) as TModel;
        }

        public TModel Get(string url)
        {
            return this.Get(url, null, 0, null);
        }

        public TModel Get(string url, string culture)
        {
            return this.Get(url, culture, 0, null);
        }

        public TModel Get(string url, string culture, string relationsToInclude)
        {
            return this.Get(url, culture, 0, relationsToInclude);
        }

        public TModel Get(string url, string culture, int versionNumber)
        {
            return this.Get(url, culture, versionNumber, null);
        }

        public TModel Get(string url, string culture, int versionNumber, string relationsToInclude)
        {
            return this.Get(typeof(TModel), null, url, culture, versionNumber, relationsToInclude) as TModel;
        }

        public TModel GetAny(string url, string culture, string relationsToInclude)
        {
            return this.Get(typeof(TModel), null, url, culture, 0, relationsToInclude, true) as TModel;
        }

        #endregion

        #region Cache

        public virtual TModel GetCached(string url, string permissionKey = null)
        {
            TModel entity = null;
            var currentCulture = StrixPlatform.CurrentCultureCode;
            var cacheKey = string.Format(CmsConstants.CONTENTPERCULTURE, typeof(TModel).Name) + permissionKey;

            // First, try to get the content from the cache. If it is not there, retrieve it from the database and add it to the cache.
            var tupleList = this._cache[cacheKey] as List<Tuple<string, string, dynamic>>;

            if (tupleList != null)
            {
                var tuple = tupleList.Where(tu => tu.Item1.ToLower() == url.ToLower() && tu.Item2 == currentCulture).FirstOrDefault();
                entity = tuple != null ? tuple.Item3 : null;
            }

            if (entity == null)
            {
                entity = this.Get(url);
                StrixPlatform.RaiseEvent<CacheEntityModelEvent<TModel>>(new CacheEntityModelEvent<TModel>(entity));

                if (tupleList == null)
                {
                    tupleList = new List<Tuple<string, string, dynamic>>();
                }

                tupleList.Add(new Tuple<string, string, dynamic>(url, currentCulture, entity));
                this._cache[cacheKey] = tupleList;
            }

            return entity;
        }

        #endregion

        #region List

        public override IEnumerable List(FilterOptions filter)
        {
            if (!typeof(EntityViewModel).IsAssignableFrom(typeof(TModel)))
            {
                return base.List(filter);
            }

            var map = EntityHelper.GetObjectMap(typeof(TModel));
            var culture = StrixPlatform.CurrentCultureCode.ToLower();
            IQueryable query = this.Manager.Query(map.ContentType).Where("Culture.ToLower().Equals(@0) AND IsCurrentVersion", culture);

            var queryEvent = new PrepareQueryEvent(query, filter);
            StrixPlatform.RaiseEvent(queryEvent);
            query = queryEvent.Query;

            return query.Map(map.ListModelType);
        }

        #endregion

        #region Tags

        public IList<TermViewModel> GetTags(Guid entityId)
        {
            return this._taxonomyManager.TagQuery().Where(t => t.TaggedEntities.Any(e => e.Id == entityId)).Map<TermViewModel>().ToList();
        }

        public IList<TermViewModel> GetAllTags()
        {
            return this._taxonomyManager.TagQuery().Map<TermViewModel>().ToList();
        }

        #endregion

        #region Save

        public override SaveResult<TModel> Save(TModel model, bool saveChanges)
        {
            if (!typeof(EntityViewModel).IsAssignableFrom(model.GetType()))
            {
                return base.Save(model, saveChanges);
            }

            if (this.Exists(model.Name, model.EntityId))
            {
                return new SaveResult<TModel> { Success = false, Message = StrixIT.Platform.Modules.Cms.Resources.Interface.NameNotUnique };
            }

            var result = new SaveResult<TModel>();
            var entityModel = model as EntityViewModel;

            RemoveUnselectedRelations(model);

            var map = EntityHelper.GetObjectMap(typeof(TModel));
            var content = entityModel.Map(map.ContentType) as IContent;
            var rteProperties = map.ContentType.GetProperties().Where(p => p.GetAttribute<RteAttribute>() != null).Select(pr => pr.Name).ToArray();
            var fileProperties = EntityHelper.GetFileIdProperties(map.ContentType);
            var isNew = entityModel.Id == Guid.Empty;

            // Todo: can combine in one call?
            var pattern = @"{0}\s*=\s*([""\'])(.*?)\1";
            CleanRteFilePaths(content, rteProperties, "src", pattern);
            CleanRteFilePaths(content, rteProperties, "href", pattern);

            if (!isNew)
            {
                // remove all old file tags for this entity.
                var oldContent = this.Manager.Get(map.ContentType, entityModel.EntityId, null, 0, "Entity.Tags") as IContent;

                if (oldContent != null)
                {
                    // Refresh all tags for this entity.
                    if (oldContent.Entity.Tags != null)
                    {
                        oldContent.Entity.Tags.Clear();
                    }

                    // Remove old tags
                    var oldFiles = this.GetFileNames(oldContent, rteProperties, fileProperties);
                    this._taxonomyManager.RemoveTagFromFiles(EntityHelper.GetEntityType(oldContent.Entity.EntityTypeId).FullName + "_" + oldContent.Id.ToString(), oldFiles);
                }
            }
            else
            {
                if (entityModel.SortOrder > 0)
                {
                    content.SortOrder = entityModel.SortOrder;
                }
            }

            this.GetNewRelations(content);

            content = this.Manager.Save(content);

            if (content != null)
            {
                // Add all current file tags for this entity.
                var newFiles = this.GetFileNames(content, rteProperties, fileProperties);
                var entityType = EntityHelper.GetEntityType(content.Entity.EntityTypeId);

                this._taxonomyManager.AddTagToFiles(entityType.FullName + "_" + content.Id.ToString(), newFiles);

                if (entityModel.Tags != null && EntityHelper.IsServiceActive(map.ContentType, EntityServiceActions.AllowFixedTagging))
                {
                    var tagIds = entityModel.Tags.Where(t => t.Selected).Select(t => t.Id).ToArray();

                    if (tagIds.Length > 0)
                    {
                        this._taxonomyManager.Tag(content, tagIds);
                    }
                }

                var cacheKey = string.Format(CmsConstants.CONTENTPERCULTURE, typeof(TModel).Name);
                this._cache.Delete(cacheKey);

                if (saveChanges)
                {
                    this.SaveChanges();

                    if (isNew)
                    {
                        var fileIncludes = EntityHelper.GetFileProperties(map.ContentType);

                        if (!fileIncludes.IsEmpty())
                        {
                            result.Entity = this.Manager.Get(map.ContentType, content.EntityId, null, 0, null);
                        }
                    }
                }

                result.Success = true;
                result.Entity = content;
            }

            return result;
        }

        #endregion

        #region Delete

        public override void Delete(Guid id, bool saveChanges)
        {
            if (typeof(EntityViewModel).IsAssignableFrom(typeof(TModel)))
            {
                this.Delete(typeof(TModel), id, null, 0, null, saveChanges);
            }
            else
            {
                base.Delete(id, saveChanges);
            }
        }

        public void Delete(Guid id, string culture)
        {
            this.Delete(id, culture, 0, null, true);
        }

        public void Delete(Guid id, string culture, int versionNumber)
        {
            this.Delete(id, culture, versionNumber, null, true);
        }

        public void Delete(Guid id, string culture, int versionNumber, string log)
        {
            this.Delete(id, culture, versionNumber, log, true);
        }

        public virtual void Delete(Guid id, string culture, int versionNumber, string log, bool saveChanges)
        {
            this.Delete(typeof(TModel), id, culture, versionNumber, log, saveChanges);
        }

        #endregion

        #region Versions

        public IList<VersionViewModel> GetVersionList(Guid id, string culture, FilterOptions filter)
        {
            var result = new List<VersionViewModel>();
            var map = EntityHelper.GetObjectMap(typeof(TModel));
            culture = string.IsNullOrWhiteSpace(culture) ? StrixPlatform.CurrentCultureCode : culture;

            if (filter == null || filter.Sort.IsEmpty())
            {
                if (filter == null)
                {
                    filter = new FilterOptions();
                }

                filter.Sort.Add(new SortField { Field = "VersionNumber", Dir = "desc" });
            }

            IQueryable query = this.Manager.Query(map.ContentType).Where("EntityId.Equals(@0) AND Culture.ToLower().Equals(@1)", id, culture).Filter(filter);
            var projection = query.Select("new ( CreatedByUserId, CreatedOn, VersionNumber, VersionLog, IsCurrentVersion )");

            foreach (var item in projection)
            {
                result.Add(new VersionViewModel
                {
                    EntityId = id,
                    VersionNumber = (int)item.GetPropertyValue("VersionNumber"),
                    IsCurrentVersion = (bool)item.GetPropertyValue("IsCurrentVersion"),
                    CreatedBy = StrixCms.GetUserName((Guid)item.GetPropertyValue("CreatedByUserId")),
                    CreatedOn = (DateTime)item.GetPropertyValue("CreatedOn"),
                    Log = (string)item.GetPropertyValue("VersionLog"),
                });
            }

            return result;
        }

        public TModel RestoreVersion(Guid id, int versionNumber, string log)
        {
            var culture = StrixPlatform.CurrentCultureCode;
            var currentVersion = this.Get(id);

            if (versionNumber == currentVersion.VersionNumber)
            {
                throw new NotSupportedException("Cannot restore the current entity version.");
            }

            var currentVersionNumber = currentVersion.VersionNumber;
            var versionToRestore = this.Get(id, culture, versionNumber);
            versionToRestore.Map<TModel>(currentVersion);
            currentVersion.VersionNumber = currentVersionNumber;

            var map = EntityHelper.GetObjectMap(typeof(TModel));
            var drafts = this.Manager.Query(map.ContentType).Where("EntityId.Equals(@0) AND Culture.Equals(@1) AND VersionNumber > @2", id, culture, currentVersionNumber);

            foreach (var entry in drafts)
            {
                var item = entry as IContent;
                item.VersionNumber++;
            }

            versionToRestore.VersionLog = log;
            var result = this.Save(versionToRestore, false);
            this.SaveChanges();
            return result.Model;
        }

        #endregion

        #region Protected Methods

        protected virtual void Delete(Type viewModelType, Guid id, string culture, int versionNumber, string log, bool saveChanges)
        {
            if (viewModelType == null)
            {
                throw new ArgumentNullException("viewModelType");
            }

            var map = EntityHelper.GetObjectMap(viewModelType);
            this.Manager.Delete(map.ContentType, id, culture, versionNumber, log);

            if (saveChanges)
            {
                this.SaveChanges();
            }

            var cacheKey = string.Format(CmsConstants.CONTENTPERCULTURE, viewModelType.Name);
            this._cache.Delete(cacheKey);
        }

        protected virtual EntityViewModel Get(Type modelType, Guid? id, string url, string culture, int versionNumber, string relationsToInclude, bool useFallBack = false)
        {
            IContent content = null;
            EntityViewModel model = null;
            var map = EntityHelper.GetObjectMap(modelType);
            var useTagging = EntityHelper.IsServiceActive(map.ContentType, EntityServiceActions.AllowFixedTagging);

            if (id != null || !string.IsNullOrWhiteSpace(url))
            {
                if (string.IsNullOrWhiteSpace(culture))
                {
                    culture = StrixPlatform.CurrentCultureCode;
                }

                relationsToInclude = GetRelationsToInclude(relationsToInclude, useTagging);

                if (id.HasValue)
                {
                    if (versionNumber > 0)
                    {
                        content = this.Manager.Get(map.ContentType, id.Value, culture, versionNumber, relationsToInclude);
                    }
                    else
                    {
                        content = this.Manager.Get(map.ContentType, id.Value, culture, 0, relationsToInclude);
                    }
                }
                else
                {
                    if (versionNumber > 0)
                    {
                        content = this.Manager.Get(map.ContentType, url, culture, versionNumber, relationsToInclude);
                    }
                    else
                    {
                        if (useFallBack && EntityHelper.IsServiceActive(map.ContentType, EntityServiceActions.Translations))
                        {
                            var query = this.Manager.Query(map.ContentType, relationsToInclude).Where("Entity.Url.ToLower().Equals(@0) AND IsCurrentVersion", url.ToLower());
                            content = query.Where("Culture.ToLower().Equals(@0)", StrixPlatform.CurrentCultureCode.ToLower()).GetFirst() as IContent;

                            // If no content was found for this url and the current culture, try and get a version for another culture and
                            // set the culture to the current one.
                            if (content == null)
                            {
                                content = query.GetFirst() as IContent;

                                if (content != null)
                                {
                                    content.Culture = StrixPlatform.CurrentCultureCode;
                                }
                            }
                        }
                        else
                        {
                            content = this.Manager.Get(map.ContentType, url, culture, 0, relationsToInclude);
                        }
                    }
                }

                model = content.Map(modelType) as EntityViewModel;
            }

            if (model == null)
            {
                model = this.Manager.Get(map.ContentType, null).Map<TModel>();

                if (id.HasValue)
                {
                    model.EntityId = id.Value;
                }

                model.Url = url;
            }

            this.AddTagsAndTranslations(content, model, map, useTagging);

            return model;
        }

        #endregion

        #region Private Methods

        private static string GetRelationsToInclude(string relationsToInclude, bool useTagging)
        {
            if (relationsToInclude == null || !relationsToInclude.ToLower().Contains("entity"))
            {
                if (string.IsNullOrWhiteSpace(relationsToInclude))
                {
                    relationsToInclude = "Entity";
                }
                else
                {
                    relationsToInclude = relationsToInclude + ", Entity";
                }
            }

            if (useTagging && !relationsToInclude.ToLower().Contains("entity.tags"))
            {
                relationsToInclude = relationsToInclude + ", Entity.Tags";
            }

            return relationsToInclude;
        }

        private static void RemoveUnselectedRelations(EntityViewModel model)
        {
            // Remove non-selected relations before mapping.
            foreach (var relationProperty in typeof(TModel).GetProperties().Where(p =>
            {
                var genericArgument = !p.PropertyType.GenericTypeArguments.IsEmpty() && typeof(IEnumerable).IsAssignableFrom(p.PropertyType) ? p.PropertyType.GenericTypeArguments.First() : null;

                if (genericArgument == null || genericArgument.GenericTypeArguments.IsEmpty())
                {
                    return false;
                }

                var relationType = typeof(SelectRelationDto<>).MakeGenericType(genericArgument.GenericTypeArguments.First());
                return genericArgument.Equals(relationType);
            }))
            {
                var relation = model.GetPropertyValue(relationProperty.Name) as IEnumerable;

                if (relation != null)
                {
                    model.SetPropertyValue(relationProperty.Name, relation.AsQueryable().Where("Selected").GetList());
                }
            }
        }

        private static void CleanRteFilePaths(IContent content, string[] properties, string attr, string pattern)
        {
            foreach (var property in properties)
            {
                var value = (string)content.GetPropertyValue(property);

                if (value != null)
                {
                    // Replace relative paths by a path to the root to prevent difficulties.
                    MatchCollection sourceMatches = Regex.Matches(value, string.Format(pattern, attr));

                    // Remove ../../ etc and replace by /.
                    foreach (Match match in sourceMatches)
                    {
                        string newValue = Regex.Replace(match.Value, @"(\.\./)+", "/");

                        Uri uriResult;
                        bool isUrl = Uri.TryCreate(newValue.Replace(attr + "=", string.Empty).Replace("\"", string.Empty), UriKind.Absolute, out uriResult);

                        // If the path does not start with a '/', add it. This seems to be a bug introduced by the current version of TinyMCE.
                        if (!isUrl && newValue.ElementAt(newValue.IndexOf("\"") + 1) != '/')
                        {
                            newValue = newValue.Substring(0, newValue.IndexOf("\"")) + "\"/" + newValue.Substring(newValue.IndexOf("\"") + 1);
                        }

                        content.SetPropertyValue(property, value.Replace(match.Value, newValue));
                        value = (string)content.GetPropertyValue(property);
                    }
                }
            }
        }

        private void AddTagsAndTranslations(IContent content, EntityViewModel model, ObjectMap map, bool useTagging)
        {
            if (content != null)
            {
                if (EntityHelper.IsServiceActive(map.ContentType, EntityServiceActions.Translations))
                {
                    model.AvailableCultures = this.Manager.GetAvailableCultures(map.ContentType, model.EntityId);
                }
            }

            if (useTagging)
            {
                var allTags = this._taxonomyManager.TagQuery().Map<TermViewModel>().ToList();

                if (content != null)
                {
                    foreach (var existingTag in content.Entity.Tags)
                    {
                        var tag = allTags.FirstOrDefault(t => t.Id == existingTag.Id);

                        if (tag != null)
                        {
                            tag.Selected = true;
                        }
                    }
                }

                model.Tags = allTags.OrderByDescending(t => t.Selected).ThenBy(t => t.Name).ToList();
            }
        }

        private string[] GetFileNames(IContent content, string[] rteProperties, string[] imageProperties)
        {
            List<string> fileNames = new List<string>();

            foreach (var property in rteProperties)
            {
                var value = (string)content.GetPropertyValue(property);

                if (value != null)
                {
                    // Find the file Guids and use them to link the files to the content.
                    MatchCollection matches = Regex.Matches(value, "[a-zA-Z0-9]{8}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{12}");

                    foreach (Match match in matches)
                    {
                        fileNames.Add(match.Value.ToLower());
                    }
                }
            }

            var imageIds = new List<Guid>(imageProperties.Count());

            foreach (var property in imageProperties)
            {
                var id = (Guid?)content.GetPropertyValue(property);

                if (id.HasValue)
                {
                    imageIds.Add(id.Value);
                }
            }

            if (!imageIds.IsEmpty())
            {
                var imageNames = this.Manager.Query<File>().Where(f => imageIds.Contains(f.Id)).Select(f => f.FileName).ToArray();
                fileNames = fileNames.Union(imageNames).Distinct().ToList();
            }

            return fileNames.Distinct().ToArray();
        }

        private void GetNewRelations(object entity)
        {
            // Get many-to-many relations
            var manyToMany = this.Manager.GetManyToManyRelations(entity.GetType());

            foreach (var relation in manyToMany)
            {
                var relationProperty = entity.GetPropertyValue(relation) as IEnumerable;

                if (relationProperty.IsEmpty())
                {
                    continue;
                }

                // Get the current relations.
                var currentRelations = this.Manager.GetExistingManyToManyRelations(entity, relation);

                // Remove old many-to-many relations
                var clearMethod = relationProperty.GetType().GetMethod("Clear");
                clearMethod.Invoke(relationProperty, null);

                // Add new many-to-many relations
                var addMethod = relationProperty.GetType().GetMethod("Add");

                foreach (var relatedEntity in currentRelations)
                {
                    addMethod.Invoke(relationProperty, new object[] { relatedEntity });
                }
            }
        }

        #endregion
    }
}
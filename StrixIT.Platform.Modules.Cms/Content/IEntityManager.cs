#region Apache License

//-----------------------------------------------------------------------
// <copyright file="IEntityManager.cs" company="StrixIT">
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface for the entity manager.
    /// </summary>
    public interface IEntityManager : IObjectManager
    {
        #region IsNameAvailable

        /// <summary>
        /// Checks whether the specified name is available for the content of the specified type
        /// with the specified id.
        /// </summary>
        /// <param name="contentType">The content type to check the name for</param>
        /// <param name="name">The name to check</param>
        /// <param name="id">The id of the content</param>
        /// <returns>True if the name is available, false otherwise</returns>
        bool IsNameAvailable(Type contentType, string name, Guid id);

        /// <summary>
        /// Checks whether the specified name is available for the content of the specified type
        /// with the specified id.
        /// </summary>
        /// <param name="contentType">The content type to check the name for</param>
        /// <param name="name">The name to check</param>
        /// <param name="id">The id of the content</param>
        /// <param name="culture">The culture to use when checking the name</param>
        /// <returns>True if the name is available, false otherwise</returns>
        bool IsNameAvailable(Type contentType, string name, Guid id, string culture);

        #endregion IsNameAvailable

        #region Get

        /// <summary>
        /// Gets an entity by its id, with the current content for the current culture.
        /// </summary>
        /// <typeparam name="T">The type of the entity to get</typeparam>
        /// <param name="id">The id of the entity to get</param>
        /// <returns>The entity</returns>
        T Get<T>(Guid id) where T : class, IContent;

        /// <summary>
        /// Gets an entity by its id, with the current content for the specified culture.
        /// </summary>
        /// <typeparam name="T">The type of the entity to get</typeparam>
        /// <param name="id">The id of the entity to get</param>
        /// <param name="culture">The culture to get the entity for</param>
        /// <returns>The entity</returns>
        T Get<T>(Guid id, string culture) where T : class, IContent;

        /// <summary>
        /// Gets an entity by its id, with the specified version of the content for the specified culture.
        /// </summary>
        /// <typeparam name="T">The type of the entity to get</typeparam>
        /// <param name="id">The id of the entity to get</param>
        /// <param name="culture">The culture to get the content for</param>
        /// <param name="versionNumber">The version of the content to get</param>
        /// <returns>The entity</returns>
        T Get<T>(Guid id, string culture, int versionNumber) where T : class, IContent;

        /// <summary>
        /// Gets an entity by its id, with the current content for the specified culture and
        /// including the specified relations.
        /// </summary>
        /// <typeparam name="T">The type of the entity to get</typeparam>
        /// <param name="id">The id of the entity to get</param>
        /// <param name="culture">The culture to get the entity for</param>
        /// <param name="relationsToInclude">The relations to include</param>
        /// <returns>The entity</returns>
        T Get<T>(Guid id, string culture, string relationsToInclude) where T : class, IContent;

        /// <summary>
        /// Gets an entity by its id, with the the specified version of the content for the
        /// specified culture and including the specified relations.
        /// </summary>
        /// <typeparam name="T">The type of the entity to get</typeparam>
        /// <param name="id">The id of the entity to get</param>
        /// <param name="culture">The culture to get the entity for</param>
        /// <param name="versionNumber">The version of the content to get</param>
        /// <param name="relationsToInclude">The relations to include</param>
        /// <returns>The entity</returns>
        T Get<T>(Guid id, string culture, int versionNumber, string relationsToInclude) where T : class, IContent;

        /// <summary>
        /// Gets an entity of the specified type by its id, with the the specified version of the
        /// content for the specified culture and including the specified relations.
        /// </summary>
        /// <param name="entityType">The type of the entity to get</param>
        /// <param name="id">The id of the entity to get</param>
        /// <param name="culture">The culture to get the entity for</param>
        /// <param name="versionNumber">The version of the content to get</param>
        /// <param name="relationsToInclude">The relations to include</param>
        /// <returns>The entity</returns>
        IContent Get(Type entityType, Guid id, string culture, int versionNumber, string relationsToInclude);

        /// <summary>
        /// Gets an entity by its url, with the current content for the current culture.
        /// </summary>
        /// <typeparam name="T">The type of the entity to get</typeparam>
        /// <param name="url">The url of the entity to get</param>
        /// <returns>The entity</returns>
        T Get<T>(string url) where T : class, IContent;

        /// <summary>
        /// Gets an entity by its url, with the current content for the specified culture.
        /// </summary>
        /// <typeparam name="T">The type of the entity to get</typeparam>
        /// <param name="url">The url of the entity to get</param>
        /// <param name="culture">The culture to get the entity for</param>
        /// <returns>The entity</returns>
        T Get<T>(string url, string culture) where T : class, IContent;

        /// <summary>
        /// Gets an entity by its url, with the specified version of the content for the specified culture.
        /// </summary>
        /// <typeparam name="T">The type of the entity to get</typeparam>
        /// <param name="url">The url of the entity to get</param>
        /// <param name="culture">The culture to get the entity for</param>
        /// <param name="versionNumber">The version of the content to get</param>
        /// <returns>The entity</returns>
        T Get<T>(string url, string culture, int versionNumber) where T : class, IContent;

        /// <summary>
        /// Gets an entity by its url, with the specified version of the content for the specified culture.
        /// </summary>
        /// <typeparam name="T">The type of the entity to get</typeparam>
        /// <param name="url">The url of the entity to get</param>
        /// <param name="culture">The culture to get the entity for</param>
        /// <param name="relationsToInclude">The relations to include</param>
        /// <returns>The entity</returns>
        T Get<T>(string url, string culture, string relationsToInclude) where T : class, IContent;

        /// <summary>
        /// Gets an entity by its url, with the specified version of the content for the specified
        /// culture and including the specified relations.
        /// </summary>
        /// <typeparam name="T">The type of the entity to get</typeparam>
        /// <param name="url">The url of the entity to get</param>
        /// <param name="culture">The culture to get the entity for</param>
        /// <param name="versionNumber">The version of the content to get</param>
        /// <param name="relationsToInclude">The relations to include</param>
        /// <returns>The entity</returns>
        T Get<T>(string url, string culture, int versionNumber, string relationsToInclude) where T : class, IContent;

        /// <summary>
        /// Gets an entity of the specified type by its url, with the the specified version of the
        /// content for the specified culture and including the specified relations.
        /// </summary>
        /// <param name="entityType">The type of the entity to get</param>
        /// <param name="url">The url of the entity to get</param>
        /// <param name="culture">The culture to get the entity for</param>
        /// <param name="versionNumber">The version of the content to get</param>
        /// <param name="relationsToInclude">The relations to include</param>
        /// <returns>The entity</returns>
        IContent Get(Type entityType, string url, string culture, int versionNumber, string relationsToInclude);

        /// <summary>
        /// Gets the cultures the content of an entity is available for.
        /// </summary>
        /// <param name="entityType">The type of the entity</param>
        /// <param name="entityId">The id of the entity</param>
        /// <returns>The culture codes for which the entity has content</returns>
        string[] GetAvailableCultures(Type entityType, Guid entityId);

        /// <summary>
        /// Gets the values of a many-to-many relation of the entity.
        /// </summary>
        /// <param name="entity">The entity to get the relation values for</param>
        /// <param name="propertyName">The property name of the relation</param>
        /// <returns>The relation values</returns>
        IList GetExistingManyToManyRelations(object entity, string propertyName);

        /// <summary>
        /// Gets a lookup for an entity type, which is a dictionary containing the entity ids and
        /// their names.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to get the lookup for</typeparam>
        /// <returns>The lookup</returns>
        IList<SelectRelationDto<Guid>> GetLookup<TEntity>() where TEntity : class, IContent;

        /// <summary>
        /// Gets the many-to-many relation property names for an entity type.
        /// </summary>
        /// <param name="entityType">The entity type to get the relation names for</param>
        /// <returns>The many-to-many relation names</returns>
        string[] GetManyToManyRelations(Type entityType);

        /// <summary>
        /// Gets the next sort order number for an entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity to get the sort order for</typeparam>
        /// <returns>The next sort order number</returns>
        int GetNextSortOrder<T>() where T : class, IContent;

        #endregion Get

        #region Query

        /// <summary>
        /// Gets a query filtered by the specified tag for an entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity to get the query for</typeparam>
        /// <param name="tag">The tag to filter the query by</param>
        /// <returns>The query</returns>
        IQueryable<T> QueryByTag<T>(string tag) where T : class, IContent;

        /// <summary>
        /// Gets a query filtered by the specified tag for an entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity to get the query for</typeparam>
        /// <param name="tag">The tag to filter the query by</param>
        /// <param name="includes">The relations to include</param>
        /// <returns>The query</returns>
        IQueryable<T> QueryByTag<T>(string tag, string includes) where T : class, IContent;

        /// <summary>
        /// Gets a query for the current content of the current culture for an entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity to get the query for</typeparam>
        /// <returns>The query</returns>
        IQueryable<T> QueryCurrent<T>() where T : class, IContent;

        /// <summary>
        /// Gets a query for the current content of the current culture for an entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity to get the query for</typeparam>
        /// <param name="includes">The relations to include</param>
        /// <returns>The query</returns>
        IQueryable<T> QueryCurrent<T>(string includes) where T : class, IContent;

        #endregion Query

        #region Save

        /// <summary>
        /// Saves an entity to the data source.
        /// </summary>
        /// <typeparam name="T">The type of the entity to save</typeparam>
        /// <param name="entity">The entity to save</param>
        /// <param name="includes">
        /// The relations to include when loading the existing entity, if it exists. This is useful
        /// if you need to work with these relations after the default saving process is completed
        /// </param>
        /// <returns>A result to indicate whether the save was successful</returns>
        T Save<T>(T entity, string includes) where T : class, IContent;

        #endregion Save

        #region Delete

        /// <summary>
        /// Deletes the entity of the specified type with the specified id and its content from the
        /// data source.
        /// </summary>
        /// <typeparam name="T">The type of the entity to delete</typeparam>
        /// <param name="id">The id of the entity to delete</param>
        void Delete<T>(Guid id) where T : class, IContent;

        /// <summary>
        /// Deletes the content for the entity of the specified type with the specified id for the
        /// specified culture from the data source.
        /// </summary>
        /// <typeparam name="T">The type of the entity to delete</typeparam>
        /// <param name="id">The id of the entity to delete</param>
        /// <param name="culture">The culture for which to delete the entity content</param>
        void Delete<T>(Guid id, string culture) where T : class, IContent;

        /// <summary>
        /// Deletes the content for the entity of the specified type with the specified id and
        /// version number for the specified culture from the data source.
        /// </summary>
        /// <typeparam name="T">The type of the entity to delete</typeparam>
        /// <param name="id">The id of the entity to delete</param>
        /// <param name="culture">The culture for which to delete the entity content</param>
        /// <param name="versionNumber">The version number of the content to delete for the entity</param>
        void Delete<T>(Guid id, string culture, int versionNumber) where T : class, IContent;

        /// <summary>
        /// Deletes the content for the entity of the specified type with the specified id and
        /// version number for the specified culture from the data source. If the trash bin is
        /// enabled, the log message is stored with the content to view the reason for deletion in
        /// the trashbin.
        /// </summary>
        /// <typeparam name="T">The type of the entity to delete</typeparam>
        /// <param name="id">The id of the entity to delete</param>
        /// <param name="culture">The culture for which to delete the entity content</param>
        /// <param name="versionNumber">The version number of the content to delete for the entity</param>
        /// <param name="log">The log message</param>
        void Delete<T>(Guid id, string culture, int versionNumber, string log) where T : class, IContent;

        /// <summary>
        /// Deletes the content for the entity of the specified type with the specified id and
        /// version number for the specified culture from the data source. If the trash bin is
        /// enabled, the log message is stored with the content to view the reason for deletion in
        /// the trashbin.
        /// </summary>
        /// <param name="entityType">The type of the entity to delete</param>
        /// <param name="id">The id of the entity to delete</param>
        /// <param name="culture">The culture for which to delete the entity content</param>
        /// <param name="versionNumber">The version number of the content to delete for the entity</param>
        /// <param name="log">The log message</param>
        void Delete(Type entityType, Guid id, string culture, int versionNumber, string log);

        #endregion Delete
    }
}
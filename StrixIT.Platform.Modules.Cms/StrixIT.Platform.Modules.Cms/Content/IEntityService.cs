//-----------------------------------------------------------------------
// <copyright file="IEntityService.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using StrixIT.Platform.Core;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface for the platform entity service
    /// </summary>
    /// <typeparam name="TModel">The type of the view model the service is for</typeparam>
    public interface IEntityService<TModel> : ICrudService<Guid, TModel> where TModel : EntityViewModel
    {
        #region Get

        /// <summary>
        /// Gets the id of an entity using a string that contains either the id or the url of an entity.
        /// </summary>
        /// <param name="idOrUrl">A string that contains either the id or the url of an entity</param>
        /// <returns>The id of the entity, or NULL if no entity is found</returns>
        Guid? GetId(string idOrUrl);

        /// <summary>
        /// Returns the url for the entity with the specified id.
        /// </summary>
        /// <param name="id">The entity id</param>
        /// <returns>The url</returns>
        string GetUrl(Guid id);

        /// <summary>
        /// Gets the content of the specified type for the entity with the specified id, for the specified culture
        /// and current version.
        /// </summary>
        /// <param name="id">The entity id</param>
        /// <param name="culture">The culture to get the content for</param>
        /// <returns>The content</returns>
        TModel Get(Guid? id, string culture);

        /// <summary>
        /// Gets the content of the specified type for the entity with the specified id, for the specified culture
        /// and current version including the specified relations.
        /// </summary>
        /// <param name="id">The entity id</param>
        /// <param name="culture">The culture to get the content for</param>
        /// <param name="relationsToInclude">The relations to include</param>
        /// <returns>The content</returns>
        TModel Get(Guid id, string culture, string relationsToInclude);

        /// <summary>
        /// Gets the content of the specified type for the entity with the specified id, for the specified culture
        /// and specified version.
        /// </summary>
        /// <param name="id">The entity id</param>
        /// <param name="culture">The culture to get the content for</param>
        /// <param name="versionNumber">The version of the content to get</param>
        /// <returns>The content</returns>
        TModel Get(Guid id, string culture, int versionNumber);

        /// <summary>
        /// Gets the content of the specified type for the entity with the specified id, for the specified culture
        /// and specified version including the specified relations.
        /// </summary>
        /// <param name="id">The entity id</param>
        /// <param name="culture">The culture to get the content for</param>
        /// <param name="versionNumber">The version of the content to get</param>
        /// <param name="relationsToInclude">The relations to include</param>
        /// <returns>The content</returns>
        TModel Get(Guid id, string culture, int versionNumber, string relationsToInclude);

        /// <summary>
        /// Gets the content of the specified type for the entity with the specified url, for the current culture
        /// and current version.
        /// </summary>
        /// <param name="url">The entity url</param>
        /// <returns>The content</returns>
        TModel Get(string url);

        /// <summary>
        /// Gets the content of the specified type for the entity with the specified url, for the specified culture
        /// and current version.
        /// </summary>
        /// <param name="url">The entity url</param>
        /// <param name="culture">The culture to get the content for</param>
        /// <returns>The content</returns>
        TModel Get(string url, string culture);

        /// <summary>
        /// Gets the content of the specified type for the entity with the specified url, for the specified culture
        /// and current version including the specified relations.
        /// </summary>
        /// <param name="url">The entity url</param>
        /// <param name="culture">The culture to get the content for</param>
        /// <param name="relationsToInclude">The relations to include</param>
        /// <returns>The content</returns>
        TModel Get(string url, string culture, string relationsToInclude);

        /// <summary>
        /// Gets the content of the specified type for the entity with the specified url, for the specified culture
        /// and specified version.
        /// </summary>
        /// <param name="url">The entity url</param>
        /// <param name="culture">The culture to get the content for</param>
        /// <param name="versionNumber">The version of the content to get</param>
        /// <returns>The content</returns>
        TModel Get(string url, string culture, int versionNumber);

        /// <summary>
        /// Gets the content of the specified type for the entity with the specified url, for the specified culture
        /// and specified version including the specified relations.
        /// </summary>
        /// <param name="url">The entity url</param>
        /// <param name="culture">The culture to get the content for</param>
        /// <param name="versionNumber">The version of the content to get</param>
        /// <param name="relationsToInclude">The relations to include</param>
        /// <returns>The content</returns>
        TModel Get(string url, string culture, int versionNumber, string relationsToInclude);

        /// <summary>
        /// Gets the content entity with the specified url, for the specified culture if available or any other culture
        /// otherwise, including the specified relations.
        /// </summary>
        /// <param name="url">The entity url</param>
        /// <param name="culture">The culture to get the content for</param>
        /// <param name="relationsToInclude">The relations to include</param>
        /// <returns>The content</returns>
        TModel GetAny(string url, string culture, string relationsToInclude);

        #endregion

        #region Cache

        /// <summary>
        /// Gets cached content for the specified entity type with the specified url for the
        /// current culture and version.
        /// </summary>
        /// <param name="url">The entity url</param>
        /// <param name="permissionKey">An optional cache key to cache for a specific permission only</param>
        /// <returns>The cached content</returns>
        TModel GetCached(string url, string permissionKey = null);

        #endregion

        #region Tags

        /// <summary>
        /// Gets the tags for an entity using its id.
        /// </summary>
        /// <param name="entityId">The entity id</param>
        /// <returns>The tags for the entity</returns>
        IList<TermViewModel> GetTags(Guid entityId);

        /// <summary>
        /// Gets all non-system tags from the data source.
        /// </summary>
        /// <returns>A list of tags</returns>
        IList<TermViewModel> GetAllTags();

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the content for the entity with the specified id for the specified culture from the data source.
        /// </summary>
        /// <param name="id">The id of the entity to delete or delete content for</param>
        /// <param name="culture">The culture to delete content for, when specified</param>
        void Delete(Guid id, string culture);

        /// <summary>
        /// Deletes the content for the entity with the specified id for the specified culture and version number from the data source.
        /// </summary>
        /// <param name="id">The id of the entity to delete or delete content for</param>
        /// <param name="culture">The culture to delete content for, when specified</param>
        /// <param name="versionNumber">The content version to delete, when specified</param>
        void Delete(Guid id, string culture, int versionNumber);

        /// <summary>
        /// Deletes the content for the entity with the specified id for the specified culture and version number from the data source.
        /// A message can be logged when the trash bin is active.
        /// </summary>
        /// <param name="id">The id of the entity to delete or delete content for</param>
        /// <param name="culture">The culture to delete content for, when specified</param>
        /// <param name="versionNumber">The content version to delete, when specified</param>
        /// <param name="log">The message to log, when the trash bin is active</param>
        void Delete(Guid id, string culture, int versionNumber, string log);

        /// <summary>
        /// Deletes the content for the entity with the specified id for the specified culture and version number from the data source.
        /// A message can be logged when the trash bin is active. When specified, the changes will be persisted to the data source.
        /// </summary>
        /// <param name="id">The id of the entity to  delete content for</param>
        /// <param name="culture">The culture to delete content for</param>
        /// <param name="versionNumber">The content version to delete</param>
        /// <param name="log">The message to log, when the trash bin is active</param>
        /// <param name="saveChanges">True if the delete should be persisted to the data source, false otherwise</param>
        void Delete(Guid id, string culture, int versionNumber, string log, bool saveChanges);

        #endregion

        #region Versions

        /// <summary>
        /// Gets a list of entity version models for the specified entity for the specified culture using the data manager parameters. 
        /// </summary>
        /// <param name="id">The entity id</param>
        /// <param name="culture">The culture to get the version list for</param>
        /// <param name="filter">The data manager parameters to use</param>
        /// <returns>The list of version view models</returns>
        IList<VersionViewModel> GetVersionList(Guid id, string culture, FilterOptions filter);

        /// <summary>
        /// Restores the version with the specified number for the specified entity, writing the message to the entity log.
        /// </summary>
        /// <param name="id">The entity id</param>
        /// <param name="versionNumber">The version number to restore</param>
        /// <param name="log">The message to write to the entity log</param>
        /// <returns>The restored entity version</returns>
        TModel RestoreVersion(Guid id, int versionNumber, string log);

        #endregion
    }
}

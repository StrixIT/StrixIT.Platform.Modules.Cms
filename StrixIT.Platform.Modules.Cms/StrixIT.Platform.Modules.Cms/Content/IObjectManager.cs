//-----------------------------------------------------------------------
// <copyright file="IObjectManager.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Linq;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface for the object manager.
    /// </summary>
    public interface IObjectManager
    {
        /// <summary>
        /// Gets an object by its id.
        /// </summary>
        /// <typeparam name="T">The type of the object to get</typeparam>
        /// <param name="id">The object id</param>
        /// <returns>The object</returns>
        T Get<T>(object id) where T : class;

        /// <summary>
        /// Gets an object by its id.
        /// </summary>
        /// <typeparam name="T">The type of the object to get</typeparam>
        /// <param name="id">The object id</param>
        /// <param name="includes">The relations to include, separated by commas</param>
        /// <returns>The object</returns>
        T Get<T>(object id, string includes) where T : class;

        /// <summary>
        /// Gets an entity of the specified type by its id.
        /// </summary>
        /// <param name="objectType">The type of the entity to get</param>
        /// <param name="id">The id of the entity to get</param>
        /// <returns>The entity</returns>
        object Get(Type objectType, object id);

        /// <summary>
        /// Gets an object of the specified type by its id.
        /// </summary>
        /// <param name="objectType">The type of the object to get</param>
        /// <param name="id">The id of the entity to get</param>
        /// <param name="includes">The relations to include, separated by commas</param>
        /// <returns>The entity</returns>
        object Get(Type objectType, object id, string includes);

        /// <summary>
        /// Gets a query for an object type.
        /// </summary>
        /// <typeparam name="T">The object type to get the query for</typeparam>
        /// <returns>The query</returns>
        IQueryable<T> Query<T>() where T : class;

        /// <summary>
        /// Gets a query for an object type.
        /// </summary>
        /// <typeparam name="T">The object type to get the query for</typeparam>
        /// <param name="includes">The relations to include</param>
        /// <returns>The query</returns>
        IQueryable<T> Query<T>(string includes) where T : class;

        /// <summary>
        /// Gets a query for an object type, including the specified relations.
        /// </summary>
        /// <param name="objectType">The object type to get the query for</param>
        /// <returns>The query</returns>
        IQueryable Query(Type objectType);

        /// <summary>
        /// Gets a query for an object type, including the specified relations.
        /// </summary>
        /// <param name="objectType">The object type to get the query for</param>
        /// <param name="includes">The relations to include</param>
        /// <returns>The query</returns>
        IQueryable Query(Type objectType, string includes);

        /// <summary>
        /// Saves the specified object to the data source.
        /// </summary>
        /// <typeparam name="T">The type of the object to save</typeparam>
        /// <param name="entity">The object to save</param>
        /// <returns>A result to indicate whether the save was successful</returns>
        T Save<T>(T entity) where T : class;

        /// <summary>
        /// Deletes the specified object from the data source.
        /// </summary>
        /// <typeparam name="T">The type of the object to delete</typeparam>
        /// <param name="entity">The object to delete</param>
        void Delete<T>(T entity) where T : class;
    }
}
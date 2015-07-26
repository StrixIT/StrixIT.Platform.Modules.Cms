#region Apache License
//-----------------------------------------------------------------------
// <copyright file="IPlatformDataSource.cs" company="StrixIT">
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
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface for the platform data source.
    /// </summary>
    public interface IPlatformDataSource : IDataSource
    {
        /// <summary>
        /// Gets the file system wrapper used.
        /// </summary>
        IFileSystemWrapper FileSystemWrapper { get; }

        /// <summary>
        /// Gets the key value of the entity with the specified key type.
        /// </summary>
        /// <typeparam name="TKey">The key type</typeparam>
        /// <param name="entity">The entity to get the key value for</param>
        /// <returns>The entity key</returns>
        TKey GetKeyValue<TKey>(object entity);

        /// <summary>
        /// Gets a query for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <param name="includes">The relations to include</param>
        /// <returns>The query</returns>
        IQueryable<T> Query<T>(string includes) where T : class;

        /// <summary>
        /// Gets a query for the specified entity type.
        /// </summary>
        /// <param name="entityType">The entity type</param>
        /// <returns>The query</returns>
        IQueryable Query(Type entityType);

        /// <summary>
        /// Gets a query for the specified entity type.
        /// </summary>
        /// <param name="entityType">The entity type</param>
        /// <param name="relations">The relations to include</param>
        /// <returns>The query</returns>
        IQueryable Query(Type entityType, string relations);

        /// <summary>
        /// Gets the names of the many-to-many relations of an entity type.
        /// </summary>
        /// <param name="entityType">The entity type to get the relations for</param>
        /// <returns>The names of the many-to-many relations</returns>
        string[] GetManyToManyRelations(Type entityType);

        /// <summary>
        /// Ignores an entity, so changes made to it will no longer be saved to the data source. Used to create new versions.
        /// </summary>
        /// <param name="entity">The entity to ignore</param>
        void Ignore(object entity);

        /// <summary>
        /// Gets a list of changed properties of the entity, the property names and their old and new values.
        /// </summary>
        /// <param name="entity">The entity to get the changed properties for</param>
        /// <returns>The list of modified property values</returns>
        IList<ModifiedPropertyValue> GetModifiedPropertyValues(object entity);

        /// <summary>
        /// Gets the values of a many-to-many relation for an entity.
        /// </summary>
        /// <param name="entity">The entity to get the relation values for</param>
        /// <param name="propertyName">The name of the relation property</param>
        /// <returns>The relation values</returns>
        IList GetExistingManyToManyRelations(object entity, string propertyName);
    }
}
#region Apache License

//-----------------------------------------------------------------------
// <copyright file="IEntityHelper.cs" company="StrixIT">
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
using System.Collections.Generic;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface for the entity helper.
    /// </summary>
    public interface IEntityHelper
    {
        #region Public Properties

        /// <summary>
        /// Gets the list of loaded entity types.
        /// </summary>
        IList<EntityType> EntityTypes { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Activates a number of actions for an entity type.
        /// </summary>
        /// <param name="entityType">The entity type to activate the actions for</param>
        /// <param name="actions">The actions to activate</param>
        void ActivateServices(Type entityType, IEnumerable<string> actions);

        /// <summary>
        /// Deactivates a number of actions for an entity type.
        /// </summary>
        /// <param name="entityType">The entity type to deactivate the actions for</param>
        /// <param name="actions">The actions to deactivate</param>
        void DeactivateServices(Type entityType, IEnumerable<string> actions);

        /// <summary>
        /// Gets an entity type based on its id.
        /// </summary>
        /// <param name="entityTypeId">The id of the entity type to get</param>
        /// <returns>The entity type</returns>
        Type GetEntityType(Guid entityTypeId);

        /// <summary>
        /// Gets an entity type based on its type name.
        /// </summary>
        /// <param name="entityTypeName">The full name of the entity type to get</param>
        /// <returns>The entity type</returns>
        Type GetEntityType(string entityTypeName);

        /// <summary>
        /// Gets the id of an antity type.
        /// </summary>
        /// <param name="entityType">The entity type to get the id for</param>
        /// <returns>The entity type id</returns>
        Guid GetEntityTypeId(Type entityType);

        /// <summary>
        /// Gets the file id property names for an entity type.
        /// </summary>
        /// <param name="entityType">The entity type to get the file id property names for</param>
        /// <returns>The file id property names</returns>
        string[] GetFileIdProperties(Type entityType);

        /// <summary>
        /// Gets the file property names for an entity type.
        /// </summary>
        /// <param name="entityType">The entity type to get the file property names for</param>
        /// <returns>The file property names</returns>
        string[] GetFileProperties(Type entityType);

        /// <summary>
        /// Gets the object map for an entity type.
        /// </summary>
        /// <param name="entityType">The entity type to get the object map for</param>
        /// <returns>The object map</returns>
        ObjectMap GetObjectMap(Type entityType);

        /// <summary>
        /// Gets a value indicating whether the action is active for the entity type.
        /// </summary>
        /// <param name="entityType">The entity type to check the action for</param>
        /// <param name="action">The action to check</param>
        /// <returns>True if the action is active for the entity type, false otherwise.</returns>
        bool IsServiceActive(Type entityType, string action);

        #endregion Public Methods
    }
}
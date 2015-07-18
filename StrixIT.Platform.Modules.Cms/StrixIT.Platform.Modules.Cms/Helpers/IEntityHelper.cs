//-----------------------------------------------------------------------
// <copyright file="IEntityHelper.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface for the entity helper.
    /// </summary>
    public interface IEntityHelper
    {
        /// <summary>
        /// Gets the list of loaded entity types.
        /// </summary>
        IList<EntityType> EntityTypes { get; }

        /// <summary>
        /// Gets the id of an antity type.
        /// </summary>
        /// <param name="entityType">The entity type to get the id for</param>
        /// <returns>The entity type id</returns>
        Guid GetEntityTypeId(Type entityType);

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
        /// Gets a value indicating whether the action is active for the entity type.
        /// </summary>
        /// <param name="entityType">The entity type to check the action for</param>
        /// <param name="action">The action to check</param>
        /// <returns>True if the action is active for the entity type, false otherwise.</returns>
        bool IsServiceActive(Type entityType, string action);

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
        /// Gets the object map for an entity type.
        /// </summary>
        /// <param name="entityType">The entity type to get the object map for</param>
        /// <returns>The object map</returns>
        ObjectMap GetObjectMap(Type entityType);

        /// <summary>
        /// Gets the file property names for an entity type.
        /// </summary>
        /// <param name="entityType">The entity type to get the file property names for</param>
        /// <returns>The file property names</returns>
        string[] GetFileProperties(Type entityType);

        /// <summary>
        /// Gets the file id property names for an entity type.
        /// </summary>
        /// <param name="entityType">The entity type to get the file id property names for</param>
        /// <returns>The file id property names</returns>
        string[] GetFileIdProperties(Type entityType);
    }
}
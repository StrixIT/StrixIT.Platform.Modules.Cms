#region Apache License

//-----------------------------------------------------------------------
// <copyright file="EntityHelper.cs" company="StrixIT">
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
using System;
using System.Collections.Generic;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The entity helper static class, used to easily use entity helper methods anywhere.
    /// </summary>
    public static class EntityHelper
    {
        #region Private Fields

        private static IEntityHelper _helper;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the list of loaded entity types.
        /// </summary>
        public static IList<EntityType> EntityTypes
        {
            get
            {
                return Helper.EntityTypes;
            }
        }

        #endregion Public Properties

        #region Private Properties

        /// <summary>
        /// Gets the entity helper used.
        /// </summary>
        private static IEntityHelper Helper
        {
            get
            {
                if (_helper == null)
                {
                    _helper = DependencyInjector.Get<IEntityHelper>();
                }

                return _helper;
            }
        }

        #endregion Private Properties

        #region Public Methods

        /// <summary>
        /// Activates a number of actions for an entity type.
        /// </summary>
        /// <param name="entityType">The entity type to activate the actions for</param>
        /// <param name="actions">The actions to activate</param>
        public static void ActivateServices(Type entityType, IEnumerable<string> actions)
        {
            Helper.ActivateServices(entityType, actions);
        }

        /// <summary>
        /// Deactivates a number of actions for an entity type.
        /// </summary>
        /// <param name="entityType">The entity type to deactivate the actions for</param>
        /// <param name="actions">The actions to deactivate</param>
        public static void DeactivateServices(Type entityType, IEnumerable<string> actions)
        {
            Helper.DeactivateServices(entityType, actions);
        }

        /// <summary>
        /// Gets an entity type based on its id.
        /// </summary>
        /// <param name="entityTypeId">The id of the entity type to get</param>
        /// <returns>The entity type</returns>
        public static Type GetEntityType(Guid entityTypeId)
        {
            return Helper.GetEntityType(entityTypeId);
        }

        /// <summary>
        /// Gets an entity type based on its type name.
        /// </summary>
        /// <param name="entityTypeName">The full name of the entity type to get</param>
        /// <returns>The entity type</returns>
        public static Type GetEntityType(string entityTypeName)
        {
            return Helper.GetEntityType(entityTypeName);
        }

        /// <summary>
        /// Gets the id of an antity type.
        /// </summary>
        /// <param name="entityType">The entity type to get the id for</param>
        /// <returns>The entity type id</returns>
        public static Guid GetEntityTypeId(Type entityType)
        {
            return Helper.GetEntityTypeId(entityType);
        }

        /// <summary>
        /// Gets the file id property names for an entity type.
        /// </summary>
        /// <param name="entityType">The entity type to get the file id property names for</param>
        /// <returns>The file id property names</returns>
        public static string[] GetFileIdProperties(Type entityType)
        {
            return Helper.GetFileIdProperties(entityType);
        }

        /// <summary>
        /// Gets the file property names for an entity type.
        /// </summary>
        /// <param name="entityType">The entity type to get the file property names for</param>
        /// <returns>The file property names</returns>
        public static string[] GetFileProperties(Type entityType)
        {
            return Helper.GetFileProperties(entityType);
        }

        /// <summary>
        /// Gets the object map for an entity type.
        /// </summary>
        /// <param name="entityType">The entity type to get the object map for</param>
        /// <returns>The object map</returns>
        public static ObjectMap GetObjectMap(Type entityType)
        {
            return Helper.GetObjectMap(entityType);
        }

        /// <summary>
        /// Gets a value indicating whether the action is active for the entity type.
        /// </summary>
        /// <param name="entityType">The entity type to check the action for</param>
        /// <param name="action">The action to check</param>
        /// <returns>True if the action is active for the entity type, false otherwise.</returns>
        public static bool IsServiceActive(Type entityType, string action)
        {
            return Helper.IsServiceActive(entityType, action);
        }

        /// <summary>
        /// Sets the entity helper to use
        /// </summary>
        /// <param name="helper">The entity helper to use</param>
        public static void SetHelper(IEntityHelper helper)
        {
            _helper = helper;
        }

        #endregion Public Methods
    }
}
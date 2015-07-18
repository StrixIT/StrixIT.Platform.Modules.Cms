//-----------------------------------------------------------------------
// <copyright file="IEntityServiceManager.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface for the service used to handle all entity service actions.
    /// </summary>
    public interface IEntityServiceManager
    {
        /// <summary>
        /// Gets all entity service records for all entities.
        /// </summary>
        /// <returns>The entity service records</returns>
        EntityServiceCollection GetManagerActionRecords();

        /// <summary>
        /// Saves all entity service records modified to the database.
        /// </summary>
        /// <param name="records">The entity service records modified</param>
        /// <returns>True if saving the changes was successful, false otherwise</returns>
        bool SaveActions(EntityServiceCollection records);
    }
}
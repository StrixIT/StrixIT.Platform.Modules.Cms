#region Apache License
//-----------------------------------------------------------------------
// <copyright file="IEntityServiceManager.cs" company="StrixIT">
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
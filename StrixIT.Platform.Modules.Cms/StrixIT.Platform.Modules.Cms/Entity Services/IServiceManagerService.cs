#region Apache License
//-----------------------------------------------------------------------
// <copyright file="IServiceManagerService.cs" company="StrixIT">
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

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface for the service manager service for the Cms.
    /// </summary>
    public interface IServiceManagerService
    {
        /// <summary>
        /// Gets all service action records.
        /// </summary>
        /// <returns>A collection of service action records</returns>
        EntityServiceCollection GetServiceActionRecords();

        /// <summary>
        /// Saves the service action records to the data source.
        /// </summary>
        /// <param name="records">The records to save</param>
        /// <returns>True if the save was successful, false otherwise.</returns>
        bool SaveActionRecords(EntityServiceCollection records);
    }
}
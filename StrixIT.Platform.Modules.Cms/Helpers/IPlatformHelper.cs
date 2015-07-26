#region Apache License

//-----------------------------------------------------------------------
// <copyright file="IPlatformHelper.cs" company="StrixIT">
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
    public interface IPlatformHelper
    {
        #region Public Properties

        /// <summary>
        /// Gets the dictionary of services and their action lists currently supported by the platform.
        /// </summary>
        IDictionary<string, IList<string>> Services { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Adds a service to the platform, along with a list of actions it supports.
        /// </summary>
        /// <param name="name">The service name</param>
        /// <param name="actions">The list of actions the service supports</param>
        void AddService(string name, IList<string> actions);

        /// <summary>
        /// Clears the cached group name dictionary.
        /// </summary>
        void ClearGroupNameDictionary();

        /// <summary>
        /// Clears the cached user name dictionary.
        /// </summary>
        void ClearUserNameDictionary();

        /// <summary>
        /// Gets the name of the group with the specified id.
        /// </summary>
        /// <param name="id">The group id</param>
        /// <returns>The group name</returns>
        string GetGroupName(Guid id);

        /// <summary>
        /// Gets the email of the user with the specified id.
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns>The user email</returns>
        string GetUserEmail(Guid id);

        /// <summary>
        /// Gets the name of the user with the specified id.
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns>The user name</returns>
        string GetUserName(Guid id);

        #endregion Public Methods
    }
}
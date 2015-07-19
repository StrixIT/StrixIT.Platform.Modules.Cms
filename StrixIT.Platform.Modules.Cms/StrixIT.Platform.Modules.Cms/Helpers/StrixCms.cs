#region Apache License
//-----------------------------------------------------------------------
// <copyright file="StrixCms.cs" company="StrixIT">
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
using System.Configuration;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A static class to hold all shared data for the platform.
    /// </summary>
    public static class StrixCms
    {
        private static CmsConfiguration _cmsConfig;
        private static IPlatformHelper _helper;

        /// <summary>
        /// Gets the tools configuration section.
        /// </summary>
        public static CmsConfiguration Config
        {
            get
            {
                if (_cmsConfig == null)
                {
                    _cmsConfig = new CmsConfiguration();
                }

                return _cmsConfig;
            }
        }

        /// <summary>
        /// Gets the dictionary of services and their action lists currently supported by the platform.
        /// </summary>
        public static IDictionary<string, IList<string>> Services
        {
            get
            {
                return Helper.Services;
            }
        }

        private static IPlatformHelper Helper
        {
            get
            {
                if (_helper == null)
                {
                    _helper = DependencyInjector.Get<IPlatformHelper>();
                }

                return _helper;
            }
        }

        /// <summary>
        /// Set the platform helper to use.
        /// </summary>
        /// <param name="helper">The helper</param>
        public static void SetHelper(IPlatformHelper helper)
        {
            _helper = helper;
        }

        /// <summary>
        /// Adds a service to the platform, along with a list of actions it supports.
        /// </summary>
        /// <param name="name">The service name</param>
        /// <param name="actions">The list of actions the service supports</param>
        public static void AddService(string name, IList<string> actions)
        {
            Helper.AddService(name, actions);
        }

        /// <summary>
        /// Gets the name of the user with the specified id.
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns>The user name</returns>
        public static string GetUserName(Guid id)
        {
            return Helper.GetUserName(id);
        }

        /// <summary>
        /// Gets the email of the user with the specified id.
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns>The user email</returns>
        public static string GetUserEmail(Guid id)
        {
            return Helper.GetUserEmail(id);
        }

        /// <summary>
        /// Gets the name of the group with the specified id.
        /// </summary>
        /// <param name="id">The group id</param>
        /// <returns>The group name</returns>
        public static string GetGroupName(Guid id)
        {
            return Helper.GetGroupName(id);
        }

        /// <summary>
        /// Clears the cached user name dictionary.
        /// </summary>
        public static void ClearUserNameDictionary()
        {
            Helper.ClearUserNameDictionary();
        }

        /// <summary>
        /// Clears the cached group name dictionary.
        /// </summary>
        public static void ClearGroupNameDictionary()
        {
            Helper.ClearGroupNameDictionary();
        }
    }
}
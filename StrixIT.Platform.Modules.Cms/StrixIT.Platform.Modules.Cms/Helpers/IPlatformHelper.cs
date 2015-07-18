//-----------------------------------------------------------------------
// <copyright file="IPlatformHelper.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace StrixIT.Platform.Modules.Cms
{
    public interface IPlatformHelper
    {
        /// <summary>
        /// Gets the dictionary of services and their action lists currently supported by the platform.
        /// </summary>
        IDictionary<string, IList<string>> Services { get; }

        /// <summary>
        /// Adds a service to the platform, along with a list of actions it supports.
        /// </summary>
        /// <param name="name">The service name</param>
        /// <param name="actions">The list of actions the service supports</param>
        void AddService(string name, IList<string> actions);

        /// <summary>
        /// Gets the name of the user with the specified id.
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns>The user name</returns>
        string GetUserName(Guid id);

        /// <summary>
        /// Gets the email of the user with the specified id.
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns>The user email</returns>
        string GetUserEmail(Guid id);

        /// <summary>
        /// Gets the name of the group with the specified id.
        /// </summary>
        /// <param name="id">The group id</param>
        /// <returns>The group name</returns>
        string GetGroupName(Guid id);

        /// <summary>
        /// Clears the cached user name dictionary.
        /// </summary>
        void ClearUserNameDictionary();

        /// <summary>
        /// Clears the cached group name dictionary.
        /// </summary>
        void ClearGroupNameDictionary();
    }
}
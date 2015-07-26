#region Apache License

//-----------------------------------------------------------------------
// <copyright file="CmsRoleNames.cs" company="StrixIT">
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

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The role names used within the Cms for restricting access.
    /// </summary>
    public static class CmsRoleNames
    {
        #region Public Fields

        /// <summary>
        /// All roles that have contributor rights.
        /// </summary>
        public const string CONTRIBUTORROLES = "Administrator, GroupAdministrator, Editor, Contributor";

        /// <summary>
        /// All roles that have editor rights.
        /// </summary>
        public const string EDITORROLES = "Administrator, GroupAdministrator, Editor";

        /// <summary>
        /// All roles that have user rights.
        /// </summary>
        public const string USERROLES = "Administrator, GroupAdministrator, Editor, Contributor, RegisteredUser";

        #endregion Public Fields
    }
}
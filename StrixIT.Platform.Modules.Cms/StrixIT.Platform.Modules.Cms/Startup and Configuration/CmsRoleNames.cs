//-----------------------------------------------------------------------
// <copyright file="CmsRoleNames.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The role names used within the Cms for restricting access.
    /// </summary>
    public static class CmsRoleNames
    {
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
    }
}
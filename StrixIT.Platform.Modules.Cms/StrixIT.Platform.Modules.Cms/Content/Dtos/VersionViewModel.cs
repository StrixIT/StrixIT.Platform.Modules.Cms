//-----------------------------------------------------------------------
// <copyright file="VersionViewModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using StrixIT.Platform.Modules.Cms;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The view model for the version overview.
    /// </summary>
    public class VersionViewModel
    {
        /// <summary>
        /// Gets or sets the id the version is for.
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the version number.
        /// </summary>
        public int VersionNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the version is the current version.
        /// </summary>
        public bool IsCurrentVersion { get; set; }

        /// <summary>
        /// Gets or sets the name of the user who created this version.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time this version was created.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the log text entered for the version.
        /// </summary>
        public string Log { get; set; }
    }
}
#region Apache License
//-----------------------------------------------------------------------
// <copyright file="VersionViewModel.cs" company="StrixIT">
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
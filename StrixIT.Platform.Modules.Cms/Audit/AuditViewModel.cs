#region Apache License

//-----------------------------------------------------------------------
// <copyright file="AuditViewModel.cs" company="StrixIT">
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

using Newtonsoft.Json;
using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The base view model for all autit entities.
    /// </summary>
    public abstract class AuditViewModel : PlatformBaseViewModel, IAudit
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the user can edit this view model.
        /// </summary>
        public bool CanEdit { get; set; }

        /// <summary>
        /// Gets or sets the name of the user that created or last modified this entity.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the id of the user that created this entity.
        /// </summary>
        [JsonIgnore]
        public Guid CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time this entity was created.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the name of the user that created or last modified this entity.
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the id of the user that last updated this entity, if any.
        /// </summary>
        [JsonIgnore]
        public Guid UpdatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user that created or last modified this entity.
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        #endregion Public Properties
    }
}
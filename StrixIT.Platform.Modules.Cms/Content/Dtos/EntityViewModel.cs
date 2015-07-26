#region Apache License

//-----------------------------------------------------------------------
// <copyright file="EntityViewModel.cs" company="StrixIT">
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
using System.Collections.Generic;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The base view model for all content entities.
    /// </summary>
    public abstract class EntityViewModel : AuditViewModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the codes of the cultures the content is available for.
        /// </summary>
        public string[] AvailableCultures { get; set; }

        /// <summary>
        /// Gets or sets the content culture code.
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the content entity id.
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this content is the current version.
        /// </summary>
        public bool IsCurrentVersion { get; set; }

        /// <summary>
        /// Gets or sets the content name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the user that published this content.
        /// </summary>
        public string PublishedBy { get; set; }

        /// <summary>
        /// Gets or sets the id of the user that published this content.
        /// </summary>
        [JsonIgnore]
        public Guid? PublishedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the date this content was published on.
        /// </summary>
        public DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the sort order for this content.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the tags for the content.
        /// </summary>
        public IList<TermViewModel> Tags { get; set; }

        /// <summary>
        /// Gets or sets the content url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the log used for versioning.
        /// </summary>
        public string VersionLog { get; set; }

        /// <summary>
        /// Gets or sets the content version number.
        /// </summary>
        public int VersionNumber { get; set; }

        #endregion Public Properties
    }
}
#region Apache License

//-----------------------------------------------------------------------
// <copyright file="Document.cs" company="StrixIT">
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

using StrixIT.Platform.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The class for all file content within the platform.
    /// </summary>
    public class Document : ContentBase, IContent
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the author of the File.
        /// </summary>
        [StringLength(250)]
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets the user of the author of the file.
        /// </summary>
        public UserData AuthorUser { get; set; }

        /// <summary>
        /// Gets or sets the Id of the user of the author of the File.
        /// </summary>
        [StrixNotDefaultWithMembership]
        public Guid? AuthorUserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time the File was created.
        /// </summary>
        [StrixNotDefault]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the document description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the number of times this document has been downloaded.
        /// </summary>
        public int DownloadCount { get; set; }

        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        public PlatformEntity Entity { get; set; }

        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        [StrixRequired]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the file for this document.
        /// </summary>
        [FileUpload]
        public File File { get; set; }

        /// <summary>
        /// Gets or sets the id of the file for this document.
        /// </summary>
        [StrixRequired]
        public Guid? FileId { get; set; }

        /// <summary>
        /// Gets or sets the location the File was created (used for media, a.o.).
        /// </summary>
        [StringLength(250)]
        public string Location { get; set; }

        #endregion Public Properties
    }
}
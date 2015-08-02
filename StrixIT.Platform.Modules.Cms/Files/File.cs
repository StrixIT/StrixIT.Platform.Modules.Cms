#region Apache License

//-----------------------------------------------------------------------
// <copyright file="File.cs" company="StrixIT">
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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A class describing the files uploaded in the Cms.
    /// </summary>
    public class File
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        [StrixRequired]
        [StringLength(5)]
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the name of the file without extension.
        /// </summary>
        [StrixRequired]
        [StringLength(200)]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the path of the file within the upload folder.
        /// </summary>
        [StrixRequired]
        [StringLength(200)]
        public string Folder { get; set; }

        /// <summary>
        /// Gets or sets the group this file belongs to.
        /// </summary>
        public GroupData Group { get; set; }

        /// <summary>
        /// Gets or sets the id of the group this file belongs to.
        /// </summary>
        [StrixRequiredWithMembershipAttribute]
        public Guid GroupId { get; set; }

        /// <summary>
        /// Gets or sets the file id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the file before it was uploaded.
        /// </summary>
        [StrixNotDefault]
        [StringLength(300)]
        public string OriginalName { get; set; }

        /// <summary>
        /// Gets or sets the path of the file within the upload folder.
        /// </summary>
        [StrixRequired]
        [StringLength(300)]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the size of the File in bytes.
        /// </summary>
        public long? Size { get; set; }

        /// <summary>
        /// Gets or sets the tags for this file. Used to track file usage.
        /// </summary>
        public ICollection<Term> Tags { get; set; }

        /// <summary>
        /// Gets or sets the user who uploaded this file.
        /// </summary>
        public UserData UploadedByUser { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who uploaded this file.
        /// </summary>
        [StrixRequiredWithMembershipAttribute]
        public Guid UploadedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the date this file was uploaded.
        /// </summary>
        [StrixRequired]
        public DateTime UploadedOn { get; set; }

        #endregion Public Properties
    }
}
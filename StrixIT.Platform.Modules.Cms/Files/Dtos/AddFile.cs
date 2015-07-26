#region Apache License

//-----------------------------------------------------------------------
// <copyright file="AddFile.cs" company="StrixIT">
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

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The view model for adding files.
    /// </summary>
    public class AddFile
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddFile"/> class.
        /// </summary>
        public AddFile() : this("FileId")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddFile"/> class.
        /// </summary>
        /// <param name="fileIdPropertyName">The id property of the file on the entity</param>
        public AddFile(string fileIdPropertyName)
        {
            this.ShowTitle = true;
            this.FileIdPropertyName = fileIdPropertyName;
            this.KeepAspectRatio = true;
            this.Unzip = true;
            this.FileTypes = string.Join(",", StrixCms.Config.Files.AllowedFileTypes.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToLower().Trim());
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the JAVAScript function to call after saving a file is done.
        /// </summary>
        public string CallbackFunctionName { get; set; }

        /// <summary>
        /// Gets or sets the custom height to create an extra "_custom" version of the file for.
        /// </summary>
        public int CustomHeight { get; set; }

        /// <summary>
        /// Gets or sets the custom width to create an extra "_custom" version of the file for.
        /// </summary>
        public int CustomWidth { get; set; }

        /// <summary>
        /// Gets or sets the virtual path to the default file to show when no file was uploaded yet.
        /// </summary>
        public string DefaultImage { get; set; }

        /// <summary>
        /// Gets or sets the height for displaying the file after uploading.
        /// </summary>
        public int DisplayHeight { get; set; }

        /// <summary>
        /// Gets or sets the width for displaying the file after uploading.
        /// </summary>
        public int DisplayWidth { get; set; }

        /// <summary>
        /// Gets or sets the document type of the file.
        /// </summary>
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// Gets or sets the name of the property to store the file id in on the entity the file is
        /// linked to.
        /// </summary>
        public string FileIdPropertyName { get; set; }

        /// <summary>
        /// Gets or sets the file types allowed (comma-separated), narrowing the file types
        /// specified in the platform.
        /// </summary>
        public string FileTypes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to keep the aspect ratio for an image when resizing.
        /// </summary>
        public bool KeepAspectRatio { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed file size in bytes.
        /// </summary>
        public int MaxFileSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum height for an image file. Larger files will be resized.
        /// </summary>
        public int MaxHeight { get; set; }

        /// <summary>
        /// Gets or sets the maximum width for an image file. Larger files will be resized.
        /// </summary>
        public int MaxWidth { get; set; }

        /// <summary>
        /// Gets or sets the mode to use for uploading.
        /// </summary>
        public FileUploadMode Mode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the uploader title should be shown.
        /// </summary>
        public bool ShowTitle { get; set; }

        /// <summary>
        /// Gets or sets the terms this file should be tagged with when created.
        /// </summary>
        public Dictionary<string, string> Terms { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the uploaded file should be unzipped.
        /// </summary>
        public bool Unzip { get; set; }

        #endregion Public Properties
    }
}
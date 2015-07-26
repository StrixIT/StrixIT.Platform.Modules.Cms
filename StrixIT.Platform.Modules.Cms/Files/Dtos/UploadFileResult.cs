#region Apache License

//-----------------------------------------------------------------------
// <copyright file="UploadFileResult.cs" company="StrixIT">
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

using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A view model to show the results of a file upload.
    /// </summary>
    public class UploadFileResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the document type.
        /// </summary>
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the id of the file created.
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// Gets or sets the image as a Base64 string for image uploads.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the upload result message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the upload was successful.
        /// </summary>
        public bool Succeeded { get; set; }

        #endregion Public Properties
    }
}
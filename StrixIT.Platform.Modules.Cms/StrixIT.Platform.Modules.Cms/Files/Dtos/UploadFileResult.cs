//-----------------------------------------------------------------------
// <copyright file="UploadFileResult.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A view model to show the results of a file upload.
    /// </summary>
    public class UploadFileResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the upload was successful.
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Gets or sets the upload result message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the id of the file created.
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// Gets or sets the image as a Base64 string for image uploads.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets the document type.
        /// </summary>
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        public string Extension { get; set; }
    }
}
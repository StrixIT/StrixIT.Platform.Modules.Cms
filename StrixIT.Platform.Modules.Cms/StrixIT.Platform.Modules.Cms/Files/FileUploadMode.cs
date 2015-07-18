//-----------------------------------------------------------------------
// <copyright file="FileUploadMode.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The modes available for uploading files.
    /// </summary>
    public enum FileUploadMode
    {
        /// <summary>
        /// Uploads a file and displays the result.
        /// </summary>
        UploadAndShow,

        /// <summary>
        /// Uploads a single file and shows the uploader again after uploading.
        /// </summary>
        SingleUpload,

        /// <summary>
        /// Uploads a single zip-file which will be extracted. Shows the uploader again when done uploading.
        /// </summary>
        MultiUpload
    }
}
//-----------------------------------------------------------------------
// <copyright file="IFileService.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Web;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface for the file service used by the Cms.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Uploads a file.
        /// </summary>
        /// <param name="request">The request</param>
        /// <returns>The upload result</returns>
        UploadFileResult UploadFile(HttpRequestBase request);

        /// <summary>
        /// Uploads multiple files at once
        /// </summary>
        /// <param name="model">The model containing the file upload data</param>
        /// <param name="request">The request</param>
        /// <returns>A list of file upload results</returns>
        IList<UploadFileResult> UploadFiles(AddFile model, HttpRequestBase request);

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="fileId">The id of the file</param>
        /// <returns>The result as an upload result</returns>
        UploadFileResult DeleteFile(Guid fileId);

        /// <summary>
        /// Check whether a user can access a file.
        /// </summary>
        /// <param name="url">The file url</param>
        /// <returns>True of the file can be accessed, false otherwise</returns>
        bool CheckAccessFile(string url);
    }
}
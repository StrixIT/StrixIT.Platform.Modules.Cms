#region Apache License
//-----------------------------------------------------------------------
// <copyright file="IFileService.cs" company="StrixIT">
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
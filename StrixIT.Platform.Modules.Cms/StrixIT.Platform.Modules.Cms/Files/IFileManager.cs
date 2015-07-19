#region Apache License
//-----------------------------------------------------------------------
// <copyright file="IFileManager.cs" company="StrixIT">
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
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// An interface for the file manager, which handles saving and deleting files.
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// Gets a value indicating whether uploading the file name supplied is allowed.
        /// </summary>
        /// <param name="name">The name of the file to check</param>
        /// <returns>True if the file is valid for uploading, false otherwise</returns>
        bool IsFileAllowed(string name);

        /// <summary>
        /// Check whether a user can access a file.
        /// </summary>
        /// <param name="url">The file url</param>
        /// <returns>True of the file can be accessed, false otherwise</returns>
        bool CheckAccessFile(string url);

        /// <summary>
        /// Gets a value indicating whether uploading the file name supplied is allowed.
        /// </summary>
        /// <param name="name">The name of the file to check</param>
        /// <param name="additionalAllowedExtensions">The additonally allowed extensions</param>
        /// <returns>True if the file is valid for uploading, false otherwise</returns>
        bool IsFileAllowed(string name, string[] additionalAllowedExtensions);

        /// <summary>
        /// Gets the extensions for a document type (image, video, audio or document)
        /// </summary>
        /// <param name="type">The type to get the extensions for</param>
        /// <returns>The extensions</returns>
        string[] GetExtensions(string type);

        /// <summary>
        /// Checks whether the file extension to see whether the file is an image
        /// </summary>
        /// <param name="filePath">The file path to check</param>
        /// <returns>True if the file is an image, false otherwise</returns>
        bool IsImage(string filePath);

        /// <summary>
        /// Checks whether the file extension to see whether the file is a video
        /// </summary>
        /// <param name="filePath">The file path to check</param>
        /// <returns>True if the file is a video, false otherwise</returns>
        bool IsVideo(string filePath);

        /// <summary>
        /// Checks whether the file extension to see whether the file is an audio file
        /// </summary>
        /// <param name="filePath">The file path to check</param>
        /// <returns>True if the file is an audio file, false otherwise</returns>
        bool IsAudio(string filePath);

        /// <summary>
        /// Checks whether the file extension to see whether the file is a document
        /// </summary>
        /// <param name="filePath">The file path to check</param>
        /// <returns>True if the file is a document, false otherwise</returns>
        bool IsDocument(string filePath);

        /// <summary>
        /// Gets the document type based on an extension.
        /// </summary>
        /// <param name="extension">The extension</param>
        /// <returns>The document type</returns>
        DocumentType GetDocumentType(string extension);

        /// <summary>
        /// Gets a file by its id.
        /// </summary>
        /// <param name="id">The id of the file to get</param>
        /// <returns>The file</returns>
        File Get(Guid id);

        /// <summary>
        /// Saves a file.
        /// </summary>
        /// <param name="arguments">The arguments to save the file</param>
        /// <returns>A new File entity for the uploaded file</returns>
        File Save(SaveFileArguments arguments);

        /// <summary>
        /// Saves a list of files.
        /// </summary>
        /// <param name="arguments">The arguments to save the file</param>
        /// <returns>A list of new File entities for the uploaded files</returns>
        IList<File> SaveMany(SaveFileArguments arguments);

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="file">The file to delete</param>
        void Delete(File file);

        /// <summary>
        /// Deletes a file by its id.
        /// </summary>
        /// <param name="fileId">The id of the file to delete</param>
        void Delete(Guid fileId);
    }
}
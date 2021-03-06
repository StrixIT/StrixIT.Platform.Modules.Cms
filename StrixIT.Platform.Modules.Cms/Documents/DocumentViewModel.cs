﻿#region Apache License

//-----------------------------------------------------------------------
// <copyright file="DocumentViewModel.cs" company="StrixIT">
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

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The view model for documents.
    /// </summary>
    public class DocumentViewModel : EntityViewModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the author of the document.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets the Id of the user of the author of the document.
        /// </summary>
        public Guid? AuthorUserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time the document was created.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the document description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the document type.
        /// </summary>
        public DocumentType DocumentType { get; set; }

        /// <summary>
        /// Gets or sets the number of times this document has been downloaded.
        /// </summary>
        public int DownloadCount { get; set; }

        /// <summary>
        /// Gets the extension of the document file.
        /// </summary>
        public string Extension
        {
            get
            {
                if (this.File != null)
                {
                    return this.File.Extension;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the document file.
        /// </summary>
        [Image(100, 100)]
        public FileDisplayModel File { get; set; }

        /// <summary>
        /// Gets or sets the document file id.
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// Gets or sets the path to the file, when the document type is image.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets the full path to the document file.
        /// </summary>
        public string FullPath
        {
            get
            {
                if (this.File != null)
                {
                    return this.File.FullPath;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the location the document was created (used for media, a.o.).
        /// </summary>
        public string Location { get; set; }

        #endregion Public Properties
    }
}
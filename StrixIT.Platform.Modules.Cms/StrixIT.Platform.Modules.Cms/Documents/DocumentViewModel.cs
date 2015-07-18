//-----------------------------------------------------------------------
// <copyright file="DocumentViewModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.ComponentModel.DataAnnotations;
using StrixIT.Platform.Core;
using StrixIT.Platform.Modules.Cms;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The view model for documents.
    /// </summary>
    public class DocumentViewModel : EntityViewModel
    {
        /// <summary>
        /// Gets or sets the document file id.
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// Gets or sets the document file.
        /// </summary>
        [Image(100, 100)]
        public FileDisplayModel File { get; set; }

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
        /// Gets or sets the document description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the author of the document.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets the Id of the user of the author of the document.
        /// </summary>
        public Guid? AuthorUserId { get; set; }

        /// <summary>
        /// Gets or sets the location the document was created (used for media, a.o.).
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the date and time the document was created.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the document type.
        /// </summary>
        public DocumentType DocumentType { get; set; }

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
        /// Gets or sets the number of times this document has been downloaded.
        /// </summary>
        public int DownloadCount { get; set; }
    }
}

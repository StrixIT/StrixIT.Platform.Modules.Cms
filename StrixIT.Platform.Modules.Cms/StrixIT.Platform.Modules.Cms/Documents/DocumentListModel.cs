//-----------------------------------------------------------------------
// <copyright file="DocumentListModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Newtonsoft.Json;
using StrixIT.Platform.Core;
using StrixIT.Platform.Modules.Cms;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The list model for displaying a list of Html.
    /// </summary>
    public class DocumentListModel : EntityListModel
    {
        /// <summary>
        /// Gets or sets the file model for the document.
        /// </summary>
        [JsonIgnore]
        [Image(100, 100)]
        public FileDisplayModel File { get; set; }

        /// <summary>
        /// Gets or sets the path to the file, when the document type is image.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets the file extension for the document.
        /// </summary>
        public string Extension
        {
            get
            {
                return this.File != null ? this.File.Extension : null;
            }
        }

        /// <summary>
        /// Gets the size of the file for the document.
        /// </summary>
        public long? FileSize
        {
            get
            {
                return this.File != null ? this.File.Size : null;
            }
        }

        /// <summary>
        /// Gets or sets the name of the author of the File.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets the location the File was created (used for media, a.o.).
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the document description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the document type.
        /// </summary>
        public DocumentType DocumentType { get; set; }
    }
}
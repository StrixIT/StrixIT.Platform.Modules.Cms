//-----------------------------------------------------------------------
// <copyright file="Document.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The class for all file content within the platform.
    /// </summary>
    public class Document : ContentBase, IContent
    {
        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        [StrixRequired]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        public PlatformEntity Entity { get; set; }

        /// <summary>
        /// Gets or sets the id of the file for this document.
        /// </summary>
        [StrixRequired]
        public Guid? FileId { get; set; }

        /// <summary>
        /// Gets or sets the file for this document.
        /// </summary>
        [FileUpload]
        public File File { get; set; }

        /// <summary>
        /// Gets or sets the document description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the author of the File.
        /// </summary>
        [StringLength(250)]
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets the Id of the user of the author of the File.
        /// </summary>
        [StrixNotDefaultWithMembership]
        public Guid? AuthorUserId { get; set; }

        /// <summary>
        /// Gets or sets the user of the author of the file.
        /// </summary>
        public UserData AuthorUser { get; set; }

        /// <summary>
        /// Gets or sets the location the File was created (used for media, a.o.).
        /// </summary>
        [StringLength(250)]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the date and time the File was created.
        /// </summary>
        [StrixNotDefault]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the number of times this document has been downloaded.
        /// </summary>
        public int DownloadCount { get; set; }
    }
}
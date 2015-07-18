//-----------------------------------------------------------------------
// <copyright file="FileDisplayModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The view model used to display files.
    /// </summary>
    public class FileDisplayModel
    {
        /// <summary>
        /// Gets or sets the file upload folder virtual path.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the file id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the file when it was uploaded.
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// Gets or sets the file size.
        /// </summary>
        public long? Size { get; set; }

        /// <summary>
        /// Gets or sets the date and time this file was uploaded.
        /// </summary>
        public DateTime UploadedOn { get; set; }

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Gets the file full path.
        /// </summary>
        public string FullPath
        {
            get
            {
                return string.Format("{0}/{1}/{2}.{3}", this.Folder, this.Path.Replace("\\", "/"), this.FileName, this.Extension);
            }
        }

        /// <summary>
        /// Gets or sets the file display height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the file display width.
        /// </summary>
        public int Width { get; set; }
    }
}
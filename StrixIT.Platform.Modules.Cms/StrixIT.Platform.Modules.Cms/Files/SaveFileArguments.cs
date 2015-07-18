//-----------------------------------------------------------------------
// <copyright file="SaveFileArguments.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A helper class for saving files.
    /// </summary>
    public class SaveFileArguments
    {
        /// <summary>
        /// Gets or sets the name of the file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets a byte array containing the file data
        /// </summary>
        public byte[] FileData { get; set; }

        /// <summary>
        /// Gets or sets the maximum width the file can have. If the uploaded file is larger, it will be resized
        /// </summary>
        public int? MaxX { get; set; }

        /// <summary>
        /// Gets or sets the maximum height the file can have. If the uploaded file is larger, it will be resized
        /// </summary>
        public int? MaxY { get; set; }

        /// <summary>
        /// Gets or sets the custom width to resize the file to, if any
        /// </summary>
        public int? CustomX { get; set; }

        /// <summary>
        /// Gets or sets the custom height to resize the file to, if any
        /// </summary>
        public int? CustomY { get; set; }
    }
}
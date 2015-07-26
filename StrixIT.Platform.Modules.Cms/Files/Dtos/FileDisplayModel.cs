#region Apache License

//-----------------------------------------------------------------------
// <copyright file="FileDisplayModel.cs" company="StrixIT">
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

using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The view model used to display files.
    /// </summary>
    public class FileDisplayModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file upload folder virtual path.
        /// </summary>
        public string Folder { get; set; }

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
        /// Gets or sets the file id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the file when it was uploaded.
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the file size.
        /// </summary>
        public long? Size { get; set; }

        /// <summary>
        /// Gets or sets the date and time this file was uploaded.
        /// </summary>
        public DateTime UploadedOn { get; set; }

        /// <summary>
        /// Gets or sets the file display width.
        /// </summary>
        public int Width { get; set; }

        #endregion Public Properties
    }
}
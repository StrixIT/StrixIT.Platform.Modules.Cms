#region Apache License

//-----------------------------------------------------------------------
// <copyright file="DocumentListModel.cs" company="StrixIT">
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

using Newtonsoft.Json;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The list model for displaying a list of Html.
    /// </summary>
    public class DocumentListModel : EntityListModel
    {
        #region Public Constructors

        public DocumentListModel() : base(typeof(Document))
        {
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the author of the File.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets the document description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the document type.
        /// </summary>
        public DocumentType DocumentType { get; set; }

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
        /// Gets or sets the location the File was created (used for media, a.o.).
        /// </summary>
        public string Location { get; set; }

        #endregion Public Properties
    }
}
#region Apache License

//-----------------------------------------------------------------------
// <copyright file="CmsConfiguration.cs" company="StrixIT">
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
using System.Collections.Generic;

namespace StrixIT.Platform.Modules.Cms
{
    public class CmsConfiguration
    {
        #region Private Fields

        private static bool? _usesSqlCompactDatabase = null;

        #endregion Private Fields

        #region Public Constructors

        public CmsConfiguration()
        {
            if (!_usesSqlCompactDatabase.HasValue && DependencyInjector.Injector != null)
            {
                var connection = DependencyInjector.Get<IConfiguration>().GetConnectionString("Cms");
                _usesSqlCompactDatabase = connection.Contains("|DataDirectory|");
            }
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the file types allowed in the application as a comma separated string.
        /// </summary>
        public string AllowedFileTypes { get; set; }

        /// <summary>
        /// Gets the extensions that should be treated as audio files as a comma separated string.
        /// </summary>
        public string AudioExtensions { get; set; }

        /// <summary>
        /// Gets the extensions that should be treated as document files as a comma separated string.
        /// </summary>
        public string DocumentExtensions { get; set; }

        /// <summary>
        /// Gets the extensions that should be treated as image files as a comma separated string.
        /// </summary>
        public string ImageExtensions { get; set; }

        /// <summary>
        /// Gets a value indicating whether files uploaded to the system should be secured, i.e. not
        /// accessible to unauthenticated users and users that do not belong to the group the file
        /// was uploaded for.
        /// </summary>
        public bool SecureFiles { get; set; }

        /// <summary>
        /// Gets the thumb directory for the application, relative to the root.
        /// </summary>
        public string ThumbDirectory { get; set; }

        /// <summary>
        /// Gets the folder to upload files to when storing them on the file system, relative to the
        /// site root.
        /// </summary>
        public string UploadFolder { get; set; }

        /// <summary>
        /// Gets a value indicating whether a summary can be added to a news item. If false,
        /// summaries are generated.
        /// </summary>
        public bool UseNewsSummary { get; set; }

        /// <summary>
        /// Gets a value indicating whether the CMS database is a SQL Compact database.
        /// </summary>
        public bool UsesSqlCompactDatabase
        {
            get
            {
                return _usesSqlCompactDatabase.Value;
            }
        }

        /// <summary>
        /// Gets the extensions that should be treated as video files as a comma separated string.
        /// </summary>
        public string VideoExtensions { get; set; }

        /// <summary>
        /// Gets the path to the water mark image, relative to the root.
        /// </summary>
        public string WaterMarkPath { get; set; }

        #endregion Public Properties
    }
}
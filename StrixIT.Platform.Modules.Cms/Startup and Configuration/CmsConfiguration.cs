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
#endregion

using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class CmsConfiguration
    {
        private static IDictionary<string, string> _settings = ModuleManager.AppSettings.ContainsKey(CmsConstants.CMS) ? ModuleManager.AppSettings[CmsConstants.CMS] : ModuleManager.AppSettings[PlatformConstants.PLATFORM];
        private static FilesConfiguration _files = new FilesConfiguration();
        private static bool? _usesSqlCompactDatabase = null;

        /// <summary>
        /// Gets a value indicating whether a summary can be added to a news item. If false, summaries are generated.
        /// </summary>
        public bool UseNewsSummary
        {
            get
            {
                return bool.Parse(_settings["useNewsSummary"]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the CMS database is a SQL Compact database.
        /// </summary>
        public bool UsesSqlCompactDatabase
        {
            get
            {
                if (!_usesSqlCompactDatabase.HasValue)
                {
                    var connection = ModuleManager.ConnectionStrings["Cms"].ConnectionString;
                    _usesSqlCompactDatabase = connection.Contains("|DataDirectory|");
                }

                return _usesSqlCompactDatabase.Value;
            }
        }

        /// <summary>
        /// Gets the Files configuration section.
        /// </summary>
        public FilesConfiguration Files
        {
            get
            {
                return _files;
            }
        }
    }
}
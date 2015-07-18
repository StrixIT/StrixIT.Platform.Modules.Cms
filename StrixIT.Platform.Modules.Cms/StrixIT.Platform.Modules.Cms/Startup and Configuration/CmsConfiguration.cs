//-----------------------------------------------------------------------
// <copyright file="CmsConfiguration.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//---------------------------------------------------------------------
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
//-----------------------------------------------------------------------
// <copyright file="FilesConfiguration.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//---------------------------------------------------------------------
using System.Configuration;
using System.Collections.Generic;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class FilesConfiguration
    {
        private static IDictionary<string, string> _settings = ModuleManager.AppSettings.ContainsKey(CmsConstants.CMS) ? ModuleManager.AppSettings[CmsConstants.CMS] : ModuleManager.AppSettings[PlatformConstants.PLATFORM];

        /// <summary>
        /// Gets a value indicating whether files uploaded to the system should be secured, i.e. not accessible to unauthenticated users
        /// and users that do not belong to the group the file was uploaded for.
        /// </summary>
        public bool SecureFiles
        {
            get
            {
                return bool.Parse(_settings["secureFiles"]);
            }
        }

        /// <summary>
        /// Gets the folder to upload files to when storing them on the file system, relative to the site root.
        /// </summary>
        public string UploadFolder
        {
            get
            {
                return _settings["uploadFolder"];
            }
        }

        /// <summary>
        /// Gets the thumb directory for the application, relative to the root.
        /// </summary>
        public string ThumbDirectory
        {
            get
            {
                return _settings["thumbDirectory"];
            }
        }

        /// <summary>
        /// Gets the path to the water mark image, relative to the root.
        /// </summary>
        public string WaterMarkPath
        {
            get
            {
                return _settings["waterMarkPath"];
            }
        }

        /// <summary>
        /// Gets the file types allowed in the application as a comma separated string.
        /// </summary>
        [ConfigurationProperty("allowedFileTypes", IsRequired = true)]
        public string AllowedFileTypes
        {
            get
            {
                return _settings["allowedFileTypes"];
            }
        }

        /// <summary>
        /// Gets the extensions that should be treated as image files as a comma separated string.
        /// </summary>
        public string ImageExtensions
        {
            get
            {
                return _settings["imageExtensions"];
            }
        }

        /// <summary>
        /// Gets the extensions that should be treated as video files as a comma separated string.
        /// </summary>
        public string VideoExtensions
        {
            get
            {
                return _settings["videoExtensions"];
            }
        }

        /// <summary>
        /// Gets the extensions that should be treated as audio files as a comma separated string.
        /// </summary>
        public string AudioExtensions
        {
            get
            {
                return _settings["audioExtensions"];
            }
        }

        /// <summary>
        /// Gets the extensions that should be treated as document files as a comma separated string.
        /// </summary>
        public string DocumentExtensions
        {
            get
            {
                return _settings["documentExtensions"];
            }
        }
    }
}
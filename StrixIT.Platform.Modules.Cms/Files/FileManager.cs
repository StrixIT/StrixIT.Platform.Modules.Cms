#region Apache License

//-----------------------------------------------------------------------
// <copyright file="FileManager.cs" company="StrixIT">
// Copyright 2015 StrixIT. Author R.G. Schurgers MA MSc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use file except in compliance with the License.
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
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace StrixIT.Platform.Modules.Cms
{
    public class FileManager : IFileManager
    {
        #region Private Fields

        private static string[] _allowedExtensions;
        private static string _uploadFolder;
        private IPlatformDataSource _dataSource;
        private IEnvironment _environment;
        private IImageConverter _imageConverter;

        #endregion Private Fields

        #region Public Constructors

        public FileManager(IPlatformDataSource dataSource, IImageConverter imageConverter, IEnvironment environment)
        {
            _dataSource = dataSource;
            _imageConverter = imageConverter;
            _environment = environment;

            if (_allowedExtensions == null)
            {
                var cmsConfig = _environment.Configuration.GetConfiguration<CmsConfiguration>();
                _allowedExtensions = cmsConfig.AllowedFileTypes.ToLower().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Trim().ToArray();
                _uploadFolder = cmsConfig.UploadFolder;
            }
        }

        #endregion Public Constructors

        #region Private Properties

        private static string CurrentStorageYearMonth
        {
            get
            {
                DateTime currentDate = DateTime.Now;
                return string.Format("{0}/{1}", currentDate.Year.ToString(), currentDate.Month.ToString());
            }
        }

        #endregion Private Properties

        #region Public Methods

        public bool CheckAccessFile(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }

            url = url.ToLower();

            if (!_environment.User.IsInMainGroup && url.Contains(string.Format("{0}/{1}/", CmsConstants.SECURE.ToLower(), _environment.User.GroupName.ToLower())))
            {
                return true;
            }

            var secureFiles = _environment.Configuration.GetConfiguration<CmsConfiguration>().SecureFiles;
            var directoryIsSecure = url.Contains(string.Format("/{0}/", CmsConstants.SECURE.ToLower()));
            var isAuthenticated = !string.IsNullOrWhiteSpace(_environment.User.Name);

            if (secureFiles || directoryIsSecure)
            {
                return isAuthenticated;
            }

            return true;
        }

        public void Delete(Guid fileId)
        {
            if (fileId == Guid.Empty)
            {
                throw new ArgumentNullException("fileId");
            }

            var file = _dataSource.Query<File>().FirstOrDefault(f => f.Id == fileId);

            if (file != null)
            {
                Delete(file);
            }
        }

        public void Delete(File file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            if (file.GroupId == _environment.User.GroupId)
            {
                try
                {
                    _dataSource.Delete(file);
                    string directory = System.IO.Path.Combine(_environment.MapPath(file.Folder), file.Path);
                    _dataSource.FileSystem.DeleteFile(string.Format("{0}.{1}", Path.Combine(directory, file.FileName), file.Extension));
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, ex, LogLevel.Fatal);
                    throw;
                }
            }
        }

        public File Get(Guid id)
        {
            return _dataSource.Query<File>().FirstOrDefault(f => f.Id == id);
        }

        public DocumentType GetDocumentType(string extension)
        {
            return IsImage(extension) ? DocumentType.Image :
                        IsVideo(extension) ? DocumentType.Video :
                        IsAudio(extension) ? DocumentType.Audio :
                        IsDocument(extension) ? DocumentType.Document :
                        DocumentType.Unknown;
        }

        public string[] GetExtensions(string type)
        {
            return GetExtensionsForFileType(type).Split(',').Trim().ToLower().ToArray();
        }

        public bool IsAudio(string filePath)
        {
            return IsType(filePath, "audio");
        }

        public bool IsDocument(string filePath)
        {
            return IsType(filePath, "document");
        }

        public bool IsFileAllowed(string name)
        {
            return IsFileAllowed(name, null);
        }

        public bool IsFileAllowed(string name, string[] additionalAllowedExtensions)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            var allowedExtensions = additionalAllowedExtensions != null ? _allowedExtensions.Union(additionalAllowedExtensions).ToArray() : _allowedExtensions;

            string extension = System.IO.Path.GetExtension(name).Replace(".", string.Empty).ToLower();

            // must be in the whitelist
            if (!allowedExtensions.Contains(extension))
            {
                return false;
            }

            // blacklist always applies
            if (name.Trim().Equals("web.config", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        public bool IsImage(string filePath)
        {
            return IsType(filePath, "image");
        }

        public bool IsVideo(string filePath)
        {
            return IsType(filePath, "video");
        }

        public File Save(SaveFileArguments arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            if (arguments.FileData == null)
            {
                Logger.Log("No fileData passed to FileRepository.SaveFile", LogLevel.Error);
                return null;
            }

            if (IsFileAllowed(arguments.FileName, new string[] { "zip" }) && arguments.FileData.Length > 0)
            {
                string extension = System.IO.Path.GetExtension(arguments.FileName).Replace(".", string.Empty);
                var isInMainGroup = _environment.User.IsInMainGroup;
                File newFile = new File();
                newFile.Path = !isInMainGroup || _environment.Configuration.GetConfiguration<CmsConfiguration>().SecureFiles ? string.Format("{0}\\", CmsConstants.SECURE) : null;

                if (!isInMainGroup)
                {
                    newFile.Path = newFile.Path + _environment.User.GroupName;
                }

                newFile.Id = Guid.NewGuid();
                newFile.OriginalName = arguments.FileName;
                newFile.FileName = newFile.Id.ToString();
                newFile.Path = newFile.Path + CurrentStorageYearMonth.Replace("/", "\\");
                newFile.Extension = extension;
                newFile.Size = arguments.FileData.LongLength;
                File createdFile = null;
                string uploadDirectory = _environment.MapPath(_uploadFolder);

                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                string directory = System.IO.Path.Combine(uploadDirectory, newFile.Path);

                try
                {
                    var path = string.Format("{0}.{1}", Path.Combine(directory, newFile.FileName), newFile.Extension);
                    _dataSource.FileSystem.SaveFile(path, arguments.FileData);

                    if (IsImage(newFile.Extension))
                    {
                        if (arguments.MaxX.HasValue && arguments.MaxX.Value > 0 && arguments.MaxY.HasValue && arguments.MaxY.Value > 0)
                        {
                            _imageConverter.Resize(path, arguments.MaxX.Value, arguments.MaxY.Value, true);
                        }

                        if (arguments.CustomX.HasValue && arguments.CustomX.Value > 0 && arguments.CustomY.HasValue && arguments.CustomY.Value > 0)
                        {
                            _imageConverter.Resize(path, arguments.CustomX.Value, arguments.CustomY.Value, false, false);
                        }
                    }

                    createdFile = CreateFile(newFile);
                }
                catch (Exception ex)
                {
                    _dataSource.FileSystem.DeleteFile(string.Format("{0}.{1}", Path.Combine(directory, newFile.FileName), newFile.Extension));
                    _dataSource.FileSystem.ProcessDeleteQueue();
                    Logger.Log(ex.Message, ex, LogLevel.Fatal);
                    throw;
                }

                return createdFile;
            }

            return null;
        }

        public IList<File> SaveMany(SaveFileArguments arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
            }

            if (arguments.FileData == null)
            {
                Logger.Log("No fileData passed to FileRepository.SaveFiles", LogLevel.Error);
                return null;
            }

            if (arguments.FileData.Length == 0)
            {
                return null;
            }

            IList<File> files = new List<File>();

            try
            {
                using (var memoryStream = new System.IO.MemoryStream(arguments.FileData))
                {
                    var archive = new ZipArchive(memoryStream);

                    foreach (var entry in archive.Entries)
                    {
                        if (entry.Name.Length > 0)
                        {
                            byte[] data = new byte[entry.Length];
                            System.IO.Stream stream = entry.Open();
                            stream.Read(data, 0, (int)entry.Length);

                            File file = Save(new SaveFileArguments
                            {
                                FileName = entry.Name,
                                FileData = data,
                                MaxX = arguments.MaxX,
                                MaxY = arguments.MaxY,
                                CustomX = arguments.CustomX,
                                CustomY = arguments.CustomY
                            });
                            files.Add(file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                foreach (File file in files)
                {
                    string directory = System.IO.Path.Combine(_environment.MapPath(_uploadFolder), file.Path);
                    _dataSource.FileSystem.DeleteFile(string.Format("{0}.{1}", Path.Combine(directory, file.FileName), file.Extension));
                }

                _dataSource.FileSystem.ProcessDeleteQueue();
                Logger.Log(ex.Message, ex, LogLevel.Fatal);
                throw;
            }

            return files;
        }

        #endregion Public Methods

        #region Private Methods

        private File CreateFile(File file)
        {
            file.Folder = _uploadFolder;
            file.GroupId = _environment.User.GroupId;
            file.UploadedOn = DateTime.Now;
            file.UploadedByUserId = _environment.User.Id;
            _dataSource.Save(file);
            return file;
        }

        private string GetExtensionsForFileType(string fileType)
        {
            string typeExtensions = string.Empty;
            var config = _environment.Configuration.GetConfiguration<CmsConfiguration>();

            switch (fileType.ToLower())
            {
                case "image":
                    {
                        typeExtensions = config.ImageExtensions;
                    }
                    break;

                case "video":
                    {
                        typeExtensions = config.VideoExtensions;
                    }
                    break;

                case "audio":
                    {
                        typeExtensions = config.AudioExtensions;
                    }
                    break;

                case "document":
                    {
                        typeExtensions = config.DocumentExtensions;
                    }
                    break;
            }

            return typeExtensions;
        }

        private bool IsType(string filePath, string type)
        {
            string extension = filePath.Split('.').Last();

            if (extension != null)
            {
                string typeExtensions = GetExtensionsForFileType(type);
                return typeExtensions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Trim().ToLower().Any(e => e.ToLower() == extension.ToLower());
            }

            return false;
        }

        #endregion Private Methods
    }
}
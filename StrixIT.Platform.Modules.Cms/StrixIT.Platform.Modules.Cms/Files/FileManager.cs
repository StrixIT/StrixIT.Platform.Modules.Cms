//-----------------------------------------------------------------------
// <copyright file="FileManager.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class FileManager : IFileManager
    {
        private static string _uploadFolder = StrixCms.Config.Files.UploadFolder;
        private static string[] _allowedExtensions = StrixCms.Config.Files.AllowedFileTypes.ToLower().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Trim().ToArray();
        private IPlatformDataSource _dataSource;
        private IImageConverter _imageConverter;

        public FileManager(IPlatformDataSource dataSource, IImageConverter imageConverter)
        {
            this._dataSource = dataSource;
            this._imageConverter = imageConverter;
        }

        private static string CurrentStorageYearMonth
        {
            get
            {
                DateTime currentDate = DateTime.Now;
                return string.Format("{0}/{1}", currentDate.Year.ToString(), currentDate.Month.ToString());
            }
        }

        public string[] GetExtensions(string type)
        {
            return GetExtensionsForFileType(type).Split(',').Trim().ToLower().ToArray();
        }

        public bool IsImage(string filePath)
        {
            return IsType(filePath, "image");
        }

        public bool IsVideo(string filePath)
        {
            return IsType(filePath, "video");
        }

        public bool IsAudio(string filePath)
        {
            return IsType(filePath, "audio");
        }

        public bool IsDocument(string filePath)
        {
            return IsType(filePath, "document");
        }

        public DocumentType GetDocumentType(string extension)
        {
            return this.IsImage(extension) ? DocumentType.Image :
                        this.IsVideo(extension) ? DocumentType.Video :
                        this.IsAudio(extension) ? DocumentType.Audio :
                        this.IsDocument(extension) ? DocumentType.Document :
                        DocumentType.Unknown;
        }

        public bool CheckAccessFile(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }

            url = url.ToLower();

            if (!StrixPlatform.User.IsInMainGroup && url.Contains(string.Format("{0}/{1}/", CmsConstants.SECURE.ToLower(), StrixPlatform.User.GroupName.ToLower())))
            {
                return true;
            }

            var secureFiles = StrixCms.Config.Files.SecureFiles;
            var directoryIsSecure = url.Contains(string.Format("/{0}/", CmsConstants.SECURE.ToLower()));
            var isAuthenticated = !string.IsNullOrWhiteSpace(StrixPlatform.User.Name);

            if (secureFiles || directoryIsSecure)
            {
                return isAuthenticated;
            }

            return true;
        }

        public bool IsFileAllowed(string name)
        {
            return this.IsFileAllowed(name, null);
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

        public File Get(Guid id)
        {
            return this._dataSource.Query<File>().FirstOrDefault(f => f.Id == id);
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

            if (this.IsFileAllowed(arguments.FileName, new string[] { "zip" }) && arguments.FileData.Length > 0)
            {
                string extension = System.IO.Path.GetExtension(arguments.FileName).Replace(".", string.Empty);
                var isInMainGroup = StrixPlatform.User.IsInMainGroup;
                File newFile = new File();
                newFile.Path = !isInMainGroup || StrixCms.Config.Files.SecureFiles ? string.Format("{0}\\", CmsConstants.SECURE) : null;

                if (!isInMainGroup)
                {
                    newFile.Path = newFile.Path + StrixPlatform.User.GroupName;
                }

                newFile.Id = Guid.NewGuid();
                newFile.OriginalName = arguments.FileName;
                newFile.FileName = newFile.Id.ToString();
                newFile.Path = newFile.Path + CurrentStorageYearMonth.Replace("/", "\\");
                newFile.Extension = extension;
                newFile.Size = arguments.FileData.LongLength;
                File createdFile = null;
                string uploadDirectory = StrixPlatform.Environment.MapPath(_uploadFolder);

                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                string directory = System.IO.Path.Combine(uploadDirectory, newFile.Path);

                try
                {
                    var path = string.Format("{0}.{1}", Path.Combine(directory, newFile.FileName), newFile.Extension);
                    this._dataSource.FileSystemWrapper.SaveFile(path, arguments.FileData);

                    if (this.IsImage(newFile.Extension))
                    {
                        if (arguments.MaxX.HasValue && arguments.MaxX.Value > 0 && arguments.MaxY.HasValue && arguments.MaxY.Value > 0)
                        {
                            this._imageConverter.Resize(path, arguments.MaxX.Value, arguments.MaxY.Value, true);
                        }

                        if (arguments.CustomX.HasValue && arguments.CustomX.Value > 0 && arguments.CustomY.HasValue && arguments.CustomY.Value > 0)
                        {
                            this._imageConverter.Resize(path, arguments.CustomX.Value, arguments.CustomY.Value, false, false);
                        }
                    }

                    createdFile = this.CreateFile(newFile);
                }
                catch (Exception ex)
                {
                    this._dataSource.FileSystemWrapper.DeleteFile(string.Format("{0}.{1}", Path.Combine(directory, newFile.FileName), newFile.Extension));
                    this._dataSource.FileSystemWrapper.ProcessDeleteQueue();
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

                            File file = this.Save(new SaveFileArguments
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
                    string directory = System.IO.Path.Combine(StrixPlatform.Environment.MapPath(_uploadFolder), file.Path);
                    this._dataSource.FileSystemWrapper.DeleteFile(string.Format("{0}.{1}", Path.Combine(directory, file.FileName), file.Extension));
                }

                this._dataSource.FileSystemWrapper.ProcessDeleteQueue();
                Logger.Log(ex.Message, ex, LogLevel.Fatal);
                throw;
            }

            return files;
        }

        public void Delete(Guid fileId)
        {
            if (fileId == Guid.Empty)
            {
                throw new ArgumentNullException("fileId");
            }

            var file = this._dataSource.Query<File>().FirstOrDefault(f => f.Id == fileId);

            if (file != null)
            {
                this.Delete(file);
            }
        }

        public void Delete(File file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            if (file.GroupId == StrixPlatform.User.GroupId)
            {
                try
                {
                    this._dataSource.Delete(file);
                    string directory = System.IO.Path.Combine(StrixPlatform.Environment.MapPath(file.Folder), file.Path);
                    this._dataSource.FileSystemWrapper.DeleteFile(string.Format("{0}.{1}", Path.Combine(directory, file.FileName), file.Extension));
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, ex, LogLevel.Fatal);
                    throw;
                }
            }
        }

        private static bool IsType(string filePath, string type)
        {
            string extension = filePath.Split('.').Last();

            if (extension != null)
            {
                string typeExtensions = GetExtensionsForFileType(type);
                return typeExtensions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Trim().ToLower().Any(e => e.ToLower() == extension.ToLower());
            }

            return false;
        }

        private static string GetExtensionsForFileType(string fileType)
        {
            string typeExtensions = string.Empty;

            switch (fileType.ToLower())
            {
                case "image":
                    {
                        typeExtensions = StrixCms.Config.Files.ImageExtensions;
                    }
                    break;

                case "video":
                    {
                        typeExtensions = StrixCms.Config.Files.VideoExtensions;
                    }
                    break;

                case "audio":
                    {
                        typeExtensions = StrixCms.Config.Files.AudioExtensions;
                    }
                    break;

                case "document":
                    {
                        typeExtensions = StrixCms.Config.Files.DocumentExtensions;
                    }
                    break;
            }

            return typeExtensions;
        }

        private File CreateFile(File file)
        {
            file.Folder = _uploadFolder;
            file.GroupId = StrixPlatform.User.GroupId;
            file.UploadedOn = DateTime.Now;
            file.UploadedByUserId = StrixPlatform.User.Id;
            this._dataSource.Save(file);
            return file;
        }
    }
}
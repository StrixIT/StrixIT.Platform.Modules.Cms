//-----------------------------------------------------------------------
// <copyright file="FileService.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Web;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class FileService : IFileService
    {
        private IPlatformDataSource _dataSource;
        private IFileManager _fileManager;
        private IImageConverter _imageConverter;

        public FileService(IPlatformDataSource dataSource, IFileManager fileManager, IImageConverter imageConverter)
        {
            this._dataSource = dataSource;
            this._fileManager = fileManager;
            this._imageConverter = imageConverter;
        }

        public IList<UploadFileResult> UploadFiles(AddFile model, HttpRequestBase request)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var result = new List<UploadFileResult>();
            var allowed = true;

            // first validate them all
            foreach (string fileName in request.Files)
            {
                var file = request.Files[fileName];

                // Todo: test for zip.
                if (!this._fileManager.IsFileAllowed(file.FileName, model.Unzip ? new string[] { "zip" } : null))
                {
                    allowed = false;
                    break;
                }
            }

            if (allowed)
            {
                // then save them
                try
                {
                    foreach (string fileName in request.Files)
                    {
                        var file = request.Files[fileName];
                        System.IO.Stream fileStream = file.InputStream;
                        byte[] fileData = new byte[file.ContentLength];
                        fileStream.Read(fileData, 0, file.ContentLength);
                        var args = new SaveFileArguments { FileData = fileData, CustomX = model.CustomWidth, CustomY = model.CustomHeight, MaxX = model.MaxWidth, MaxY = model.MaxHeight };

                        if (model.Unzip && file.FileName.ToLower().EndsWith(".zip"))
                        {
                            var files = this._fileManager.SaveMany(args);

                            foreach (var theFile in files)
                            {
                                result.Add(this.GetUploadResult(theFile, model));
                            }
                        }
                        else
                        {
                            args.FileName = file.FileName;
                            var theFile = this._fileManager.Save(args);
                            result.Add(this.GetUploadResult(theFile, model));
                        }
                    }

                    this._dataSource.SaveChanges();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message, ex, LogLevel.Fatal);
                    throw;
                }
            }

            return result;
        }

        public UploadFileResult UploadFile(HttpRequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var result = new UploadFileResult();

            if (request.Files.Count < 1 || request.Files[0].ContentLength == 0)
            {
                result.Succeeded = false;
                result.Message = "<script type=\"text/javascript\">var result = {{ error: \"{0}\" }};</script>";
                result.Message = Resources.Interface.NoFileSelected;
            }

            try
            {
                var postedFile = request.Files[0];
                System.IO.Stream fileStream = postedFile.InputStream;
                byte[] fileData = new byte[postedFile.ContentLength];
                fileStream.Read(fileData, 0, postedFile.ContentLength);
                var args = new SaveFileArguments { FileData = fileData, FileName = postedFile.FileName };

                if (this._fileManager.IsFileAllowed(postedFile.FileName))
                {
                    var file = this._fileManager.Save(args);
                    this._dataSource.SaveChanges();
                    var path = file.Folder;
                    string filePath = string.Format("/{0}/{1}/{2}.{3}", path, file.Path.Replace("\\", "/"), file.FileName, file.Extension);
                    result.Succeeded = true;
                    result.Message = string.Format("<script type=\"text/javascript\">var result = {{ url: \"{0}\", name: \"{1}\" }};</script>", filePath, file.OriginalName);
                    result.DocumentType = this._fileManager.GetDocumentType(file.Extension);
                    result.Extension = file.Extension;
                    return result;
                }
                else
                {
                    result.Succeeded = false;
                    result.Message = "<script type=\"text/javascript\">var result = {{ error: \"{0}\"  }};</script>";
                    result.Message = Resources.Interface.FileTypeNotAllowed;
                }
            }
            catch (Exception)
            {
                result.Succeeded = false;
                result.Message = string.Format("<script type=\"text/javascript\">var result = {{ error: \"{0}\"  }};</script>", Resources.Interface.ErrorUploadingFile);
            }

            return result;
        }

        public UploadFileResult DeleteFile(Guid fileId)
        {
            if (fileId == Guid.Empty)
            {
                throw new ArgumentException("fileId cannot be empty", "fileId");
            }

            var result = new UploadFileResult();

            try
            {
                var file = this._fileManager.Get(fileId);

                if (file != null)
                {
                    this._fileManager.Delete(file);
                    this._dataSource.SaveChanges();
                    result.Succeeded = true;
                    result.Message = Resources.Interface.FileDeleteSuccess;
                }
            }
            catch (Exception ex)
            {
                this._dataSource.FileSystemWrapper.ClearDeleteQueue();
                Logger.Log(ex.Message, ex, LogLevel.Fatal);
                throw;
            }

            return result;
        }

        public bool CheckAccessFile(string url)
        {
            return this._fileManager.CheckAccessFile(url);
        }

        private UploadFileResult GetUploadResult(File uploadedFile, AddFile model)
        {
            var result = new UploadFileResult();

            if (uploadedFile != null)
            {
                result.Succeeded = true;
                result.FileId = uploadedFile.Id;
                int height = model.DisplayHeight > 0 ? model.DisplayHeight : model.MaxHeight;
                int width = model.DisplayWidth > 0 ? model.DisplayWidth : model.MaxWidth;
                var resultPath = string.Format("{0}.{1}", System.IO.Path.Combine(uploadedFile.Folder, uploadedFile.Path, uploadedFile.FileName), uploadedFile.Extension);
                result.Image = this._imageConverter.ImageAsBase64(resultPath, width, height, model.KeepAspectRatio);
                result.Message = Resources.Interface.FileUploadSuccess;
                result.DocumentType = this._fileManager.GetDocumentType(uploadedFile.Extension);
                result.Extension = uploadedFile.Extension;
            }
            else
            {
                result.Message = Resources.Interface.ErrorUploadingFile;
            }

            return result;
        }
    }
}
#region Apache License
//-----------------------------------------------------------------------
// <copyright file="FileController.cs" company="StrixIT">
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

using System.Linq;
using System.Web;
using System.Web.Mvc;
using System;
using System.Web.Helpers;
using StrixIT.Platform.Core;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    [StrixAuthorization(Roles = CmsRoleNames.CONTRIBUTORROLES)]
    public class FileController : BaseController
    {
        private IFileService _fileService;
        private IImageConverter _imageConverter;

        public FileController(IFileService fileService, IImageConverter imageConverter)
        {
            this._fileService = fileService;
            this._imageConverter = imageConverter;
        }

        [HttpPost]
        public JsonResult UploadFile(AddFile model)
        {
            if (model == null)
            {
                return null;
            }

            return this.Json(this._fileService.UploadFiles(model, this.Request));
        }

        [HttpPost]
        public ContentResult AddFromRTE()
        {
            return this.Content(this._fileService.UploadFile(Request).Message);
        }

        [HttpPost]
        public JsonResult DeleteFile(Guid fileId)
        {
            var result = this._fileService.DeleteFile(fileId);
            return this.Json(result);
        }

        [AllowAnonymous]
        [AuthenticatedCache(Duration = 86400)]
        public ActionResult GetFile(string type, string url)
        {
            return this.GetFileInternal(type, url);
        }

        [AllowAnonymous]
        [AuthenticatedCache(Duration = 86400)]
        public ActionResult GetImage(string type, int width, int height, string url)
        {
            return this.GetFileInternal(type, Web.Helpers.HtmlDecode(url, false), width, height);
        }

        private ActionResult GetFileInternal(string type, string url, int width = 0, int height = 0)
        {
            url = url.Replace("&#47;", "/");

            if (!this._fileService.CheckAccessFile(url))
            {
                this.Response.StatusCode = 401;
                return new HttpUnauthorizedResult();
            }

            string path = null;

            if (width > 0 && height > 0)
            {
                path = this._imageConverter.GetThumbPath(url, width, height);
                var pathParts = path.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                url = pathParts.Last();
                path = string.Format("/{0}/", Web.Helpers.GetVirtualPath(string.Join("\\", pathParts.Take(pathParts.Length - 1))));
            }
            else
            {
                path = this.Request.Url.Segments.Contains(string.Format("{0}/", CmsConstants.SECURE)) ? string.Format("~/Content/{0}/{1}/", type, CmsConstants.SECURE) : string.Format("~/Content/{0}/", type);
            }

            // Todo: logic te determine which images should be watermarked. Use an event?
            if (false)
            {
                var webImage = new WebImage(path);
                string waterMarkPath = string.Format("{0}{1}", Server.MapPath("~"), StrixCms.Config.Files.WaterMarkPath.Replace('/', '\\'));
                webImage.AddImageWatermark(waterMarkPath);
                return this.File(webImage.GetBytes(), MimeMapping.GetMimeMapping(url));
            }

            return this.File(this.Server.MapPath(path) + url, MimeMapping.GetMimeMapping(url));
        }
    }
}
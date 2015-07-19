#region Apache License
//-----------------------------------------------------------------------
// <copyright file="DocumentController.cs" company="StrixIT">
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

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;
using System.Web;
using StrixIT.Platform.Core;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    [StrixAuthorization(Roles = CmsRoleNames.CONTRIBUTORROLES)]
    public class DocumentController : EntityController<DocumentViewModel>
    {
        private ITaxonomyService _taxonomyService;

        public DocumentController(IDocumentService documentService, ICommentService commentService, ITaxonomyService taxonomyService)
            : base(documentService, commentService) 
        {
            this._taxonomyService = taxonomyService;
        }

        public override ActionResult Index()
        {
            var config = new EntityListConfiguration<DocumentViewModel>(StrixPlatform.User);
            config.Fields.Insert(0, new ListFieldConfiguration("Extension"));
            config.Fields.Insert(1, new ListFieldConfiguration("FileSize", "bytes") { ShowFilter = false });
            return this.View(config);
        }

        public ActionResult CreateMany()
        {
            ViewBag.ModelType = typeof(Document);
            return this.View("CreateMany");
        }

        [StrixAuthorization(Roles = CmsRoleNames.CONTRIBUTORROLES)]
        public JsonResult GetAllTags()
        {
            return this.Json(this._taxonomyService.GetAllTags().OrderBy(t => t.Name));
        }

        [HttpPost]
        public JsonResult CreateMany(IList<DocumentViewModel> models)
        {
            var result = ((IDocumentService)this.Service).CreateMany(models);
            return this.Json(result);
        }

        [AllowAnonymous]
        public ActionResult Download(string id)
        {
            var document = ((IDocumentService)this.Service).GetForDownload(id);

            if (document == null)
            {
                this.Response.StatusCode = 401;
                return new HttpUnauthorizedResult();
            }

            return new FilePathResult(StrixPlatform.Environment.MapPath(document.FullPath), MimeMapping.GetMimeMapping(document.FullPath));
        }
    }
}
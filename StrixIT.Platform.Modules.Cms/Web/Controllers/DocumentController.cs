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

#endregion Apache License

using StrixIT.Platform.Core;
using StrixIT.Platform.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StrixIT.Platform.Modules.Cms
{
    [StrixAuthorization(Roles = CmsRoleNames.CONTRIBUTORROLES)]
    public class DocumentController : EntityController<DocumentViewModel>
    {
        #region Public Constructors

        public DocumentController(IDocumentService documentService, ICmsServices cmsServices)
            : base(documentService, cmsServices)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public ActionResult CreateMany()
        {
            ViewBag.ModelType = typeof(Document);
            return this.View("CreateMany");
        }

        [HttpPost]
        public JsonResult CreateMany(IList<DocumentViewModel> models)
        {
            var result = ((IDocumentService)this.EntityService).CreateMany(models);
            return this.Json(result);
        }

        [AllowAnonymous]
        public ActionResult Download(string id)
        {
            var document = ((IDocumentService)this.EntityService).GetForDownload(id);

            if (document == null)
            {
                this.Response.StatusCode = 401;
                return new HttpUnauthorizedResult();
            }

            return new FilePathResult(Environment.MapPath(document.FullPath), MimeMapping.GetMimeMapping(document.FullPath));
        }

        [StrixAuthorization(Roles = CmsRoleNames.CONTRIBUTORROLES)]
        public JsonResult GetAllTags()
        {
            return this.Json(Services.TaxonomyService.GetAllTags().OrderBy(t => t.Name));
        }

        public override ActionResult Index()
        {
            var config = new EntityListConfiguration<DocumentViewModel>(Environment.User, Services.EntityHelper);
            config.Fields.Insert(0, new ListFieldConfiguration("Extension"));
            config.Fields.Insert(1, new ListFieldConfiguration("FileSize", "bytes") { ShowFilter = false });
            return this.View(config);
        }

        #endregion Public Methods
    }
}
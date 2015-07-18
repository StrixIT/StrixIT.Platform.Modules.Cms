//-----------------------------------------------------------------------
// <copyright file="VocabularyController.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Web.Mvc;
using StrixIT.Platform.Core;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    [StrixAuthorization(Roles = CmsRoleNames.EDITORROLES)]
    public class VocabularyController : BaseCrudController<Guid, VocabularyViewModel>
    {
        public VocabularyController(ITaxonomyService service) : base(service) { }

        private ITaxonomyService Service
        {
            get
            {
                return this._service as ITaxonomyService;
            }
        }

        public override ActionResult Index()
        {
            var config = new VocabularyListConfiguration(StrixPlatform.User);
            return this.View(config);
        }

        public override ActionResult Get(string id)
        {
            Guid key;

            if (Guid.TryParse(id, out key))
            {
                return base.Get(id);
            }
            else
            {
                return this.Json(this.Service.GetVocabulary(id));
            }
        }

        [HttpPost]
        public JsonResult GetTags(Guid vocabularyId)
        {
            var list = this.Service.GetTagList(null, vocabularyId);
            return this.Json(list);
        }

        [HttpPost]
        public JsonResult SaveTag(TermViewModel model)
        {
            var result = this.Service.SaveTag(model);
            return this.Json(result.Success);
        }
    }
}
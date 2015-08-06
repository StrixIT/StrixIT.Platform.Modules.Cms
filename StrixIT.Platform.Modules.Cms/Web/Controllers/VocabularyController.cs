#region Apache License

//-----------------------------------------------------------------------
// <copyright file="VocabularyController.cs" company="StrixIT">
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
using System;
using System.Web.Mvc;

namespace StrixIT.Platform.Modules.Cms
{
    [StrixAuthorization(Roles = CmsRoleNames.EDITORROLES)]
    public class VocabularyController : BaseCrudController<Guid, VocabularyViewModel>
    {
        #region Private Fields

        private IUserContext _user;

        #endregion Private Fields

        #region Public Constructors

        public VocabularyController(ITaxonomyService service, IUserContext user) : base(service)
        {
            _user = user;
        }

        #endregion Public Constructors

        #region Private Properties

        private ITaxonomyService Service
        {
            get
            {
                return this._service as ITaxonomyService;
            }
        }

        #endregion Private Properties

        #region Public Methods

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

        public override ActionResult Index()
        {
            var config = new VocabularyListConfiguration(_user);
            return this.View(config);
        }

        [HttpPost]
        public JsonResult SaveTag(TermViewModel model)
        {
            var result = this.Service.SaveTag(model);
            return this.Json(result.Success);
        }

        #endregion Public Methods
    }
}
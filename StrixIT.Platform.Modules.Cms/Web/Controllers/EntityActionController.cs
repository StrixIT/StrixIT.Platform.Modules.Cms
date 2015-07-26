#region Apache License

//-----------------------------------------------------------------------
// <copyright file="EntityActionController.cs" company="StrixIT">
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
using System.Web.Mvc;

namespace StrixIT.Platform.Modules.Cms
{
    [StrixAuthorization(Roles = PlatformConstants.ADMINROLE)]
    public class EntityActionController : BaseController
    {
        #region Private Fields

        private IServiceManagerService _service;

        #endregion Private Fields

        #region Public Constructors

        public EntityActionController(IServiceManagerService managerService)
        {
            this._service = managerService;
        }

        #endregion Public Constructors

        #region Public Methods

        public JsonResult GetData()
        {
            return this.Json(this._service.GetServiceActionRecords());
        }

        public ActionResult Index()
        {
            return this.View(MvcConstants.INDEX);
        }

        [HttpPost]
        public JsonResult SaveData(EntityServiceCollection records)
        {
            JsonStatusResult status = new JsonStatusResult();

            if (records.IsEmpty())
            {
                status.Message = StrixIT.Platform.Modules.Cms.Resources.Interface.NoRecordsToSave;
                return this.Json(status);
            }

            status.Success = this._service.SaveActionRecords(records);

            if (!status.Success)
            {
                status.Message = StrixIT.Platform.Modules.Cms.Resources.Interface.ErrorSavingServiceAction;
            }

            return status;
        }

        #endregion Public Methods
    }
}
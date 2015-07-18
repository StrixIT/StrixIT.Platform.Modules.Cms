//-----------------------------------------------------------------------
// <copyright file="EntityActionController.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Web.Mvc;
using StrixIT.Platform.Core;
using StrixIT.Platform.Web;
using Newtonsoft.Json;

namespace StrixIT.Platform.Modules.Cms
{
    [StrixAuthorization(Roles = PlatformConstants.ADMINROLE)]
    public class EntityActionController : BaseController
    {
        private IServiceManagerService _service;

        public EntityActionController(IServiceManagerService managerService)
        {
            this._service = managerService;
        }

        public ActionResult Index()
        {
            return this.View(MvcConstants.INDEX);
        }

        public JsonResult GetData()
        {
            return this.Json(this._service.GetServiceActionRecords());
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
    }
}
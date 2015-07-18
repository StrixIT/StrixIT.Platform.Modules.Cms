//-----------------------------------------------------------------------
// <copyright file="CommentController.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Web.Mvc;
using StrixIT.Platform.Core;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    public class CommentController : BaseController
    {
        private ICommentService _service;

        public CommentController(ICommentService service)
            : base()
        {
            this._service = service;
        }

        public JsonResult GetComments(Guid entityId, string culture)
        {
            return this.Json(this._service.GetComments(entityId, culture));
        }

        public JsonResult SaveComment(CommentViewModel model)
        {
            return this.Json(this._service.SaveComment(model));
        }

        public JsonResult DeleteComment(CommentViewModel model)
        {
            return this.Json(this._service.DeleteComment(model));
        }
    }
}
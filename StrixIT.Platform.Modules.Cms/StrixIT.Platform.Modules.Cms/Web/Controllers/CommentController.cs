#region Apache License
//-----------------------------------------------------------------------
// <copyright file="CommentController.cs" company="StrixIT">
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
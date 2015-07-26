#region Apache License
//-----------------------------------------------------------------------
// <copyright file="NewsController.cs" company="StrixIT">
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

using System.Web.Mvc;
using StrixIT.Platform.Web;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    [StrixAuthorization(Roles = CmsRoleNames.EDITORROLES)]
    public class NewsController : EntityController<NewsViewModel>
    {
        public NewsController(INewsService newsService, ICommentService commentService) : base(newsService, commentService) { }

        [AllowAnonymous]
        public override ActionResult Index()
        {
            if (!this.CheckIsAccessAllowed())
            {
                return new HttpStatusCodeResult(401);
            }

            var config = new EntityListConfiguration<NewsViewModel>(StrixPlatform.User);
            config.Fields.Insert(0, new ListFieldConfiguration("PublishedOn", "kendoDate") { ShowFilter = false });
            config.Fields.Insert(1, new ListFieldConfiguration("ExpireTime", "kendoDate") { ShowFilter = false });
            return this.View(config);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult DisplayLatest()
        {
            var model = ((INewsService)this.Service).GetLatest();

            if (model == null)
            {
                model = new NewsViewModel();
            }

            return this.View("Latest", model);
        }
    }
}
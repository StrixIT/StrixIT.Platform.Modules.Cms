//-----------------------------------------------------------------------
// <copyright file="NewsController.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
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
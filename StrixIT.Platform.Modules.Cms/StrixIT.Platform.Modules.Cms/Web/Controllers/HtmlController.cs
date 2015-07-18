//-----------------------------------------------------------------------
// <copyright file="HtmlController.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    [StrixAuthorization(Roles = CmsRoleNames.EDITORROLES)]
    public class HtmlController : EntityController<HtmlViewModel>
    {
        public HtmlController(IEntityService<HtmlViewModel> service, ICommentService commentService) : base(service, commentService) { }
    }
}
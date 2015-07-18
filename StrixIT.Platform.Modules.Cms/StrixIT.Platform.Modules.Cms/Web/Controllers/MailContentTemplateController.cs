//-----------------------------------------------------------------------
// <copyright file="MailContentTemplateController.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using StrixIT.Platform.Core;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    [StrixAuthorization(Roles = PlatformConstants.ADMINROLE)]
    public class MailContentTemplateController : EntityController<MailContentTemplateViewModel>
    {
        public MailContentTemplateController(IEntityService<MailContentTemplateViewModel> service, ICommentService commentService) : base(service, commentService) { }
    }
}
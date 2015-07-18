//-----------------------------------------------------------------------
// <copyright file="MembershipMailHandler.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class MembershipMailHandler : IHandlePlatformEvent<GeneralEvent>
    {
        private IMailService _mailService;

        public MembershipMailHandler(IMailService mailService)
        {
            this._mailService = mailService;
        }

        public void Handle(GeneralEvent args)
        {
            if (args.EventName != "SendMembershipMailEvent")
            {
                return;
            }

            var mailTemplateName = (string)args.Data["TemplateName"];
            var culture = (string)args.Data["Culture"];
            var template = (string)args.Data["Template"];
            var body = (string)args.Data["Body"];
            var subject = (string)args.Data["Subject"];

            var mail = this._mailService.GetMailContent(culture, mailTemplateName);

            if (mail != null)
            {
                args.Data["Template"] = mail.Template.Body;
                args.Data["Body"] = mail.Body;
                args.Data["Subject"] = mail.Subject;
            }
        }
    }
}
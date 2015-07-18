//-----------------------------------------------------------------------
// <copyright file="MailContentTemplateViewModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Web.Mvc;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The view model for mail templates.
    /// </summary>
    public class MailContentTemplateViewModel : EntityViewModel
    {
        /// <summary>
        /// Gets or sets the mail template body.
        /// </summary>
        [AllowHtml]
        public string Body { get; set; }
    }
}
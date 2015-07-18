//-----------------------------------------------------------------------
// <copyright file="MailContentViewModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The view model class for mails.
    /// </summary>
    public class MailContentViewModel : EntityViewModel
    {
        /// <summary>
        /// Gets or sets the mail from address.
        /// </summary>
        [StrixRequired]
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the mail subject.
        /// </summary>
        [StrixRequired]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the mail body.
        /// </summary>
        [AllowHtml]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the mail template id.
        /// </summary>
        public Guid TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the mail's mail template.
        /// </summary>
        public MailContentTemplateViewModel Template { get; set; }

        /// <summary>
        /// Gets or sets a select list with all availabe templates for editing.
        /// </summary>
        public List<MailContentTemplateListModel> Templates { get; set; }
    }
}
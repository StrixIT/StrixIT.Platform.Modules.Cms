//-----------------------------------------------------------------------
// <copyright file="MailContent.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.ComponentModel.DataAnnotations;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The class for mail content.
    /// </summary>
    public class MailContent : ContentBase, IContent
    {
        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        [StrixRequired]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        public PlatformEntity Entity { get; set; }

        /// <summary>
        /// Gets or sets the id of the mail template.
        /// </summary>
        [StrixRequired]
        public Guid TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the mail template.
        /// </summary>
        public MailContentTemplate Template { get; set; }

        /// <summary>
        /// Gets or sets the mail from address.
        /// </summary>
        [StrixRequired]
        [StringLength(250)]
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the mail subject.
        /// </summary>
        [StrixRequired]
        [StringLength(250)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the mail body.
        /// </summary>
        [Rte]
        [StrixRequired]
        public string Body { get; set; }
    }
}
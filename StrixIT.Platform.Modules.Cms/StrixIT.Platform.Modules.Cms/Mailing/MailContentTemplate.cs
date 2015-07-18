//-----------------------------------------------------------------------
// <copyright file="MailContentTemplate.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The mail template, used to be able to style multiple mails in a uniform way.
    /// </summary>
    public class MailContentTemplate : ContentBase, IContent
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
        /// Gets or sets the template body.
        /// </summary>
        [Rte]
        [StrixRequired]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the mail content using this template.
        /// </summary>
        public ICollection<MailContent> Mails { get; set; }
    }
}
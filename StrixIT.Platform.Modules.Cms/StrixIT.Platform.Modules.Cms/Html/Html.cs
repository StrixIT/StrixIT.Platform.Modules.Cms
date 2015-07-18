//-----------------------------------------------------------------------
// <copyright file="Html.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.ComponentModel.DataAnnotations;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The class for all text messages in the system, such as web texts and news.
    /// </summary>
    public class Html : ContentBase, IContent
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
        /// Gets or sets the html body text.
        /// </summary>
        [Rte]
        [StrixRequired]
        public string Body { get; set; }
    }
}
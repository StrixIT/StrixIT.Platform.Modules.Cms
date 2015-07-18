//-----------------------------------------------------------------------
// <copyright file="News.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The class for all text messages in the system, such as web texts and news.
    /// </summary>
    public class News : ContentBase, IContent
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
        /// Gets or sets the news summary.
        /// </summary>
        [Rte]
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the news body text.
        /// </summary>
        [Rte]
        [StrixRequired]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the news expire time. Used for news that has a limited scope, such as announcements.
        /// </summary>
        [StrixNotDefault]
        public DateTime? ExpireTime { get; set; }
    }
}
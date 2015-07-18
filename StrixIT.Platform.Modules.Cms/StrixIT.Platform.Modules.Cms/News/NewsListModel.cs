//-----------------------------------------------------------------------
// <copyright file="NewsListModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Newtonsoft.Json;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The list model for displaying a list of news messages.
    /// </summary>
    public class NewsListModel : EntityListModel
    {
        /// <summary>
        /// Gets or sets the message summary.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the message body.
        /// </summary>
        [JsonIgnore]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the message publish time.
        /// </summary>
        public DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the message expire time.
        /// </summary>
        public DateTime? ExpireTime { get; set; }
    }
}
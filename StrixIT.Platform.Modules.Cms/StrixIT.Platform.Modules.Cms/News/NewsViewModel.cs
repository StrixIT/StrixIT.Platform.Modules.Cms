//-----------------------------------------------------------------------
// <copyright file="NewsViewModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Web.Mvc;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The view model for a news message.
    /// </summary>
    public class NewsViewModel : EntityViewModel
    {
        /// <summary>
        /// Gets or sets the message summary.
        /// </summary>
        [AllowHtml]
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the message body.
        /// </summary>
        [AllowHtml]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the expire time.
        /// </summary>
        public DateTime? ExpireTime { get; set; }
    }
}
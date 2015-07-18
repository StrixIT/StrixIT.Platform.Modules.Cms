//-----------------------------------------------------------------------
// <copyright file="IAudit.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// An interface to define audit properties on entities.
    /// </summary>
    public interface IAudit
    {
        /// <summary>
        /// Gets or sets the id of the user who created this content.
        /// </summary>
        Guid CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time this content was created.
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who last updated this content.
        /// </summary>
        Guid UpdatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time this content was last updated.
        /// </summary>
        DateTime UpdatedOn { get; set; }
    }
}
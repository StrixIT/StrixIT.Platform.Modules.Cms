//-----------------------------------------------------------------------
// <copyright file="AuditViewModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using Newtonsoft.Json;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The base view model for all autit entities.
    /// </summary>
    public abstract class AuditViewModel : PlatformBaseViewModel, IAudit
    {
        /// <summary>
        /// Gets or sets the id of the user that created this entity.
        /// </summary>
        [JsonIgnore]
        public Guid CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time this entity was created.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the name of the user that created or last modified this entity.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the id of the user that last updated this entity, if any.
        /// </summary>
        [JsonIgnore]
        public Guid UpdatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user that created or last modified this entity.
        /// </summary>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets the name of the user that created or last modified this entity.
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can edit this view model.
        /// </summary>
        public bool CanEdit { get; set; }
    }
}
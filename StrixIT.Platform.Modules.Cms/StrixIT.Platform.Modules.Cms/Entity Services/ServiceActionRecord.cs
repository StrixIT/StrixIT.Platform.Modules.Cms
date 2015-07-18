//-----------------------------------------------------------------------
// <copyright file="ServiceActionRecord.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The class used to create the entity services screen.
    /// </summary>
    public class ServiceActionRecord
    {
        /// <summary>
        /// Gets or sets the entity service record id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the entity type id for this service record.
        /// </summary>
        public Guid EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the entity type for this service record.
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Gets or sets the service name for this service record.
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Gets or sets the action name for this service record.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this record is selected.
        /// </summary>
        public bool Selected { get; set; }
    }
}
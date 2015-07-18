//-----------------------------------------------------------------------
// <copyright file="EntityCustomField.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A class to define custom fields for entities.
    /// </summary>
    public class EntityCustomField : CustomField
    {
        /// <summary>
        /// Gets or sets the entity type id the custom field is for.
        /// </summary>
        public Guid EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the entity type the custom field is for.
        /// </summary>
        public EntityType EntityType { get; set; }

        /// <summary>
        /// Gets or sets the group id the custom field is defined for.
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// Gets or sets the group the custom field is defined for.
        /// </summary>
        public GroupData Group { get; set; }
    }
}
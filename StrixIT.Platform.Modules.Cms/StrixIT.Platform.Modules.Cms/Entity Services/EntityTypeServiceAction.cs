//-----------------------------------------------------------------------
// <copyright file="EntityTypeServiceAction.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.ComponentModel.DataAnnotations;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A class for enabling/disabling entity services, such as translations and versioning, for an entity type.
    /// </summary>
    public class EntityTypeServiceAction : ValidationBase
    {
        /// <summary>
        /// Gets or sets the id of this EntityTypeServiceAvtion.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the id of the entity type this EntityTypeServiceAction object is for.
        /// </summary>
        [StrixRequired]
        public Guid EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the entity type this EntityTypeServiceAction object is for.
        /// </summary>
        public EntityType EntityType { get; set; }

        /// <summary>
        /// Gets or sets the id of the group this EntityTypeServiceAction object is for.
        /// </summary>
        [StrixRequired]
        public Guid GroupId { get; set; }

        /// <summary>
        /// Gets or sets the group this EntityTypeServiceAction object is for.
        /// </summary>
        [StrixRequired]
        public GroupData Group { get; set; }

        /// <summary>
        /// Gets or sets the name of the action this Entity Type Service Action object is for.
        /// </summary>
        [StrixRequired]
        [StringLength(250)]
        public string Action { get; set; }
    }
}
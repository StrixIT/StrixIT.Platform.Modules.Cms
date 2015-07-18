//-----------------------------------------------------------------------
// <copyright file="ContentSharedWithGroup.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A class to allow sharing content with other groups.
    /// </summary>
    public class ContentSharedWithGroup
    {
        /// <summary>
        /// Gets or sets the id of the entity shared.
        /// </summary>
        [StrixRequired]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the entity shared.
        /// </summary>
        public PlatformEntity Entity { get; set; }

        /// <summary>
        /// Gets or sets the group the entity is shared with.
        /// </summary>
        [StrixRequired]
        public Guid GroupId { get; set; }

        /// <summary>
        /// Gets or sets the group this entity is shared with.
        /// </summary>
        public GroupData Group { get; set; }
    }
}
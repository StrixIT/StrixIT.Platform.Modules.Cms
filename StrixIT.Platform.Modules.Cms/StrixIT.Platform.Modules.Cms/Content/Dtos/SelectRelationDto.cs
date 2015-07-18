//-----------------------------------------------------------------------
// <copyright file="SelectRelationDto.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A dto class to support managing many-on-many relations between entities.
    /// </summary>
    /// <typeparam name="TKey">The type of the relation primary key</typeparam>
    public class SelectRelationDto<TKey> where TKey : struct
    {
        /// <summary>
        /// Gets or sets the relation id.
        /// </summary>
        public TKey? Id { get; set; }

        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        public Guid? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the name of the relation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the relation is selected.
        /// </summary>
        public bool Selected { get; set; }
    }
}
#region Apache License

//-----------------------------------------------------------------------
// <copyright file="PlatformEntity.cs" company="StrixIT">
// Copyright 2015 StrixIT. Author R.G. Schurgers MA MSc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

#endregion Apache License

using StrixIT.Platform.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The base entity class for all platform entities.
    /// </summary>
    public class PlatformEntity
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the entity's comments.
        /// </summary>
        public ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Gets or sets the groups that are allowed to access this entity, in addition to the
        /// user's own group.
        /// </summary>
        public ICollection<ContentSharedWithGroup> ContentSharedWithGroups { get; set; }

        /// <summary>
        /// Gets or sets the EntityType of the entity.
        /// </summary>
        public EntityType EntityType { get; set; }

        /// <summary>
        /// Gets or sets the EntityTypeId of the entity.
        /// </summary>
        [StrixRequired]
        public Guid EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the group this entity belongs to.
        /// </summary>
        public GroupData Group { get; set; }

        /// <summary>
        /// Gets or sets the id of the group this entity belongs to.
        /// </summary>
        [StrixRequiredWithMembershipAttribute]
        public Guid GroupId { get; set; }

        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the owning user allows other users with the
        /// proper rights to access this content.
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Gets or sets the user who owns this entity.
        /// </summary>
        public UserData OwnerUser { get; set; }

        /// <summary>
        /// Gets or sets the Id of the user who owns this entity.
        /// </summary>
        [StrixRequiredWithMembershipAttribute]
        public Guid OwnerUserId { get; set; }

        /// <summary>
        /// Gets or sets the entity's tags.
        /// </summary>
        public ICollection<Term> Tags { get; set; }

        /// <summary>
        /// Gets or sets the Url of the entity.
        /// </summary>
        [StrixRequired]
        [StringLength(300)]
        public string Url { get; set; }

        #endregion Public Properties
    }
}
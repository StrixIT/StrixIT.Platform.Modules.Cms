﻿#region Apache License

//-----------------------------------------------------------------------
// <copyright file="Vocabulary.cs" company="StrixIT">
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
    /// A class that defines a vocabulary for the Taxonomy system. A vocabulary holds a collection
    /// of terms.
    /// </summary>
    public class Vocabulary : ValidationBase
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the culture this vocabulary is for.
        /// </summary>
        [StrixRequired]
        [StringLength(5)]
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the entity types that can use this Vocabulary. If empty, all types can use
        /// this Vocabulary.
        /// </summary>
        public virtual ICollection<EntityType> EntityTypes { get; set; }

        /// <summary>
        /// Gets or sets the group this vocabulary belongs to.
        /// </summary>
        public GroupData Group { get; set; }

        /// <summary>
        /// Gets or sets the id of the group this vocabulary belongs to.
        /// </summary>
        [StrixRequiredWithMembershipAttribute]
        public Guid GroupId { get; set; }

        /// <summary>
        /// Gets or sets the vocabulary id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this Vocabulary is used by the Platform
        /// internally and should not be modified or extended by users.
        /// </summary>
        public bool IsSystemVocabulary { get; set; }

        /// <summary>
        /// Gets or sets the name of the Entity.
        /// </summary>
        [StrixRequired]
        [StringLength(250, MinimumLength = 2)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Terms in this Vocabulary.
        /// </summary>
        public virtual ICollection<Term> Terms { get; set; }

        /// <summary>
        /// Gets or sets the Url of the entity.
        /// </summary>
        [StrixRequired]
        [StringLength(300)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether regular users can add Terms to the Vocabulary.
        /// Used for free tagging.
        /// </summary>
        public bool UserExtensible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this Vocabulary allows for parent-child
        /// relations between Terms.
        /// </summary>
        public bool UseTermHierarchy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this vocabulary allows for relations between Terms.
        /// </summary>
        public bool UseTermRelations { get; set; }

        #endregion Public Properties
    }
}
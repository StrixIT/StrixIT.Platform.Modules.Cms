#region Apache License
//-----------------------------------------------------------------------
// <copyright file="EntityType.cs" company="StrixIT">
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
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A class that contains the basic information for an entity type as stored in the database. 
    /// </summary>
    public class EntityType : ValidationBase
    {
        /// <summary>
        /// Gets or sets the Id of this EntityType.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the full name of the type of the entities of this type.
        /// </summary>
        [StrixRequired]
        [StringLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the vocabularies that can be used with this entity type.
        /// </summary>
        public ICollection<Vocabulary> Vocabularies { get; set; }

        /// <summary>
        /// Gets or sets the service actions that entities of this type can use.
        /// </summary>
        public ICollection<EntityTypeServiceAction> EntityTypeServiceActions { get; set; }

        /// <summary>
        /// Gets or sets the entity's custom fields.
        /// </summary>
        public ICollection<EntityCustomField> CustomFields { get; set; }
    }
}
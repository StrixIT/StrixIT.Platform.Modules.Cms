#region Apache License

//-----------------------------------------------------------------------
// <copyright file="EntityCustomField.cs" company="StrixIT">
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

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A class to define custom fields for entities.
    /// </summary>
    public class EntityCustomField : CustomField
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the entity type the custom field is for.
        /// </summary>
        public EntityType EntityType { get; set; }

        /// <summary>
        /// Gets or sets the entity type id the custom field is for.
        /// </summary>
        public Guid EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the group the custom field is defined for.
        /// </summary>
        public GroupData Group { get; set; }

        /// <summary>
        /// Gets or sets the group id the custom field is defined for.
        /// </summary>
        public Guid GroupId { get; set; }

        #endregion Public Properties
    }
}
#region Apache License
//-----------------------------------------------------------------------
// <copyright file="ServiceActionRecord.cs" company="StrixIT">
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
﻿#region Apache License

//-----------------------------------------------------------------------
// <copyright file="SelectRelationDto.cs" company="StrixIT">
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

using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A dto class to support managing many-on-many relations between entities.
    /// </summary>
    /// <typeparam name="TKey">The type of the relation primary key</typeparam>
    public class SelectRelationDto<TKey> where TKey : struct
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        public Guid? EntityId { get; set; }

        /// <summary>
        /// Gets or sets the relation id.
        /// </summary>
        public TKey? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the relation.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the relation is selected.
        /// </summary>
        public bool Selected { get; set; }

        #endregion Public Properties
    }
}
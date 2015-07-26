﻿#region Apache License
//-----------------------------------------------------------------------
// <copyright file="ContentSharedWithGroup.cs" company="StrixIT">
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
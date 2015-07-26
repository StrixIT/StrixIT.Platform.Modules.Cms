#region Apache License

//-----------------------------------------------------------------------
// <copyright file="News.cs" company="StrixIT">
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
    /// The class for all text messages in the system, such as web texts and news.
    /// </summary>
    public class News : ContentBase, IContent
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the news body text.
        /// </summary>
        [Rte]
        [StrixRequired]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the entity.
        /// </summary>
        public PlatformEntity Entity { get; set; }

        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        [StrixRequired]
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the news expire time. Used for news that has a limited scope, such as announcements.
        /// </summary>
        [StrixNotDefault]
        public DateTime? ExpireTime { get; set; }

        /// <summary>
        /// Gets or sets the news summary.
        /// </summary>
        [Rte]
        public string Summary { get; set; }

        #endregion Public Properties
    }
}
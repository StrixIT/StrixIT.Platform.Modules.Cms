#region Apache License

//-----------------------------------------------------------------------
// <copyright file="NewsListModel.cs" company="StrixIT">
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

using Newtonsoft.Json;
using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The list model for displaying a list of news messages.
    /// </summary>
    public class NewsListModel : EntityListModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the message body.
        /// </summary>
        [JsonIgnore]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the message expire time.
        /// </summary>
        public DateTime? ExpireTime { get; set; }

        /// <summary>
        /// Gets or sets the message publish time.
        /// </summary>
        public DateTime? PublishedOn { get; set; }

        /// <summary>
        /// Gets or sets the message summary.
        /// </summary>
        public string Summary { get; set; }

        #endregion Public Properties
    }
}
#region Apache License

//-----------------------------------------------------------------------
// <copyright file="MailContentTemplate.cs" company="StrixIT">
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

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The mail template, used to be able to style multiple mails in a uniform way.
    /// </summary>
    public class MailContentTemplate : ContentBase, IContent
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the template body.
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
        /// Gets or sets the mail content using this template.
        /// </summary>
        public ICollection<MailContent> Mails { get; set; }

        #endregion Public Properties
    }
}
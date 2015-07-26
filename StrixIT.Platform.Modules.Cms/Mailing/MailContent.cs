#region Apache License

//-----------------------------------------------------------------------
// <copyright file="MailContent.cs" company="StrixIT">
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
using System.ComponentModel.DataAnnotations;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The class for mail content.
    /// </summary>
    public class MailContent : ContentBase, IContent
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the mail body.
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
        /// Gets or sets the mail from address.
        /// </summary>
        [StrixRequired]
        [StringLength(250)]
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the mail subject.
        /// </summary>
        [StrixRequired]
        [StringLength(250)]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the mail template.
        /// </summary>
        public MailContentTemplate Template { get; set; }

        /// <summary>
        /// Gets or sets the id of the mail template.
        /// </summary>
        [StrixRequired]
        public Guid TemplateId { get; set; }

        #endregion Public Properties
    }
}
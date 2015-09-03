#region Apache License

//-----------------------------------------------------------------------
// <copyright file="MailContentViewModel.cs" company="StrixIT">
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
using System.Web.Mvc;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The view model class for mails.
    /// </summary>
    public class MailContentViewModel : EntityViewModel
    {
        #region Public Constructors

        public MailContentViewModel() : base(typeof(MailContent))
        {
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the mail body.
        /// </summary>
        [AllowHtml]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the mail from address.
        /// </summary>
        [StrixRequired]
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the mail subject.
        /// </summary>
        [StrixRequired]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the mail's mail template.
        /// </summary>
        public MailContentTemplateViewModel Template { get; set; }

        /// <summary>
        /// Gets or sets the mail template id.
        /// </summary>
        public Guid TemplateId { get; set; }

        /// <summary>
        /// Gets or sets a select list with all availabe templates for editing.
        /// </summary>
        public List<MailContentTemplateListModel> Templates { get; set; }

        #endregion Public Properties
    }
}
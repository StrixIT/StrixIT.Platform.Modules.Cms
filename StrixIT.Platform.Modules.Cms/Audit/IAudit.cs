#region Apache License

//-----------------------------------------------------------------------
// <copyright file="IAudit.cs" company="StrixIT">
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
    /// An interface to define audit properties on entities.
    /// </summary>
    public interface IAudit
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the id of the user who created this content.
        /// </summary>
        Guid CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time this content was created.
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who last updated this content.
        /// </summary>
        Guid UpdatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time this content was last updated.
        /// </summary>
        DateTime UpdatedOn { get; set; }

        #endregion Public Properties
    }
}
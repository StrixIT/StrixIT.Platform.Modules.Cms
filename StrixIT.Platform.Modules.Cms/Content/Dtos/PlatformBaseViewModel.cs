#region Apache License

//-----------------------------------------------------------------------
// <copyright file="PlatformBaseViewModel.cs" company="StrixIT">
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
using StrixIT.Platform.Core;
using System;
using System.Linq;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The base view model for all view models in the Cms.
    /// </summary>
    public abstract class PlatformBaseViewModel : ValidationBase
    {
        #region Private Fields

        /// <summary>
        /// The entity type this view model is for.
        /// </summary>
        private Type _entityType;

        #endregion Private Fields

        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformBaseViewModel"/> class.
        /// </summary>
        protected PlatformBaseViewModel()
        {
            this._entityType = EntityHelper.GetObjectMap(this.GetType()).ContentType;
            this.AdminRoles = new string[] { PlatformConstants.ADMINROLE };
            this.EditRoles = CmsRoleNames.EDITORROLES.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries).Trim().ToArray();
        }

        #endregion Protected Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the names of the roles that are allowed to administer this object.
        /// </summary>
        [JsonIgnore]
        public string[] AdminRoles { get; set; }

        /// <summary>
        /// Gets or sets the names of the roles that are allowed to edit this object.
        /// </summary>
        [JsonIgnore]
        public string[] EditRoles { get; set; }

        /// <summary>
        /// Gets the entity type this view model is for.
        /// </summary>
        [JsonIgnore]
        public virtual Type EntityType
        {
            get
            {
                return this._entityType;
            }
        }

        #endregion Public Properties
    }
}
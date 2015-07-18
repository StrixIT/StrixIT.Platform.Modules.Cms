//-----------------------------------------------------------------------
// <copyright file="PlatformBaseViewModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Linq;
using Newtonsoft.Json;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The base view model for all view models in the Cms.
    /// </summary>
    public abstract class PlatformBaseViewModel : ValidationBase
    {
        /// <summary>
        /// The entity type this view model is for.
        /// </summary>
        private Type _entityType;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformBaseViewModel" /> class.
        /// </summary>
        protected PlatformBaseViewModel()
        {
            this._entityType = EntityHelper.GetObjectMap(this.GetType()).ContentType;
            this.AdminRoles = new string[] { PlatformConstants.ADMINROLE };
            this.EditRoles = CmsRoleNames.EDITORROLES.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries).Trim().ToArray();
        }

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
    }
}
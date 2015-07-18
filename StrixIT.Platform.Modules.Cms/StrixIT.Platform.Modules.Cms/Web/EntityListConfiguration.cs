//-----------------------------------------------------------------------
// <copyright file="EntityListConfiguration.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The default list configuration class for cms entities.
    /// </summary>
    /// <typeparam name="T">The entity view model type</typeparam>
    public class EntityListConfiguration<T> : ListConfiguration where T : EntityViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityListConfiguration{T}" /> class.
        /// </summary>
        /// <param name="userContext">The user context to use</param>
        public EntityListConfiguration(IUserContext userContext) : this(userContext, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityListConfiguration{T}" /> class.
        /// </summary>
        /// <param name="userContext">The user context to use</param>
        /// <param name="properties">The names of the properties to display in the list</param>
        public EntityListConfiguration(IUserContext userContext, IEnumerable<string> properties)
            : base(EntityHelper.GetObjectMap(typeof(T)).ListModelType, properties != null ? properties.Concat(new string[] { CmsConstants.UPDATEDBY, CmsConstants.UPDATEDON }) : new string[] { CmsConstants.UPDATEDBY, CmsConstants.UPDATEDON })
        {
            var updatedOnField = this.Fields.First(f => f.Name == CmsConstants.UPDATEDON);
            updatedOnField.ShowFilter = false;
            this.InterfaceResourceType = typeof(Resources.Interface);
            this.CanCreate = true;
            this.CanEdit = true;
            this.CanDelete = true;
        }
    }
}
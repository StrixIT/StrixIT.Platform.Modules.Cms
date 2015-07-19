#region Apache License
//-----------------------------------------------------------------------
// <copyright file="EntityListConfiguration.cs" company="StrixIT">
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
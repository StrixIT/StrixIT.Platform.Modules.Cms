//-----------------------------------------------------------------------
// <copyright file="VocabularyListConfiguration.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class VocabularyListConfiguration : ListConfiguration
    {
        public VocabularyListConfiguration(IUserContext userContext)
            : base(typeof(VocabularyViewModel))
        {
            this.InterfaceResourceType = typeof(Resources.Interface);
            this.CanCreate = userContext.HasPermission(CmsPermissions.AddVocabulary);
            this.CanEdit = userContext.HasPermission(CmsPermissions.EditVocabulary);
            this.CanDelete = userContext.HasPermission(CmsPermissions.DeleteVocabulary);
        }
    }
}
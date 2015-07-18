//-----------------------------------------------------------------------
// <copyright file="CmsModuleConfiguration.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using System;
using StrixIT.Platform.Core;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    public class CmsModuleConfiguration : IWebModuleConfiguration
    {
        public string Name
        {
            get
            {
                return CmsConstants.CMS;
            }
        }

        public IList<ModuleLink> ModuleLinks
        {
            get
            {
                return new List<ModuleLink> 
                { 
                    new ModuleLink(Resources.Interface.HtmlIndex, null, "Html"),
                    new ModuleLink(Resources.Interface.NewsIndex, null, "News"),
                    new ModuleLink(Resources.Interface.DocumentIndex, null, "Document"),
                    new ModuleLink(Resources.Interface.MailContentTemplateIndex, null, "MailContentTemplate"),
                    new ModuleLink(Resources.Interface.MailContentIndex, null, "MailContent"),
                    new ModuleLink(Resources.Interface.TaxonomyIndex, CmsPermissions.ViewVocabularies, "Vocabulary"),
                    new ModuleLink(Resources.Interface.EntityServiceActionIndex, CmsPermissions.ManageServices, "EntityAction"),
                };
            }
        }

        public IDictionary<string, IList<string>> ModulePermissions
        {
            get
            {
                var dictionary = new Dictionary<string, IList<string>>();

                var adminPermissions = new List<string>
                {
                    CmsPermissions.ViewVocabularies,
                    CmsPermissions.AddVocabulary,
                    CmsPermissions.EditVocabulary,
                    CmsPermissions.DeleteVocabulary,
                    CmsPermissions.ManageServices
                };

                var groupAdminPermissions = new List<string>
                {
                    CmsPermissions.ViewVocabularies,
                    CmsPermissions.AddVocabulary,
                    CmsPermissions.EditVocabulary,
                    CmsPermissions.DeleteVocabulary,
                    CmsPermissions.ManageServices
                };

                var editorPermissions = new List<string>()
                {
                    PlatformPermissions.AccessSite,
                    PlatformPermissions.ViewAdminDashboard,
                    CmsPermissions.ViewVocabularies,
                    CmsPermissions.EditVocabulary,
                };

                var contributorPermissions = new List<string>()
                {
                    PlatformPermissions.AccessSite
                };

                dictionary.Add(PlatformConstants.ADMINROLE, adminPermissions);
                dictionary.Add(PlatformConstants.GROUPADMINROLE, groupAdminPermissions);
                dictionary.Add(PlatformConstants.EDITORROLE, editorPermissions);
                dictionary.Add(PlatformConstants.CONTRIBUTORROLE, contributorPermissions);
                return dictionary;
            }
        }

        public IList<string> StyleBundles
        {
            get
            {
                return new List<string> { "~/Areas/Cms/Styles/css" };
            }
        }

        public IList<string> ScriptBundles
        {
            get
            {
                return new List<string> { "~/bundles/cms" };
            }
        }
    }
}
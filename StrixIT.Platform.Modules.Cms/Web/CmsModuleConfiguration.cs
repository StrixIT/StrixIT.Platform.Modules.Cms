#region Apache License

//-----------------------------------------------------------------------
// <copyright file="CmsModuleConfiguration.cs" company="StrixIT">
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
using StrixIT.Platform.Web;
using System.Collections.Generic;

namespace StrixIT.Platform.Modules.Cms
{
    public class CmsModuleConfiguration : IWebModuleConfiguration
    {
        #region Public Properties

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

        public string Name
        {
            get
            {
                return CmsConstants.CMS;
            }
        }

        public IList<string> ScriptBundles
        {
            get
            {
                return new List<string> { "~/bundles/cms" };
            }
        }

        public IList<string> StyleBundles
        {
            get
            {
                return new List<string> { "~/Areas/Cms/Styles/css" };
            }
        }

        #endregion Public Properties
    }
}
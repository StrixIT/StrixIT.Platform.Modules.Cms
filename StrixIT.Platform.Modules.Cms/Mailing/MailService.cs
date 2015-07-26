#region Apache License

//-----------------------------------------------------------------------
// <copyright file="MailService.cs" company="StrixIT">
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
using System.Linq;

namespace StrixIT.Platform.Modules.Cms
{
    public class MailService : EntityService<MailContentViewModel>, IMailService
    {
        #region Public Constructors

        public MailService(IPlatformDataSource dataSource, IEntityManager entityManager, ITaxonomyManager taxonomyManager, ICacheService cache) : base(dataSource, entityManager, taxonomyManager, cache)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public MailContent GetMailContent(string culture, string name)
        {
            if (string.IsNullOrWhiteSpace(culture))
            {
                Logger.Log("No user culture specified. Using default culture.", LogLevel.Warning);
                culture = StrixPlatform.CurrentCultureCode;
            }

            var mail = this.Manager.Query<MailContent>("Template").Where(m => m.Name.ToLower().Equals(name.ToLower())
                                                                                             && m.Culture == culture
                                                                                             && m.IsCurrentVersion).FirstOrDefault();

            if (mail == null)
            {
                Logger.Log(string.Format("Mail {0} does not exist. Please create it first.", name), LogLevel.Error);
                return null;
            }

            if (mail.Template == null)
            {
                Logger.Log(string.Format("Mail {0} does not have a template. Please assign it.", mail.Name), LogLevel.Error);
                return null;
            }

            mail.Body = Web.Helpers.HtmlDecode(mail.Body, false);
            mail.Template.Body = Web.Helpers.HtmlDecode(mail.Template.Body, false);
            return mail;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override EntityViewModel Get(Type modelType, Guid? id, string url, string culture, int versionNumber, string relationsToInclude, bool useFallBack = false)
        {
            var model = base.Get(modelType, id, url, culture, versionNumber, "Template", useFallBack) as MailContentViewModel;

            this.GetTemplates(model, culture ?? StrixPlatform.CurrentCultureCode);

            if (model.Template != null)
            {
                model.Template.Body = StrixIT.Platform.Web.Helpers.HtmlDecode(model.Template.Body, false);
            }

            return model;
        }

        #endregion Protected Methods

        #region Private Methods

        private void GetTemplates(MailContentViewModel model, string culture)
        {
            model.Templates = this.Manager.Query<MailContentTemplate>().Where(t => t.IsCurrentVersion && t.Culture.ToLower() == culture.ToLower()).Select(t => new MailContentTemplateListModel { Id = t.Id, Name = t.Name }).ToList();

            if (model.TemplateId == Guid.Empty && !model.Templates.IsEmpty())
            {
                model.TemplateId = model.Templates.First().Id;
            }
        }

        #endregion Private Methods
    }
}
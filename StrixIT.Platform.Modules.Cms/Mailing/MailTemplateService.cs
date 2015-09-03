#region Apache License

//-----------------------------------------------------------------------
// <copyright file="MailTemplateService.cs" company="StrixIT">
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
using StrixIT.Platform.Core.Environment;
using System;
using System.Linq;

namespace StrixIT.Platform.Modules.Cms
{
    public class MailTemplateService : EntityService<MailContentTemplateViewModel>
    {
        #region Public Constructors

        public MailTemplateService(ICmsData cmsData, ICacheService cache) : base(cmsData, cache)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public override SaveResult<MailContentTemplateViewModel> Save(MailContentTemplateViewModel model, bool saveChanges)
        {
            var entityModel = model as EntityViewModel;
            var currentVersionNumber = entityModel.VersionNumber;
            var currentId = entityModel.Id;
            var result = base.Save(model, false);
            var newVersionNumber = result.Model.VersionNumber;
            var newId = result.Model.Id;

            // If the template version has changed, update all mails that use this template.
            if (currentVersionNumber < newVersionNumber)
            {
                var mails = this.Manager.Query<MailContent>().Where(m => m.TemplateId == currentId).ToList();

                foreach (var mail in mails)
                {
                    mail.TemplateId = newId;
                    this.Manager.Save(mail);
                }
            }

            this.SaveChanges();

            return result;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void Delete(Type viewModelType, Guid id, string culture, int versionNumber, string log, bool saveChanges)
        {
            // Get and delete all mails using this template before trying to delete the template itself.
            var template = CmsData.EntityManager.Get<MailContentTemplate>(id, culture, versionNumber, "Mails");

            if (template == null)
            {
                return;
            }

            var mails = template.Mails.ToList();
            template.Mails.Clear();

            foreach (var mail in mails)
            {
                this.Manager.Delete(mail);
            }

            base.Delete(typeof(MailContentTemplateViewModel), id, culture, versionNumber, log, true);
        }

        #endregion Protected Methods
    }
}
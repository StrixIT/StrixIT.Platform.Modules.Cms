//-----------------------------------------------------------------------
// <copyright file="MailTemplateService.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Linq;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class MailTemplateService : EntityService<MailContentTemplateViewModel>
    {
        public MailTemplateService(IPlatformDataSource dataSource, IEntityManager entityManager, ITaxonomyManager taxonomyManager, ICacheService cache) : base(dataSource, entityManager, taxonomyManager, cache) { }

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

        protected override void Delete(Type viewModelType, Guid id, string culture, int versionNumber, string log, bool saveChanges)
        {
            // Get and delete all mails using this template before trying to delete the template itself.
            var template = this.Manager.Get<MailContentTemplate>(id, culture, versionNumber, "Mails");

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
    }
}
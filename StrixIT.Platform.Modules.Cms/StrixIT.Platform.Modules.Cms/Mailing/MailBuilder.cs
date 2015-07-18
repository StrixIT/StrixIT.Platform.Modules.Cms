//-----------------------------------------------------------------------
// <copyright file="MailBuilder.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class MailBuilder
    {
        private IFileSystemWrapper _fileSystemWrapper;
        private IMembershipService _membershipService;

        public MailBuilder(IFileSystemWrapper fileSystemWrapper, IMembershipService membershipService)
        {
            this._fileSystemWrapper = fileSystemWrapper;
            this._membershipService = membershipService;
        }

        public void InitMails(IPlatformDataSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (this._membershipService != null)
            {
                string from = this.GetFromAddress();

                // Add the default entity types.
                var mailTemplateType = new EntityType { Id = Guid.NewGuid(), Name = typeof(MailContentTemplate).FullName };
                var mailType = new EntityType { Id = Guid.NewGuid(), Name = typeof(MailContent).FullName };
                source.Save(mailTemplateType);
                source.Save(mailType);
                var adminId = this._membershipService.AdminId;
                var mainGroupId = StrixPlatform.MainGroupId;
                var date = DateTime.Now;
                var templateDir = ModuleManager.AppSettings["Membership"]["mailTemplateFolder"];
                var directory = StrixPlatform.Environment.MapPath(templateDir);
                var supportedCultures = StrixPlatform.Configuration.Cultures.ToLower().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Trim().ToArray();

                var templateContent = this.AddMailTemplate(source, adminId, mainGroupId, directory, mailTemplateType, date, supportedCultures);
                this.AddMails(source, adminId, mainGroupId, directory, templateContent, mailType, from, date, supportedCultures);
                source.SaveChanges();
            }
        }

        private string GetFromAddress()
        {
            var mailSettings = Helpers.GetConfigSectionGroup<MailSettingsSectionGroup>("system.net/mailSettings");
            return mailSettings.Smtp.From ?? "rutger@strixit.com";
        }

        private List<MailContentTemplate> AddMailTemplate(IPlatformDataSource source, Guid adminId, Guid mainGroupId, string directory, EntityType mailTemplateType, DateTime date, string[] supportedCultures)
        {
            // Add the mail template
            var templateName = "Default";
            var templateData = this._fileSystemWrapper.GetHtmlTemplate(directory, "MailTemplate", null);
            List<MailContentTemplate> templateContent = new List<MailContentTemplate>();

            var templateEntity = new PlatformEntity
            {
                Id = Guid.NewGuid(),
                GroupId = mainGroupId,
                Url = templateName.ToLower(),
                EntityType = mailTemplateType,
                OwnerUserId = adminId
            };

            source.Save(templateEntity);

            foreach (var data in templateData.Where(t => supportedCultures.Contains(t.Culture.ToLower())))
            {
                var template = new MailContentTemplate
                {
                    Id = Guid.NewGuid(),
                    EntityId = templateEntity.Id,
                    Entity = templateEntity,
                    Culture = data.Culture,
                    VersionNumber = 1,
                    Name = "Default",
                    CreatedByUserId = adminId,
                    CreatedOn = date,
                    UpdatedByUserId = adminId,
                    UpdatedOn = date,
                    IsCurrentVersion = true,
                    PublishedByUserId = adminId,
                    PublishedOn = date,
                    Body = data.Body
                };

                templateContent.Add(template);
                source.Save(template);
            }

            return templateContent;
        }

        private void AddMails(IPlatformDataSource source, Guid adminId, Guid mainGroupId, string directory, List<MailContentTemplate> templateContent, EntityType mailType, string from, DateTime date, string[] supportedCultures)
        {
            var allFiles = Directory.GetFiles(directory).Where(f => !f.ToLower().Split('\\').Last().StartsWith("mailtemplate") && f.EndsWith(".html"));
            List<string> mailFiles = new List<string>();

            foreach (var file in allFiles)
            {
                var normalisedName = file.Contains("_") ? file.Split('_').First() + "." + file.Split('.').Last() : file;

                if (!mailFiles.Contains(normalisedName))
                {
                    mailFiles.Add(normalisedName);
                }
            }

            foreach (var mailFile in mailFiles)
            {
                var mailData = this._fileSystemWrapper.GetHtmlTemplate(directory, mailFile.Split('\\').Last().Split('.').First(), null);

                var entity = new PlatformEntity
                {
                    Id = Guid.NewGuid(),
                    GroupId = mainGroupId,
                    Url = mailFile.Split('\\').Last().Split('.').First().ToLower(),
                    EntityType = mailType,
                    OwnerUserId = adminId
                };

                source.Save(entity);

                foreach (var data in mailData.Where(t => supportedCultures.Contains(t.Culture.ToLower())))
                {
                    var template = templateContent.First(t => t.Culture == data.Culture);

                    var content = new MailContent
                    {
                        Id = Guid.NewGuid(),
                        EntityId = entity.Id,
                        Entity = entity,
                        Template = template,
                        TemplateId = template.Id,
                        Culture = data.Culture,
                        VersionNumber = 1,
                        Name = mailFile.Split('\\').Last().Split('.').First(),
                        CreatedByUserId = adminId,
                        CreatedOn = date,
                        UpdatedByUserId = adminId,
                        UpdatedOn = date,
                        IsCurrentVersion = true,
                        PublishedByUserId = adminId,
                        PublishedOn = date,
                        From = from,
                        Subject = data.Subject,
                        Body = data.Body
                    };

                    source.Save(content);
                }
            }
        }
    }
}
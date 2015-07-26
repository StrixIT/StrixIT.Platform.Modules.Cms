#region Apache License

//-----------------------------------------------------------------------
// <copyright file="MailBuilder.cs" company="StrixIT">
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;

namespace StrixIT.Platform.Modules.Cms
{
    public class MailBuilder
    {
        #region Private Fields

        private IFileSystemWrapper _fileSystemWrapper;
        private IMembershipService _membershipService;

        #endregion Private Fields

        #region Public Constructors

        public MailBuilder(IFileSystemWrapper fileSystemWrapper, IMembershipService membershipService)
        {
            this._fileSystemWrapper = fileSystemWrapper;
            this._membershipService = membershipService;
        }

        #endregion Public Constructors

        #region Public Methods

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

        #endregion Public Methods

        #region Private Methods

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

        private string GetFromAddress()
        {
            var mailSettings = Helpers.GetConfigSectionGroup<MailSettingsSectionGroup>("system.net/mailSettings");
            return mailSettings.Smtp.From ?? "rutger@strixit.com";
        }

        #endregion Private Methods
    }
}
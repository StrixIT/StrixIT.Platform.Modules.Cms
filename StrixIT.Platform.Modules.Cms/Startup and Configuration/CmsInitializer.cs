#region Apache License
//-----------------------------------------------------------------------
// <copyright file="CmsInitializer.cs" company="StrixIT">
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Resources;
using System.Web.Mvc;
using System.Web.Optimization;
using StrixIT.Platform.Core;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    public class CmsInitializer : IInitializer, IWebInitializer
    {
        private static bool _isInitialized = false;

        private IPlatformDataSource _platformDataSource;
        private IFileSystemWrapper _fileSystemWrapper;
        private IImageConverter _imageConverter;
        private IMembershipService _membershipService;

        // Todo: inject membershipservice instead of using service locator.
        public CmsInitializer(IPlatformDataSource platformDataSource, IFileSystemWrapper fileSystemWrapper, IImageConverter imageConverter)
        {
            this._platformDataSource = platformDataSource;
            this._fileSystemWrapper = fileSystemWrapper;
            this._imageConverter = imageConverter;
            this._membershipService = DependencyInjector.TryGet<IMembershipService>(); //membershipService;
        }

        public void Initialize()
        {
            this.UpdateMembershipLookups();
            this.AddMailTemplates();
            RegisterDefaultTokens();
            DataMapper.OnCreateMap += this.ConfigureTypeMaps;
            this.RegisterMappings();
            PageRegistration.LocatePages();
        }

        public void WebInitialize()
        {
            RegisterBundles(BundleTable.Bundles);
        }

        internal static void ConfigureEntityMaps()
        {
            if (_isInitialized)
            {
                return;
            }

            DataMapper.CreateMap<PlatformEntity, SelectRelationDto<Guid>>().ForMember(s => s.EntityId, c => c.MapFrom(p => p.Id));

            var idConfig = new MapConfig<IContent, EntityListModel>();
            idConfig.MembersToMap.Add(d => d.Id, s => s.EntityId);
            DataMapper.RegisterMapConfig(idConfig);

            var listUrlConfig = new MapConfig<IContent, EntityListModel>();
            listUrlConfig.MembersToMap.Add(d => d.Url, s => s.Entity.Url);
            DataMapper.RegisterMapConfig(listUrlConfig);

            var detailUrlConfig = new MapConfig<IContent, EntityViewModel>();
            detailUrlConfig.MembersToMap.Add(d => d.Url, s => s.Entity.Url);
            DataMapper.RegisterMapConfig(detailUrlConfig);

            var entityCollectionConfig = new MapConfig<PlatformBaseViewModel, PlatformEntity>();
            entityCollectionConfig.MembersToIgnore.Add(d => d.EntityType);
            DataMapper.RegisterMapConfig(entityCollectionConfig);

            var entityConfig = new MapConfig<EntityViewModel, IContent>();

            entityConfig.AfterMapAction = (x, y) =>
            {
                var content = y as IContent;
                var model = x as EntityViewModel;

                if (content.Entity == null)
                {
                    content.Entity = new PlatformEntity();
                }

                content.Entity.Url = model.Url;
            };

            DataMapper.RegisterMapConfig(entityConfig);

            _isInitialized = true;
        }

        internal void UpdateMembershipLookups()
        {
            if (this._membershipService != null)
            {
                var existingGroupIds = this._platformDataSource.Query<GroupData>().Select(u => u.Id).ToArray();
                var newGroupData = this._membershipService.GroupData().Where(g => !existingGroupIds.Contains(g.Id));

                if (newGroupData.Count() > 0)
                {
                    this._platformDataSource.Save(newGroupData.ToList());
                    this._platformDataSource.SaveChanges();
                }

                var existingUserIds = this._platformDataSource.Query<UserData>().Select(u => u.Id).ToArray();
                var newUserData = this._membershipService.UserData().Where(g => !existingUserIds.Contains(g.Id));

                if (newUserData.Count() > 0)
                {
                    this._platformDataSource.Save(newUserData.ToList());
                    this._platformDataSource.SaveChanges();
                }
            }
        }

        internal void AddMailTemplates()
        {
            if (this._platformDataSource.Query<MailContentTemplate>().Count() == 0)
            {
                new MailBuilder(this._fileSystemWrapper, this._membershipService).InitMails(this._platformDataSource);
            }
        }

        internal void RegisterMappings()
        {
            ConfigureFilterMaps();
            ConfigureEntityMaps();
            ConfigureAuditMaps();
            ConfigureFileMaps(this._imageConverter);

            DataMapper.CreateMap<Vocabulary, VocabularyViewModel>().AfterMap((vocabulary, model) =>
            {
                model.CanEdit = StrixPlatform.User.IsInRole(CmsRoleNames.EDITORROLES);
            });

            DataMapper.CreateMap<Term, TermViewModel>();
            var tagConfig = new MapConfig<IContent, EntityViewModel>();
            tagConfig.MembersToMap.Add(d => d.Tags, s => s.Entity.Tags);
            DataMapper.RegisterMapConfig(tagConfig);

            DataMapper.CreateMap<Comment, CommentViewModel>()
                .ForMember(cv => cv.CreatedBy, ce => ce.MapFrom(co => StrixCms.GetUserName(co.CreatedByUserId)))
                .ForMember(cv => cv.CreatedByEmail, ce => ce.MapFrom(co => StrixCms.GetUserEmail(co.CreatedByUserId)))
                .ForMember(cv => cv.UpdatedBy, ce => ce.MapFrom(co => StrixCms.GetUserName(co.UpdatedByUserId)));
            DataMapper.CreateMap<CommentViewModel, Comment>()
                .ForMember(co => co.CreatedByUserId, ce => ce.Ignore())
                .ForMember(co => co.CreatedOn, ce => ce.Ignore());

            DataMapper.CreateMap<File, FileDisplayModel>();
            DataMapper.CreateMap<FileDisplayModel, File>().ForAllMembers(mem => mem.Ignore());
            DataMapper.CreateMap<MailContentTemplate, MailContentTemplateViewModel>();

            ConfigureHtmlEncoding();
        }

        private static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Areas/Cms/Styles/css").Include(
                                        "~/Areas/Cms/Styles/cms*",
                                        "~/Areas/Cms/Styles/comments*",
                                        "~/Areas/Cms/Styles/tinymce*"));

            bundles.Add(new ScriptBundle("~/bundles/cms").Include(
                                         "~/Areas/Cms/Scripts/Controllers/strixit.*")
                                         .IncludeDirectory("~/Areas/Cms/Scripts/Directives", "*.js", true));
        }

        private static void ConfigureAuditMaps()
        {
            Action<object, object> auditAction = (x, y) =>
            {
                var audit = x as IAudit;
                var model = y as AuditViewModel;
                model.CanEdit = StrixPlatform.User.IsInRole(CmsRoleNames.EDITORROLES) || model.CreatedByUserId == StrixPlatform.User.Id;
                model.UpdatedBy = StrixCms.GetUserName(audit.UpdatedByUserId);
                model.CreatedBy = StrixCms.GetUserName(audit.CreatedByUserId);

                var content = x as IContent;

                if (content != null)
                {
                    if (model is EntityViewModel && content.PublishedByUserId.HasValue)
                    {
                        ((EntityViewModel)model).PublishedBy = StrixCms.GetUserName(content.PublishedByUserId.Value);
                    }
                }
            };

            var listAuditConfig = new MapConfig<AuditViewModel, AuditViewModel>();
            listAuditConfig.AfterMapAction = auditAction;
            DataMapper.RegisterMapConfig(listAuditConfig);

            var entityAuditConfig = new MapConfig<IContent, AuditViewModel>();
            entityAuditConfig.AfterMapAction = auditAction;
            DataMapper.RegisterMapConfig(entityAuditConfig);

            var auditReturnConfig = new MapConfig<EntityViewModel, IContent>();
            auditReturnConfig.MembersToIgnore.Add(d => d.IsCurrentVersion);
            auditReturnConfig.MembersToIgnore.Add(d => d.CreatedByUserId);
            auditReturnConfig.MembersToIgnore.Add(d => d.CreatedOn);
            auditReturnConfig.MembersToIgnore.Add(d => d.UpdatedByUserId);
            auditReturnConfig.MembersToIgnore.Add(d => d.UpdatedOn);
            DataMapper.RegisterMapConfig(auditReturnConfig);
        }

        private static void ConfigureFileMaps(IImageConverter imageConverter)
        {
            // Map a file to a base64 string if available.
            var fileConfig = new MapConfig<object, object>();

            fileConfig.AfterMapAction = (content, model) =>
            {
                var fileProperties = model.GetType().GetProperties().Where(pr => pr.GetCustomAttributes(typeof(ImageAttribute), true).Any());

                foreach (var fileProperty in fileProperties)
                {
                    var file = model.GetPropertyValue(fileProperty.Name) as FileDisplayModel;
                    var attribute = fileProperty.GetAttribute<ImageAttribute>();
                    var sizeX = string.IsNullOrWhiteSpace(attribute.WidthProperty) ? attribute.Width : (int)model.GetPropertyValue(attribute.WidthProperty);
                    var sizeY = string.IsNullOrWhiteSpace(attribute.HeightProperty) ? attribute.Height : (int)model.GetPropertyValue(attribute.HeightProperty);
                    string path = null;

                    if (file != null && file.Folder != null)
                    {
                        var imagePath = string.Format("{0}.{1}", System.IO.Path.Combine(file.Folder, file.Path, file.FileName), file.Extension);
                        path = imageConverter.ImageAsPath(imagePath, sizeX, sizeY, attribute.KeepAspectRatio);
                    }

                    if (model.HasProperty(fileProperty.Name + "Path"))
                    {
                        model.SetPropertyValue(fileProperty.Name + "Path", path);
                    }
                }
            };

            DataMapper.RegisterMapConfig(fileConfig);
        }

        private static void ConfigureHtmlEncoding()
        {
            Action<object, object> decodingAction = (x, y) =>
            {
                object source = x as IContent == null ? x as Comment == null ? null : (object)(x as Comment) : (object)(x as IContent);
                var target = y as EntityViewModel == null ? y as CommentViewModel == null ? null : (object)(y as CommentViewModel) : (object)(y as EntityViewModel);

                if (source != null && target != null)
                {
                    foreach (var allowHtmlProperty in target.GetType().GetProperties().Where(p => p.HasAttribute<AllowHtmlAttribute>()))
                    {
                        var decodedValue = Web.Helpers.HtmlDecode((string)source.GetPropertyValue(allowHtmlProperty.Name), false);
                        target.SetPropertyValue(allowHtmlProperty.Name, decodedValue);
                    }
                }
            };

            var decodeEntityConfig = new MapConfig<IContent, EntityViewModel>();
            decodeEntityConfig.AfterMapAction = decodingAction;
            DataMapper.RegisterMapConfig(decodeEntityConfig);

            var decodeCommentConfig = new MapConfig<Comment, CommentViewModel>();
            decodeCommentConfig.AfterMapAction = decodingAction;
            DataMapper.RegisterMapConfig(decodeCommentConfig);

            Action<object, object> encodingAction = (x, y) =>
            {
                var source = x as EntityViewModel == null ? x as CommentViewModel == null ? null : (object)(x as CommentViewModel) : (object)(x as EntityViewModel);
                object target = y as IContent == null ? y as Comment == null ? null : (object)(y as Comment) : (object)(y as IContent);

                if (source != null && target != null)
                {
                    foreach (var allowHtmlProperty in source.GetType().GetProperties().Where(p => p.HasAttribute<AllowHtmlAttribute>()))
                    {
                        var encodedValue = Web.Helpers.HtmlEncode((string)source.GetPropertyValue(allowHtmlProperty.Name));
                        target.SetPropertyValue(allowHtmlProperty.Name, encodedValue);
                    }
                }
            };

            var encodeEntityConfig = new MapConfig<EntityViewModel, IContent>();
            encodeEntityConfig.AfterMapAction = encodingAction;
            DataMapper.RegisterMapConfig(encodeEntityConfig);

            var encodeCommentConfig = new MapConfig<CommentViewModel, Comment>();
            encodeCommentConfig.AfterMapAction = encodingAction;
            DataMapper.RegisterMapConfig(encodeCommentConfig);
        }

        private static void RegisterDefaultTokens()
        {
            ResourceManager manager = new ResourceManager(typeof(StrixIT.Platform.Modules.Cms.Resources.DefaultTokens));
            ResourceSet resources = manager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);
            IDictionaryEnumerator enumerator = resources.GetEnumerator();

            while (enumerator.MoveNext())
            {
                Tokenizer.RegisterToken(string.Format("[[{0}]]", enumerator.Key.ToString().ToUpper()), (string)enumerator.Value);
            }
        }

        private static void ConfigureFilterMaps()
        {
            DataFilter.RegisterFilterMap<IContent>(CreateNameMap("CreatedBy"));
            DataFilter.RegisterFilterMap<IContent>(CreateNameMap(CmsConstants.UPDATEDBY));
            DataFilter.RegisterFilterMap<IContent>(CreateTagsMap());
            DataFilter.RegisterFilterMap<IContent>(CreateCommentsMap());
            DataFilter.RegisterFilterMap<Document>(CreateExtensionMap());
            DataFilter.RegisterFilterMap<Document>(CreateFileSizeMap());
        }

        private static FilterSortMap CreateNameMap(string propertyName)
        {
            FilterSortMap nameMap = new FilterSortMap { FieldToMap = propertyName };

            nameMap.FilterMap = (method, input) =>
            {
                return string.Format(@"{0}User.Name.ToLower().{1}(""{2}"")", propertyName, method.ToString(), input);
            };

            nameMap.SortMap = (query, order) =>
            {
                query = query.OrderBy(string.Format(@"{0}User.Name {1}", propertyName, order));
                return query;
            };

            return nameMap;
        }

        private static FilterSortMap CreateTagsMap()
        {
            FilterSortMap tagMap = new FilterSortMap { FieldToMap = "Tags" };

            tagMap.FilterMap = (method, input) =>
            {
                return string.Format("Entity.Tags.Any(Name.ToLower().{0}(@0))", method.ToString());
            };

            return tagMap;
        }

        private static FilterSortMap CreateCommentsMap()
        {
            FilterSortMap tagMap = new FilterSortMap { FieldToMap = "Comments" };

            tagMap.FilterMap = (method, input) =>
            {
                return string.Format("Entity.Comments.Any(Text.ToLower().{0}(@0))", method.ToString());
            };

            return tagMap;
        }

        private static FilterSortMap CreateExtensionMap()
        {
            FilterSortMap extensionMap = new FilterSortMap { FieldToMap = "Extension" };

            extensionMap.FilterMap = (method, input) =>
            {
                return string.Format("File.Extension.ToLower().Equals(\"{0}\")", input.ToLower());
            };

            extensionMap.SortMap = (query, order) =>
            {
                query = query.OrderBy(string.Format(@"File.Extension {0}", order));
                return query;
            };

            return extensionMap;
        }

        private static FilterSortMap CreateFileSizeMap()
        {
            FilterSortMap sizeMap = new FilterSortMap { FieldToMap = "FileSize" };

            sizeMap.FilterMap = (method, input) =>
            {
                return string.Format("File.Size.Equals(\"{0}\")", input);
            };

            sizeMap.SortMap = (query, order) =>
            {
                query = query.OrderBy(string.Format(@"File.Size {0}", order));
                return query;
            };

            return sizeMap;
        }

        private void ConfigureTypeMaps(object sender, CreateMapEventArgs e)
        {
            if (typeof(IContent).IsAssignableFrom(e.Types.Key) && (typeof(EntityListModel).IsAssignableFrom(e.Types.Value) || typeof(EntityViewModel).IsAssignableFrom(e.Types.Value)))
            {
                // Ignore platform entities in mappings.
                var platformEntityCollections = e.Types.Key.GetProperties().Where(p => typeof(ICollection<PlatformEntity>).IsAssignableFrom(p.PropertyType));

                foreach (var prop in platformEntityCollections)
                {
                    var viewModelCollection = e.Types.Value.GetProperty(prop.Name);

                    if (viewModelCollection != null)
                    {
                        var arg = viewModelCollection.PropertyType.GenericTypeArguments[0];
                        DataMapper.CreateMap(arg, typeof(PlatformEntity));
                    }
                    else
                    {
                        if (e.Types.Value.HasProperty(prop.Name))
                        {
                            e.PropertiesToIgnore.Add(prop.Name);
                        }
                    }
                }
            }

            // Prevent overwriting the file properties.
            if (typeof(IContent).IsAssignableFrom(e.Types.Value))
            {
                var fileProperties = e.Types.Value.GetProperties().Where(pr => pr.GetCustomAttributes(typeof(FileUploadAttribute), true).Any());

                foreach (var property in fileProperties)
                {
                    e.PropertiesToIgnore.Add(property.Name);
                }
            }
        }
    }
}
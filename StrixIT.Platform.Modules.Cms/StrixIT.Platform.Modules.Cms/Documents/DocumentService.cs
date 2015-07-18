//-----------------------------------------------------------------------
// <copyright file="DocumentService.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class DocumentService : EntityService<DocumentViewModel>, IDocumentService
    {
        private IFileManager _fileManager;

        public DocumentService(IPlatformDataSource dataSource, IEntityManager entityManager, ITaxonomyManager taxonomyManager, IFileManager fileManager, ICacheService cache)
            : base(dataSource, entityManager, taxonomyManager, cache) 
        {
            this._fileManager = fileManager;
        }

        public override IEnumerable List(FilterOptions filter)
        {
            IEnumerable list = null;
            var documentTypeField = filter != null ? filter.ExtractField("DocumentType") : null;

            if (documentTypeField != null)
            {
                filter.TraverseListPropertyName = "EntityId";
                var extEnum = Enum.Parse(typeof(DocumentType), documentTypeField.Value);
                var extensions = this._fileManager.GetExtensions(extEnum.ToString());
                var culture = StrixPlatform.CurrentCultureCode.ToLower();
                list = this.Manager.Query<Document>().Where(d => d.Culture.ToLower() == culture && d.IsCurrentVersion && extensions.Contains(d.File.Extension.ToLower())).Filter(filter).Map<DocumentListModel>();
                return list;
            }
            else
            {
                list = base.List(filter);
            }

            foreach (var model in list.Cast<DocumentListModel>())
            {
                this.SetDocumentType(model);
            }

            return list;
        }

        public IList<DocumentViewModel> CreateMany(IList<DocumentViewModel> models)
        {
            if (models == null)
            {
                throw new ArgumentNullException("models");
            }

            var nextSortOrder = this.Manager.GetNextSortOrder<Document>();

            var results = new List<DocumentViewModel>();

            foreach (var model in models)
            {
                model.SortOrder = nextSortOrder;
                results.Add(this.Save(model, false).Map<DocumentViewModel>());
                nextSortOrder++;
            }

            this.SaveChanges();

            return results;
        }

        public DocumentViewModel GetForDownload(string url)
        {
            var document = this.Manager.Get<Document>(url, StrixPlatform.CurrentCultureCode, "File");

            if (document == null)
            {
                return null;
            }

            var filePath = string.Format("{0}.{1}", System.IO.Path.Combine(document.File.Folder, document.File.Path, document.File.FileName), document.File.Extension);

            var canAccess = this._fileManager.CheckAccessFile(filePath);

            if (!canAccess)
            {
                return null;
            }

            document.DownloadCount++;
            this.SaveChanges();
            Logger.LogToAnalytics("DocumentDownload", document.Id.ToString());
            return document.Map<DocumentViewModel>();
        }

        protected override EntityViewModel Get(Type modelType, Guid? id, string url, string culture, int versionNumber, string relationsToInclude, bool useFallBack = false)
        {
            var model = base.Get(modelType, id, url, culture, versionNumber, relationsToInclude, useFallBack) as DocumentViewModel;
            this.SetDocumentType(model);
            return model;
        }

        private void SetDocumentType(dynamic model)
        {
            if (model != null && model.File != null)
            {
                model.DocumentType = this._fileManager.GetDocumentType(model.File.Extension);
            }
        }
    }
}
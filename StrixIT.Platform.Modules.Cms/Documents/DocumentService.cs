#region Apache License

//-----------------------------------------------------------------------
// <copyright file="DocumentService.cs" company="StrixIT">
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StrixIT.Platform.Modules.Cms
{
    public class DocumentService : EntityService<DocumentViewModel>, IDocumentService
    {
        #region Private Fields

        private IFileManager _fileManager;

        #endregion Private Fields

        #region Public Constructors

        public DocumentService(IPlatformDataSource dataSource, IEntityManager entityManager, ITaxonomyManager taxonomyManager, IFileManager fileManager, ICacheService cache)
            : base(dataSource, entityManager, taxonomyManager, cache)
        {
            this._fileManager = fileManager;
        }

        #endregion Public Constructors

        #region Public Methods

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

        #endregion Public Methods

        #region Protected Methods

        protected override EntityViewModel Get(Type modelType, Guid? id, string url, string culture, int versionNumber, string relationsToInclude, bool useFallBack = false)
        {
            var model = base.Get(modelType, id, url, culture, versionNumber, relationsToInclude, useFallBack) as DocumentViewModel;
            this.SetDocumentType(model);
            return model;
        }

        #endregion Protected Methods

        #region Private Methods

        private void SetDocumentType(dynamic model)
        {
            if (model != null && model.File != null)
            {
                model.DocumentType = this._fileManager.GetDocumentType(model.File.Extension);
            }
        }

        #endregion Private Methods
    }
}
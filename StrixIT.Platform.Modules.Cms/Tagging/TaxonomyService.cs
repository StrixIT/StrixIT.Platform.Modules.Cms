#region Apache License

//-----------------------------------------------------------------------
// <copyright file="TaxonomyService.cs" company="StrixIT">
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
    public class TaxonomyService : ITaxonomyService
    {
        #region Private Fields

        private ITaxonomyManager _manager;
        private IPlatformDataSource _source;

        #endregion Private Fields

        #region Public Constructors

        public TaxonomyService(IPlatformDataSource dataSource, ITaxonomyManager manager)
        {
            this._source = dataSource;
            this._manager = manager;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Delete(Guid id)
        {
            this.Delete(id, true);
        }

        public void Delete(Guid id, bool saveChanges)
        {
            this._manager.DeleteVocabulary(id);

            if (saveChanges)
            {
                this._source.SaveChanges();
            }
        }

        public void DeleteTag(Guid id)
        {
            this._manager.DeleteTag(id);
            this._source.SaveChanges();
        }

        public bool Exists(string name, Guid? id)
        {
            var query = this._manager.VocabularyQuery().Where(v => v.Name.ToLower() == name.ToLower());

            if (id.HasValue && id.Value != Guid.Empty)
            {
                query = query.Where(t => t.Id != id.Value);
            }

            return query.Any();
        }

        public VocabularyViewModel Get(Guid? id)
        {
            VocabularyViewModel model;

            if (!id.HasValue)
            {
                model = new VocabularyViewModel();
            }
            else
            {
                model = this._manager.GetVocabulary(id.Value).Map<VocabularyViewModel>();
            }

            return model;
        }

        public IList<TermViewModel> GetAllTags()
        {
            return this._manager.TagQuery().Map<TermViewModel>().ToList();
        }

        public TermViewModel GetTag(Guid id)
        {
            return this._manager.GetTag(id).Map<TermViewModel>();
        }

        public IList<TermViewModel> GetTagList(FilterOptions filter, Guid vocabularyId)
        {
            return this._manager.TagQueryForVocabulary(vocabularyId).Filter(filter).Map<TermViewModel>().ToList();
        }

        public IList<TermViewModel> GetTagList(FilterOptions filter, string vocabularyName)
        {
            return this._manager.TagQueryForVocabulary(vocabularyName, null).Filter(filter).Map<TermViewModel>().ToList();
        }

        public VocabularyViewModel GetVocabulary(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return new VocabularyViewModel();
            }

            return this._manager.GetVocabularyByUrl(url).Map<VocabularyViewModel>();
        }

        public IEnumerable List(FilterOptions filter)
        {
            return this._manager.VocabularyQuery().Filter(filter).Map<VocabularyViewModel>().ToList();
        }

        public SaveResult<VocabularyViewModel> Save(VocabularyViewModel model)
        {
            return this.Save(model, true);
        }

        public SaveResult<VocabularyViewModel> Save(VocabularyViewModel model, bool saveChanges)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            var result = new SaveResult<VocabularyViewModel>();
            Vocabulary vocabulary;

            if (model.Id == Guid.Empty)
            {
                vocabulary = this._manager.CreateVocabulary(model.Name, model.Culture);
            }
            else
            {
                vocabulary = this._manager.GetVocabulary(model.Id);
                model.Map<Vocabulary>(vocabulary);
            }

            result.Success = vocabulary != null;

            if (result.Success && saveChanges)
            {
                this._source.SaveChanges();
            }

            result.Entity = vocabulary;
            return result;
        }

        public SaveResult<TermViewModel> SaveTag(TermViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            var result = new SaveResult<TermViewModel>();
            Term term;

            if (model.Id == Guid.Empty)
            {
                term = this._manager.CreateTag(model.VocabularyId, model.Name);
            }
            else
            {
                term = this._manager.GetTag(model.Id);
                model.Map<Term>(term);
            }

            result.Success = term != null;

            if (result.Success)
            {
                this._source.SaveChanges();
            }

            result.Entity = term;
            return result;
        }

        #endregion Public Methods
    }
}
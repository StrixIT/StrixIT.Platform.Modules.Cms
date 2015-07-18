//-----------------------------------------------------------------------
// <copyright file="TaxonomyService.cs" company="StrixIT">
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
    public class TaxonomyService : ITaxonomyService
    {
        private IPlatformDataSource _source;

        private ITaxonomyManager _manager;

        public TaxonomyService(IPlatformDataSource dataSource, ITaxonomyManager manager)
        {
            this._source = dataSource;
            this._manager = manager;
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

        public IList<TermViewModel> GetAllTags()
        {
            return this._manager.TagQuery().Map<TermViewModel>().ToList();
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

        public void DeleteTag(Guid id)
        {
            this._manager.DeleteTag(id);
            this._source.SaveChanges();
        }

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
    }
}
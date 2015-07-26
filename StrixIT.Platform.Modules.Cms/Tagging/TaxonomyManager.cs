#region Apache License

//-----------------------------------------------------------------------
// <copyright file="TaxonomyManager.cs" company="StrixIT">
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
using StrixIT.Platform.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace StrixIT.Platform.Modules.Cms
{
    public class TaxonomyManager : ITaxonomyManager
    {
        #region Private Fields

        private IPlatformDataSource _dataSource;

        #endregion Private Fields

        #region Public Constructors

        public TaxonomyManager(IPlatformDataSource dataSource)
        {
            this._dataSource = dataSource;
        }

        #endregion Public Constructors

        #region Tag

        public bool Tag(IContent entity, IEnumerable<Guid> tagIds)
        {
            return this.Tag(entity, null, tagIds);
        }

        public bool Tag(Guid entityId, Guid termId)
        {
            return this.Tag(null, entityId, new Guid[] { termId });
        }

        public bool Tag(Guid entityId, IEnumerable<Guid> tagIds)
        {
            return this.Tag(null, entityId, tagIds);
        }

        public bool Tag(Guid entityId, string vocabularyName, string termName)
        {
            return this.Tag(entityId, vocabularyName, termName, null);
        }

        public bool Tag(Guid entityId, string vocabularyName, string termName, string culture)
        {
            if (entityId == Guid.Empty || string.IsNullOrWhiteSpace(vocabularyName) || string.IsNullOrWhiteSpace(termName))
            {
                throw new ArgumentException("Please supply an entity id and both a vocabulary name and tag name");
            }

            if (string.IsNullOrWhiteSpace(culture))
            {
                culture = StrixPlatform.DefaultCultureCode;
            }

            bool result = true;

            // See if the entity is tagged already.
            var entity = this._dataSource.Query<PlatformEntity>().Include("Tags.Vocabulary").Where(e => e.Id == entityId).FirstOrDefault();

            if (entity == null)
            {
                throw new InvalidOperationException(string.Format("No entity with id {0} found", entityId));
            }

            if (!entity.Tags.Any(GetTermFunc(vocabularyName, termName, culture).Compile()))
            {
                var term = this.GetTag(vocabularyName, termName);
                entity.Tags.Add(term);
                term.TagCount++;
            }

            return result;
        }

        #endregion Tag

        #region Get Tags

        public Term GetTag(Guid id)
        {
            return this._dataSource.Query<Term>().FirstOrDefault(t => t.Id == id);
        }

        public Term GetTag(string vocabularyName, string tagName)
        {
            return this.GetTag(vocabularyName, tagName, null, null);
        }

        public Term GetTag(string vocabularyName, string tagName, string culture)
        {
            return this.GetTag(vocabularyName, tagName, culture, null);
        }

        public Term GetTag(string vocabularyName, string tagName, string culture, string include)
        {
            if (string.IsNullOrWhiteSpace(vocabularyName) || string.IsNullOrWhiteSpace(tagName))
            {
                Logger.Log("No vocabulary name or term name specified for TaxonomyRepository.GetTag", LogLevel.Error);
                return null;
            }

            if (string.IsNullOrWhiteSpace(culture))
            {
                culture = StrixPlatform.DefaultCultureCode;
            }

            // Try to get the tag from the database.
            var term = this._dataSource.Query<Term>(include).FirstOrDefault(GetTermFunc(vocabularyName, tagName, culture));

            if (term == null)
            {
                term = this.CreateTag(vocabularyName, tagName);
                term.TaggedFiles = new List<File>();
            }

            return term;
        }

        public IQueryable<Term> TagQuery()
        {
            return this.TagQueryForVocabulary(null, null);
        }

        public IQueryable<Term> TagQuery(string culture)
        {
            return this.TagQueryForVocabulary(null, culture);
        }

        public IQueryable<Term> TagQueryForVocabulary(Guid vocabularyId)
        {
            if (vocabularyId == Guid.Empty)
            {
                throw new ArgumentNullException("vocabularyId");
            }

            return this._dataSource.Query<Vocabulary>().Where(v => v.Id == vocabularyId).SelectMany(v => v.Terms);
        }

        public IQueryable<Term> TagQueryForVocabulary(string vocabularyName)
        {
            return this.TagQueryForVocabulary(vocabularyName, null);
        }

        public IQueryable<Term> TagQueryForVocabulary(string vocabularyName, string culture)
        {
            if (string.IsNullOrWhiteSpace(culture))
            {
                culture = StrixPlatform.DefaultCultureCode;
            }

            bool vocabularySpecified = !string.IsNullOrWhiteSpace(vocabularyName);
            return this._dataSource.Query<Vocabulary>().Where(v => v.Culture.ToLower() == culture.ToLower()
                                                                   && ((!vocabularySpecified && !v.IsSystemVocabulary)
                                                                        || v.Name.ToLower() == vocabularyName.ToLower())).SelectMany(v => v.Terms);
        }

        #endregion Get Tags

        #region Create Tags

        public Term CreateTag(Guid vocabularyId, string tagName)
        {
            var vocabulary = this.GetVocabulary(vocabularyId);
            return this.CreateTag(vocabulary.Name, tagName, vocabulary.Culture);
        }

        public Term CreateTag(string vocabularyName, string tagName)
        {
            return this.CreateTag(vocabularyName, tagName, null);
        }

        public Term CreateTag(string vocabularyName, string tagName, string culture)
        {
            if (string.IsNullOrWhiteSpace(vocabularyName) || string.IsNullOrWhiteSpace(tagName))
            {
                throw new ArgumentException("Please supply both a vocabulary name and tag name");
            }

            if (string.IsNullOrWhiteSpace(culture))
            {
                culture = StrixPlatform.DefaultCultureCode;
            }

            var term = this._dataSource.Query<Term>().FirstOrDefault(GetTermFunc(vocabularyName, tagName, culture));

            if (term != null)
            {
                return term;
            }

            // Create the term if it does not exist yet.
            var vocabulary = this.GetVocabulary(vocabularyName, culture);
            var id = Guid.NewGuid();

            term = new Term
            {
                Id = id,
                Name = tagName,
                Url = UrlHelpers.CreateUniqueUrl(this._dataSource.Query<Term>(), tagName, id),
                VocabularyId = vocabulary.Id
            };

            this._dataSource.Save(term);
            vocabulary.Terms.Add(term);

            return term;
        }

        #endregion Create Tags

        #region Get Vocabulary

        public Vocabulary GetVocabulary(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("id");
            }

            return this._dataSource.Query<Vocabulary>().FirstOrDefault(vo => vo.Id == id);
        }

        public Vocabulary GetVocabulary(string vocabularyName)
        {
            return this.GetVocabulary(vocabularyName, null, false);
        }

        public Vocabulary GetVocabulary(string vocabularyName, string culture)
        {
            return this.GetVocabulary(vocabularyName, culture, false);
        }

        public Vocabulary GetVocabulary(string vocabularyName, string culture, bool includeTerms)
        {
            if (string.IsNullOrWhiteSpace(vocabularyName))
            {
                throw new ArgumentNullException("vocabularyName");
            }

            if (string.IsNullOrWhiteSpace(culture))
            {
                culture = StrixPlatform.DefaultCultureCode;
            }

            var query = this._dataSource.Query<Vocabulary>();

            if (includeTerms)
            {
                query = query.Include(v => v.Terms);
            }

            var vocabulary = query.FirstOrDefault(GetVocabularyFunc(vocabularyName, culture));

            if (vocabulary == null)
            {
                vocabulary = this.CreateVocabulary(vocabularyName, culture);
            }

            return vocabulary;
        }

        public Vocabulary GetVocabularyByUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }

            return this._dataSource.Query<Vocabulary>().FirstOrDefault(vo => vo.Url.ToLower() == url.ToLower());
        }

        public IQueryable<Vocabulary> VocabularyQuery()
        {
            return this._dataSource.Query<Vocabulary>().Where(v => !v.IsSystemVocabulary);
        }

        #endregion Get Vocabulary

        #region Create Vocabulary

        public Vocabulary CreateVocabulary(string vocabularyName)
        {
            return this.CreateVocabulary(vocabularyName, null);
        }

        public Vocabulary CreateVocabulary(string vocabularyName, string culture)
        {
            if (string.IsNullOrWhiteSpace(vocabularyName))
            {
                throw new ArgumentNullException("vocabularyName");
            }

            if (string.IsNullOrWhiteSpace(culture))
            {
                culture = StrixPlatform.DefaultCultureCode;
            }

            var vocabulary = this._dataSource.Query<Vocabulary>().FirstOrDefault(GetVocabularyFunc(vocabularyName, culture));

            if (vocabulary != null)
            {
                return vocabulary;
            }

            var isSystem = Enum.GetNames(typeof(CoreVocabulary)).ToLower().Contains(vocabularyName.ToLower());
            var id = Guid.NewGuid();

            vocabulary = new Vocabulary
            {
                Id = id,
                GroupId = StrixPlatform.User.GroupId,
                Culture = culture,
                Name = vocabularyName,
                Url = UrlHelpers.CreateUniqueUrl(this._dataSource.Query<Vocabulary>(), vocabularyName, id),
                IsSystemVocabulary = isSystem,
                Terms = new List<Term>(),
            };

            this._dataSource.Save(vocabulary);
            return vocabulary;
        }

        #endregion Create Vocabulary

        #region Delete Tags

        public void DeleteTag(Guid id)
        {
            var tag = this._dataSource.Query<Term>().Where(t => t.Id == id);
            this._dataSource.Delete(tag);
        }

        public void DeleteTags(Guid entityId)
        {
            var tags = this._dataSource.Query<PlatformEntity>().Where(t => t.Id == entityId).Select(t => t.Tags).ToList();
            var result = true;

            foreach (var tag in tags)
            {
                this._dataSource.Delete(tag);

                if (!result)
                {
                    break;
                }
            }
        }

        #endregion Delete Tags

        #region Delete Vocabulary

        public void DeleteVocabulary(Guid id)
        {
            var vocabulary = this._dataSource.Query<Vocabulary>().Where(v => v.Id == id);
            this._dataSource.Delete(vocabulary);
        }

        #endregion Delete Vocabulary

        #region Tag Files

        public void AddTagToFiles(string tag, string[] fileNames)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                throw new ArgumentNullException("tag");
            }

            if (fileNames.IsEmpty())
            {
                return;
            }

            var theTag = this.GetTag(CoreVocabulary.FileUse.ToString(), tag);

            // Save the tag to the data source to reset its deleted status, in case the tag is
            // marked as deleted.
            this._dataSource.Save(theTag);

            var files = this._dataSource.Query<File>().Include(f => f.Tags).Where(f => fileNames.Contains(f.FileName.ToLower())).ToList();

            foreach (var name in fileNames)
            {
                var file = files.FirstOrDefault(f => f.FileName.ToLower() == name.ToLower());

                if (file != null && !file.Tags.Any(t => t.Name.ToLower() == tag.ToLower()))
                {
                    file.Tags.Add(theTag);
                    theTag.TagCount++;
                }
            }
        }

        public void RemoveTagFromFiles(string tag, string[] fileNames)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                throw new ArgumentNullException("tag");
            }

            if (fileNames == null)
            {
                return;
            }

            var theTag = this.GetTag(CoreVocabulary.FileUse.ToString(), tag, null, "TaggedFiles.Tags");

            foreach (var name in fileNames)
            {
                if (theTag.TaggedFiles.Any(f => f.FileName == name))
                {
                    var file = theTag.TaggedFiles.First(f => f.FileName.ToLower() == name.ToLower());
                    theTag.TaggedFiles.Remove(file);
                    theTag.TagCount--;

                    // If there are no more files tagged with this tag, remove it.
                    if (theTag.TagCount == 0)
                    {
                        this._dataSource.Delete(theTag);
                    }
                }
            }
        }

        #endregion Tag Files

        #region Private Functions

        /// <summary>
        /// Gets the expression to filter using a vocabulary and tag name and vocabulary culture.
        /// </summary>
        /// <param name="vocabularyName">The vocabulary name to filter on</param>
        /// <param name="tagName">The tag name to filter on</param>
        /// <param name="culture">The vocabulary culture to filter on</param>
        /// <returns>The filter expression</returns>
        private static Expression<Func<Term, bool>> GetTermFunc(string vocabularyName, string tagName, string culture)
        {
            return t => t.Vocabulary.Name.ToLower() == vocabularyName.ToLower()
                        && t.Vocabulary.Culture.ToLower() == culture.ToLower()
                        && t.Name.ToLower() == tagName.ToLower();
        }

        /// <summary>
        /// Gets the expression to filter using a vocabulary name and culture.
        /// </summary>
        /// <param name="vocabularyName">The vocabulary name to filter on</param>
        /// <param name="culture">The vocabulary culture to filter on</param>
        /// <returns>The filter expression</returns>
        private static Expression<Func<Vocabulary, bool>> GetVocabularyFunc(string vocabularyName, string culture)
        {
            return v => v.Name.ToLower() == vocabularyName.ToLower()
                        && v.Culture.ToLower() == culture.ToLower();
        }

        private bool Tag(IContent content, Guid? entityId, IEnumerable<Guid> tagIds)
        {
            if (entityId == Guid.Empty || tagIds.IsEmpty())
            {
                throw new ArgumentException("Please supply both an entity id and one or more tag ids");
            }

            PlatformEntity entity;

            if (content == null)
            {
                entity = this._dataSource.Query<PlatformEntity>().Include(e => e.Tags).FirstOrDefault(e => e.Id == entityId);
            }
            else
            {
                entity = content.Entity;

                if (entity.Tags == null)
                {
                    entity.Tags = new List<Term>();
                }
            }

            var tags = this._dataSource.Query<Term>().Where(t => tagIds.Contains(t.Id)).ToList();

            foreach (var id in tagIds)
            {
                if (!entity.Tags.Any(t => t.Id == id))
                {
                    var tag = tags.First(t => t.Id == id);
                    entity.Tags.Add(tag);
                    tag.TagCount++;
                }
            }

            return true;
        }

        #endregion Private Functions
    }
}
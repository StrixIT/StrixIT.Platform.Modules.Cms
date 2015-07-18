//-----------------------------------------------------------------------
// <copyright file="ITaxonomyManager.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface defining the platform's Taxonomy repository.
    /// </summary>
    public interface ITaxonomyManager
    {
        #region Tag

        /// <summary>
        /// Tags an entity.
        /// </summary>
        /// <param name="entityId">The entity id</param>
        /// <param name="termId">The tag id</param>
        /// <returns>True if tagging was successful, false otherwise</returns>
        bool Tag(Guid entityId, Guid termId);

        /// <summary>
        /// Tags an entity with multiple tags.
        /// </summary>
        /// <param name="entity">The entity to tag</param>
        /// <param name="tagIds">The tag ids</param>
        /// <returns>True if tagging was successful, false otherwise</returns>
        bool Tag(IContent entity, IEnumerable<Guid> tagIds);

        /// <summary>
        /// Tags an entity with multiple tags.
        /// </summary>
        /// <param name="entityId">The entity id</param>
        /// <param name="tagIds">The tag ids</param>
        /// <returns>True if tagging was successful, false otherwise</returns>
        bool Tag(Guid entityId, IEnumerable<Guid> tagIds);

        /// <summary>
        /// Tags an entity.
        /// </summary>
        /// <param name="entityId">The entity id</param>
        /// <param name="vocabularyName">The name of the Vocabulary the tag is part of</param>
        /// <param name="termName">The tag</param>
        /// <returns>True if tagging was successful, false otherwise</returns>
        bool Tag(Guid entityId, string vocabularyName, string termName);

        /// <summary>
        /// Tags an entity.
        /// </summary>
        /// <param name="entityId">The entity id</param>
        /// <param name="vocabularyName">The name of the Vocabulary the tag is part of</param>
        /// <param name="termName">The tag</param>
        /// <param name="culture">The vocabulary culture</param>
        /// <returns>True if tagging was successful, false otherwise</returns>
        bool Tag(Guid entityId, string vocabularyName, string termName, string culture);

        #endregion

        #region Get Tags

        /// <summary>
        /// Gets a term by its id.
        /// </summary>
        /// <param name="id">The term id</param>
        /// <returns>The term</returns>
        Term GetTag(Guid id);

        /// <summary>
        /// Gets a tag for a vocabulary.
        /// </summary>
        /// <param name="vocabularyName">The name of the vocabulary to get the tag for</param>
        /// <param name="tagName">The tag name</param>
        /// <returns>The tag</returns>
        Term GetTag(string vocabularyName, string tagName);

        /// <summary>
        /// Gets a tag for a vocabulary.
        /// </summary>
        /// <param name="vocabularyName">The name of the vocabulary to get the tag for</param>
        /// <param name="tagName">The tag name</param>
        /// <param name="culture">The vocabulary culture</param>
        /// <returns>The tag</returns>
        Term GetTag(string vocabularyName, string tagName, string culture);

        /// <summary>
        /// Gets a tag for a vocabulary.
        /// </summary>
        /// <param name="vocabularyName">The name of the vocabulary to get the tag for</param>
        /// <param name="tagName">The tag name</param>
        /// <param name="culture">The vocabulary culture</param>
        /// <param name="include">The include to get for the tag</param>
        /// <returns>The tag</returns>
        Term GetTag(string vocabularyName, string tagName, string culture, string include);

        /// <summary>
        /// Gets a query for all the non-system tags available.
        /// </summary>
        /// <returns>A query for all non-system tags</returns>
        IQueryable<Term> TagQuery();

        /// <summary>
        /// Gets a query for all the non-system tags available.
        /// </summary>
        /// <param name="culture">The culture to get the tags for</param>
        /// <returns>A query for all non-system tags</returns>
        IQueryable<Term> TagQuery(string culture);

        /// <summary>
        /// Gets a query for the tags of a vocabulary.
        /// </summary>
        /// <param name="vocabularyId">The id of the vocabulary to get the tag query for</param>
        /// <returns>The tag query</returns>
        IQueryable<Term> TagQueryForVocabulary(Guid vocabularyId);

        /// <summary>
        /// Gets a query for the tags of a vocabulary.
        /// </summary>
        /// <param name="vocabularyName">The name of the vocabulary to get the tag query for</param>
        /// <returns>The tag query</returns>
        IQueryable<Term> TagQueryForVocabulary(string vocabularyName);

        /// <summary>
        /// Gets a query for the tags of a vocabulary.
        /// </summary>
        /// <param name="vocabularyName">The name of the vocabulary to get the tag query for</param>
        /// <param name="culture">The culture to get the tags for</param>
        /// <returns>The tag query</returns>
        IQueryable<Term> TagQueryForVocabulary(string vocabularyName, string culture);

        #endregion

        #region Create Tag

        /// <summary>
        /// Creates a tag for a vocabulary.
        /// </summary>
        /// <param name="vocabularyId">The id of the vocabulary to create the tag for</param>
        /// <param name="tagName">The tag name</param>
        /// <returns>The tag</returns>
        Term CreateTag(Guid vocabularyId, string tagName);

        /// <summary>
        /// Creates a tag for a vocabulary.
        /// </summary>
        /// <param name="vocabularyName">The name of the vocabulary to create the tag for</param>
        /// <param name="tagName">The tag name</param>
        /// <returns>The tag</returns>
        Term CreateTag(string vocabularyName, string tagName);

        #endregion

        #region Get Vocabulary

        /// <summary>
        /// Gets a vocabulary by its id.
        /// </summary>
        /// <param name="id">The vocabulary id</param>
        /// <returns>The vocabulary</returns>
        Vocabulary GetVocabulary(Guid id);

        /// <summary>
        /// Gets a vocabulary by its url.
        /// </summary>
        /// <param name="url">The vocabulary url</param>
        /// <returns>The vocabulary</returns>
        Vocabulary GetVocabularyByUrl(string url);

        /// <summary>
        /// Gets a vocabulary by its name.
        /// </summary>
        /// <param name="vocabularyName">The vocabulary name</param>
        /// <returns>The vocabulary</returns>
        Vocabulary GetVocabulary(string vocabularyName);

        /// <summary>
        /// Gets a vocabulary by its name and culture.
        /// </summary>
        /// <param name="vocabularyName">The vocabulary name</param>
        /// <param name="culture">The vocabulary culture</param>
        /// <returns>The vocabulary</returns>
        Vocabulary GetVocabulary(string vocabularyName, string culture);

        /// <summary>
        /// Gets a vocabulary by its name and culture, including the terms if required.
        /// </summary>
        /// <param name="vocabularyName">The vocabulary name</param>
        /// <param name="culture">The vocabulary culture</param>
        /// <param name="includeTerms">True if the vocabulary terms should be included, false otherwise</param>
        /// <returns>The vocabulary</returns>
        Vocabulary GetVocabulary(string vocabularyName, string culture, bool includeTerms);

        /// <summary>
        /// Gets a query of all non-system vocabularies.
        /// </summary>
        /// <returns>The vocabulary query</returns>
        IQueryable<Vocabulary> VocabularyQuery();

        #endregion

        #region Create Vocabulary

        /// <summary>
        /// Creates a new vocabulary.
        /// </summary>
        /// <param name="vocabularyName">The name of the vocabulary</param>
        /// <returns>The vocabulary</returns>
        Vocabulary CreateVocabulary(string vocabularyName);

        /// <summary>
        /// Creates a new vocabulary.
        /// </summary>
        /// <param name="vocabularyName">The name of the vocabulary</param>
        /// <param name="culture">The vocabulary culture</param>
        /// <returns>The vocabulary</returns>
        Vocabulary CreateVocabulary(string vocabularyName, string culture);

        #endregion

        #region Delete Tags

        /// <summary>
        /// Deletes a tag.
        /// </summary>
        /// <param name="id">The tag id</param>
        void DeleteTag(Guid id);

        /// <summary>
        /// Deletes all tags for an entity from the data source.
        /// </summary>
        /// <param name="entityId">The entity platform id</param>
        void DeleteTags(Guid entityId);

        #endregion

        #region Delete Vocabulary

        /// <summary>
        /// Deletes a vocabulary.
        /// </summary>
        /// <param name="id">The vocabulary id</param>
        void DeleteVocabulary(Guid id);

        #endregion

        #region Tag Files

        /// <summary>
        /// Adds the specified tag to the specified files.
        /// </summary>
        /// <param name="tag">The tag</param>
        /// <param name="fileNames">The names of the files to add the tags to</param>
        void AddTagToFiles(string tag, string[] fileNames);

        /// <summary>
        /// Removes the specified tag from the specified files.
        /// </summary>
        /// <param name="tag">The tag</param>
        /// <param name="fileNames">The names of the files to remove the tags from</param>
        void RemoveTagFromFiles(string tag, string[] fileNames);

        #endregion
    }
}
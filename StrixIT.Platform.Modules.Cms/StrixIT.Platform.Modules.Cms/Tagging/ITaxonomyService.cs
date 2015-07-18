//-----------------------------------------------------------------------
// <copyright file="ITaxonomyService.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The taxonomy service interface for the platform.
    /// </summary>
    public interface ITaxonomyService : IObjectService<Guid, VocabularyViewModel>
    {
        /// <summary>
        /// Gets a tag by its id.
        /// </summary>
        /// <param name="id">The tag id</param>
        /// <returns>The tag</returns>
        TermViewModel GetTag(Guid id);

        /// <summary>
        /// Gets a list of tags for the specified vocabulary using the data manager parameters.
        /// </summary>
        /// <param name="filter">The data manager parameters to use</param>
        /// <param name="vocabularyId">The id of the vocabulary to get the tags for</param>
        /// <returns>The list of tags</returns>
        IList<TermViewModel> GetTagList(FilterOptions filter, Guid vocabularyId);

        /// <summary>
        /// Gets a list of tags for the specified vocabulary using the data manager parameters.
        /// </summary>
        /// <param name="filter">The data manager parameters to use</param>
        /// <param name="vocabularyName">The name of the vocabulary to get the tags for</param>
        /// <returns>The list of tags</returns>
        IList<TermViewModel> GetTagList(FilterOptions filter, string vocabularyName);

        IList<TermViewModel> GetAllTags();

        /// <summary>
        /// Gets a vocabulary by its url.
        /// </summary>
        /// <param name="url">The vocabulary url</param>
        /// <returns>The vocabulary</returns>
        VocabularyViewModel GetVocabulary(string url);

        /// <summary>
        /// Saves a tag.
        /// </summary>
        /// <param name="model">The tag view model</param>
        /// <returns>A save result</returns>
        SaveResult<TermViewModel> SaveTag(TermViewModel model);

        /// <summary>
        /// Deletes the tag with the specified id.
        /// </summary>
        /// <param name="id">The id of the tag to delete</param>
        void DeleteTag(Guid id);
    }
}
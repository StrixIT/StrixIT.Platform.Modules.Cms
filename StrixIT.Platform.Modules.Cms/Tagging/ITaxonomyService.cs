#region Apache License

//-----------------------------------------------------------------------
// <copyright file="ITaxonomyService.cs" company="StrixIT">
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

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The taxonomy service interface for the platform.
    /// </summary>
    public interface ITaxonomyService : IObjectService<Guid, VocabularyViewModel>
    {
        #region Public Methods

        /// <summary>
        /// Deletes the tag with the specified id.
        /// </summary>
        /// <param name="id">The id of the tag to delete</param>
        void DeleteTag(Guid id);

        IList<TermViewModel> GetAllTags();

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

        #endregion Public Methods
    }
}
#region Apache License

//-----------------------------------------------------------------------
// <copyright file="VocabularyViewModel.cs" company="StrixIT">
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

using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The view model for vocabularies.
    /// </summary>
    public class VocabularyViewModel : PlatformBaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the user can edit this view model.
        /// </summary>
        public bool CanEdit { get; set; }

        /// <summary>
        /// Gets or sets the vocabulary culture.
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the vocabulary id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the vocabulary name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the vocabulary url.
        /// </summary>
        public string Url { get; set; }

        #endregion Public Properties
    }
}
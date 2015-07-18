//-----------------------------------------------------------------------
// <copyright file="VocabularyViewModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The view model for vocabularies.
    /// </summary>
    public class VocabularyViewModel : PlatformBaseViewModel
    {
        /// <summary>
        /// Gets or sets the vocabulary id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the vocabulary culture.
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the vocabulary name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the vocabulary url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can edit this view model.
        /// </summary>
        public bool CanEdit { get; set; }
    }
}
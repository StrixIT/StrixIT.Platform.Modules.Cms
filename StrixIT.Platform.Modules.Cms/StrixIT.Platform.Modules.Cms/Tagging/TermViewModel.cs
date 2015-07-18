//-----------------------------------------------------------------------
// <copyright file="TermViewModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The term view model.
    /// </summary>
    public class TermViewModel
    {
        /// <summary>
        /// Gets or sets the term id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the term vocabulary id.
        /// </summary>
        public Guid VocabularyId { get; set; }

        /// <summary>
        /// Gets or sets the term name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the term url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tag is selected.
        /// </summary>
        public bool Selected { get; set; }
    }
}
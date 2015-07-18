//-----------------------------------------------------------------------
// <copyright file="Synonym.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A class to define synonyms for a Taxonomy Term.
    /// </summary>
    public class Synonym : ValidationBase
    {
        /// <summary>
        /// Gets or sets the Id of the Term this Synonym is for.
        /// </summary>
        [StrixRequired]
        public Guid TermId { get; set; }

        /// <summary>
        /// Gets or sets the Term this Synonym is for.
        /// </summary>
        public Term Term { get; set; }

        /// <summary>
        /// Gets or sets the name of the Entity.
        /// </summary>
        [StrixRequired]
        [StringLength(250, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
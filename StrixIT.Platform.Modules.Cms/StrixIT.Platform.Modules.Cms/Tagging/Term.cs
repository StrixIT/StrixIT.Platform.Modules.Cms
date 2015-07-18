//-----------------------------------------------------------------------
// <copyright file="Term.cs" company="StrixIT">
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
    /// A class used to define a Term in the Taxonomy system.
    /// </summary>
    public class Term : ValidationBase
    {
        /// <summary>
        /// Gets or sets the term id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the id of the vocabulary this term is a part of.
        /// </summary>
        [StrixRequired]
        public Guid VocabularyId { get; set; }

        /// <summary>
        /// Gets or sets the vocabulary this term is a part of.
        /// </summary>
        public Vocabulary Vocabulary { get; set; }

        /// <summary>
        /// Gets or sets the name of the Entity.
        /// </summary>
        [StrixRequired]
        [StringLength(250, MinimumLength = 2)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Url of the entity.
        /// </summary>
        [StrixRequired]
        [StringLength(300)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the number of Content objects tagged with this term.
        /// </summary>
        public int TagCount { get; set; }

        /// <summary>
        /// Gets or sets the entities tagged with this term.
        /// </summary>
        public ICollection<PlatformEntity> TaggedEntities { get; set; }

        /// <summary>
        /// Gets or sets the files tagged with this term.
        /// </summary>
        public ICollection<File> TaggedFiles { get; set; }

        /// <summary>
        /// Gets or sets the parents of this Term.
        /// </summary>
        public virtual ICollection<Term> Parents { get; set; }

        /// <summary>
        /// Gets or sets the children of this Term.
        /// </summary>
        public virtual ICollection<Term> Children { get; set; }

        /// <summary>
        /// Gets or sets the sibling terms of this Term.
        /// </summary>
        public virtual ICollection<Term> SiblingTerms { get; set; }

        /// <summary>
        /// Gets or sets the terms that have this Term as a sibling.
        /// </summary>
        public virtual ICollection<Term> OtherSiblingTerms { get; set; }

        /// <summary>
        /// Gets or sets the synomyms for this Term.
        /// </summary>
        public ICollection<Synonym> Synonyms { get; set; }
    }
}
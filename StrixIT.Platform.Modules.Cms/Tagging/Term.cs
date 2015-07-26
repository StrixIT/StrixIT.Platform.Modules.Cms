#region Apache License

//-----------------------------------------------------------------------
// <copyright file="Term.cs" company="StrixIT">
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
using System.ComponentModel.DataAnnotations;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A class used to define a Term in the Taxonomy system.
    /// </summary>
    public class Term : ValidationBase
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the children of this Term.
        /// </summary>
        public virtual ICollection<Term> Children { get; set; }

        /// <summary>
        /// Gets or sets the term id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the Entity.
        /// </summary>
        [StrixRequired]
        [StringLength(250, MinimumLength = 2)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the terms that have this Term as a sibling.
        /// </summary>
        public virtual ICollection<Term> OtherSiblingTerms { get; set; }

        /// <summary>
        /// Gets or sets the parents of this Term.
        /// </summary>
        public virtual ICollection<Term> Parents { get; set; }

        /// <summary>
        /// Gets or sets the sibling terms of this Term.
        /// </summary>
        public virtual ICollection<Term> SiblingTerms { get; set; }

        /// <summary>
        /// Gets or sets the synomyms for this Term.
        /// </summary>
        public ICollection<Synonym> Synonyms { get; set; }

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
        /// Gets or sets the Url of the entity.
        /// </summary>
        [StrixRequired]
        [StringLength(300)]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the vocabulary this term is a part of.
        /// </summary>
        public Vocabulary Vocabulary { get; set; }

        /// <summary>
        /// Gets or sets the id of the vocabulary this term is a part of.
        /// </summary>
        [StrixRequired]
        public Guid VocabularyId { get; set; }

        #endregion Public Properties
    }
}
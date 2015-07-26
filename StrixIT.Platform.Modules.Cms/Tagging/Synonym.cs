#region Apache License

//-----------------------------------------------------------------------
// <copyright file="Synonym.cs" company="StrixIT">
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
using System.ComponentModel.DataAnnotations;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A class to define synonyms for a Taxonomy Term.
    /// </summary>
    public class Synonym : ValidationBase
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the Entity.
        /// </summary>
        [StrixRequired]
        [StringLength(250, MinimumLength = 2)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Term this Synonym is for.
        /// </summary>
        public Term Term { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Term this Synonym is for.
        /// </summary>
        [StrixRequired]
        public Guid TermId { get; set; }

        #endregion Public Properties
    }
}
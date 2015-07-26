#region Apache License

//-----------------------------------------------------------------------
// <copyright file="CommentListModel.cs" company="StrixIT">
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
using System.Collections.Generic;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The list model for comments.
    /// </summary>
    public class CommentListModel
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the list of child comments of this comment.
        /// </summary>
        public IList<CommentViewModel> ChildComments { get; set; }

        /// <summary>
        /// Gets or sets the culture id of the entity the comment is for.
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the key of the entity the comment is for.
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the comment id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the current version number of the entity the comment is for.
        /// </summary>
        public int VersionNumber { get; set; }

        #endregion Public Properties
    }
}
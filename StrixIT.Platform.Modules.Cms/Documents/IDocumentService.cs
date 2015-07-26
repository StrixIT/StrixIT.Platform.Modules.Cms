#region Apache License

//-----------------------------------------------------------------------
// <copyright file="IDocumentService.cs" company="StrixIT">
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

using System.Collections.Generic;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The document service interface.
    /// </summary>
    public interface IDocumentService : IEntityService<DocumentViewModel>
    {
        #region Public Methods

        /// <summary>
        /// Creates many documents at once.
        /// </summary>
        /// <param name="models">The models to savr</param>
        /// <returns>The list of created document models</returns>
        IList<DocumentViewModel> CreateMany(IList<DocumentViewModel> models);

        /// <summary>
        /// Gets a document while updating its download count.
        /// </summary>
        /// <param name="url">The url of the document to get</param>
        /// <returns>The document</returns>
        DocumentViewModel GetForDownload(string url);

        #endregion Public Methods
    }
}
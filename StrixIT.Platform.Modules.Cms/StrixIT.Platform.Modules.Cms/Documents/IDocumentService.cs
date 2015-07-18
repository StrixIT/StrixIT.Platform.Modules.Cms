//-----------------------------------------------------------------------
// <copyright file="IDocumentService.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The document service interface.
    /// </summary>
    public interface IDocumentService : IEntityService<DocumentViewModel>
    {
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
    }
}
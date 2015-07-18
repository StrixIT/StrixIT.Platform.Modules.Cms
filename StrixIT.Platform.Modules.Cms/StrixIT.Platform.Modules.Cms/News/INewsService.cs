//-----------------------------------------------------------------------
// <copyright file="INewsService.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The news service interface for the Cms.
    /// </summary>
    public interface INewsService : IEntityService<NewsViewModel>
    {
        /// <summary>
        /// Gets the latest news.
        /// </summary>
        /// <returns>The latest news view model</returns>
        NewsViewModel GetLatest();
    }
}
//-----------------------------------------------------------------------
// <copyright file="IMailService.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The mail service interface for the Cms.
    /// </summary>
    public interface IMailService : IEntityService<MailContentViewModel>
    {
        /// <summary>
        /// Gets a mail content for the specified culture with the specified name.
        /// </summary>
        /// <param name="culture">The culture to get the mail conteht for</param>
        /// <param name="name">The content name to get</param>
        /// <returns>The mail content</returns>
        MailContent GetMailContent(string culture, string name);
    }
}
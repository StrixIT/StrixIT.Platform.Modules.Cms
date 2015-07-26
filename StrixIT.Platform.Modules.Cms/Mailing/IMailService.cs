#region Apache License
//-----------------------------------------------------------------------
// <copyright file="IMailService.cs" company="StrixIT">
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
#endregion

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
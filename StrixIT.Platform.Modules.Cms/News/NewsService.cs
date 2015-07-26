#region Apache License

//-----------------------------------------------------------------------
// <copyright file="NewsService.cs" company="StrixIT">
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
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace StrixIT.Platform.Modules.Cms
{
    public class NewsService : EntityService<NewsViewModel>, INewsService
    {
        #region Public Constructors

        public NewsService(IPlatformDataSource dataSource, IEntityManager entityManager, ITaxonomyManager taxonomyManager, ICacheService cache) : base(dataSource, entityManager, taxonomyManager, cache)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public NewsViewModel GetLatest()
        {
            var news = this.Manager.QueryCurrent<News>("Entity").OrderByDescending(n => n.PublishedOn).FirstOrDefault();
            return news.Map<NewsViewModel>();
        }

        public override System.Collections.IEnumerable List(FilterOptions filter)
        {
            var list = base.List(filter).Cast<NewsListModel>();

            foreach (var model in list)
            {
                model.Summary = GetSummary(model.Summary, model.Body);
            }

            return list;
        }

        #endregion Public Methods

        #region Private Methods

        private static string GetSummary(string summary, string body)
        {
            if (string.IsNullOrWhiteSpace(summary) && !string.IsNullOrWhiteSpace(body))
            {
                string cleanedText = string.Empty;
                var matches = Regex.Matches(HttpUtility.HtmlDecode(body), @"<([DPdp][A-Za-z0-9]*)\b[^>]*>(.*?)</\1>");

                foreach (var match in matches)
                {
                    if (cleanedText.Length <= 500)
                    {
                        cleanedText = cleanedText + match.ToString();
                    }
                    else
                    {
                        break;
                    }
                }

                return cleanedText;
            }
            else
            {
                return summary;
            }
        }

        #endregion Private Methods
    }
}
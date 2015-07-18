//-----------------------------------------------------------------------
// <copyright file="NewsService.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class NewsService : EntityService<NewsViewModel>, INewsService
    {
        public NewsService(IPlatformDataSource dataSource, IEntityManager entityManager, ITaxonomyManager taxonomyManager, ICacheService cache) : base(dataSource, entityManager, taxonomyManager, cache) { }

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
    }
}
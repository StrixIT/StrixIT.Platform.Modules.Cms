#region Apache License
//-----------------------------------------------------------------------
// <copyright file="SearchService.cs" company="StrixIT">
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using StrixIT.Platform.Core;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    public class SearchService : ISearchService
    {
        private IPlatformDataSource _source;

        public SearchService(IPlatformDataSource source)
        {
            this._source = source;
        }

        public SearchResult Search(FilterOptions options)
        {
            if (options == null)
            {
                options = new FilterOptions();
            }

            var culture = StrixPlatform.CurrentCultureCode;
            var result = new SearchResult();
            result.Locators = PageRegistration.ContentLocators;
            var typesTosearch = EntityHelper.EntityTypes.Where(e => e.Name != typeof(MailContentTemplate).FullName && e.Name != typeof(MailContent).FullName).ToList();
            var itemsToSkip = options.PageSize == 1 ? 0 : options.PageSize * (options.Page - 1);

            // Todo: make configurable.
            var itemsToGet = options.PageSize == 0 ? 100 : options.PageSize;

            foreach (var entityType in typesTosearch)
            {
                var entryType = EntityHelper.GetEntityType(entityType.Id);
                var query = this._source.Query(entryType).Where("Culture.Equals(@0) AND IsCurrentVersion", culture);

                var queryEvent = new PrepareQueryEvent(query, options, false);
                StrixPlatform.RaiseEvent(queryEvent);
                query = queryEvent.Query;

                if (itemsToGet > 0)
                {
                    if (entryType.Equals(typeof(Html)))
                    {
                        var htmlTypeName = typeof(Html).FullName;
                        var htmlLocators = PageRegistration.ContentLocators.Where(l => l.ContentTypeName == htmlTypeName);
                        var htmlQuery = query.Cast<Html>().Select(h => new { Name = h.Name, Url = h.Entity.Url }).ToList().Select(h => new { Name = h.Name, Url = htmlLocators.Where(l => l.ContentUrl.ToLower() == h.Url.ToLower()).Select(l => l.PageUrl).FirstOrDefault() });
                        query = htmlQuery.GroupBy(h => h.Url).Select(h => new Html { Name = h.Select(i => i.Url.ToTitleCase()).First(), Entity = new PlatformEntity { Id = Guid.Empty, Url = h.Select(i => i.Url).First() } }).AsQueryable();
                    }

                    int addedItems = 0;
                    var entries = query.Skip(itemsToSkip).Take(itemsToGet).Select<SearchItem>("new (Entity.Id, Name, Entity.Url)").ToList();          

                    foreach (var entry in entries)
                    {
                        entry.TypeName = entryType.FullName;
                        result.Data.Add(entry);
                        addedItems++;
                    }

                    options.Total = 0;
                    itemsToGet -= addedItems;
                }

                var queryCount = query.Count();
                itemsToSkip -= queryCount;

                if (itemsToSkip < 0)
                {
                    itemsToSkip = 0;
                }

                result.Total += queryCount;
            }

            return result;
        }
    }
}
﻿#region Apache License
//-----------------------------------------------------------------------
// <copyright file="SearchController.cs" company="StrixIT">
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

using System.Linq;
using System.Web.Mvc;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class SearchController : Controller
    {
        private ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            this._searchService = searchService;
        }

        public ActionResult Index()
        {
            // Register all pages that have not yet been registered before starting the search.
            PageRegistration.RegisterAllPages(this.ControllerContext.HttpContext, this.RouteData);
            return this.View();
        }

        [HttpPost]
        public JsonResult Search(FilterOptions options)
        {
            var searchTerm = options != null && options.Filter != null && options.Filter.Filters != null ? options.Filter.Filters.FirstOrDefault() : null;

            if (searchTerm != null)
            {
                this.ViewBag.SearchTerm = searchTerm.Value;
            }

            return this.Json(this._searchService.Search(options));
        }
    }
}
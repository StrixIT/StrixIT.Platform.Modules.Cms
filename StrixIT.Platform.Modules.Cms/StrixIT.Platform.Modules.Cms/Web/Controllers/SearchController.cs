//-----------------------------------------------------------------------
// <copyright file="SearchController.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
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
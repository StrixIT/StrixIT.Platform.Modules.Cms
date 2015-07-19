#region Apache License
//-----------------------------------------------------------------------
// <copyright file="EntityController.cs" company="StrixIT">
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
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StrixIT.Platform.Core;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The base class for all controllers used for entities.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the entity</typeparam>
    [StrixAuthorization(Roles = CmsRoleNames.EDITORROLES)]
    public abstract class EntityController<TModel> : BaseCrudController<Guid, TModel> where TModel : EntityViewModel, new()
    {
        private ICommentService _commentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityController{TModel}" /> class.
        /// </summary>
        /// <param name="entityService">The entity service to use</param>
        /// <param name="commentService">The comment service to use</param>
        protected EntityController(IEntityService<TModel> entityService, ICommentService commentService)
            : base(entityService)
        {
            this._commentService = commentService;
        }

        /// <summary>
        /// Gets the entity service used.
        /// </summary>
        protected IEntityService<TModel> Service
        {
            get
            {
                return this._service as IEntityService<TModel>;
            }
        }

        /// <summary>
        /// Gets the index view, which displays a list of entities.
        /// </summary>
        /// <returns>The index view</returns>
        public override ActionResult Index()
        {
            var config = new EntityListConfiguration<TModel>(StrixPlatform.User);
            return this.View(config);
        }

        #region Content

        /// <summary>
        /// Displays content as a partial.
        /// </summary>
        /// <param name="options">The filter options to use when displaying a list</param>
        /// <param name="url">The url of the content when displaying a single item</param>
        /// <returns>The content partial</returns>
        [ChildActionOnly]
        [AllowAnonymous]
        public virtual ActionResult Display(FilterOptions options, string url)
        {
            var type = (string)HttpContext.Items[CmsConstants.DISPLAYTYPE] ?? CmsConstants.DISPLAYTYPEITEM;
            var itemPageUrl = (string)HttpContext.Items[CmsConstants.ITEMPAGEURL];

            if (type == CmsConstants.DISPLAYTYPELIST && !string.IsNullOrWhiteSpace(itemPageUrl) && HttpContext.Items[CmsConstants.ITEMURL] != null)
            {
                type = CmsConstants.DISPLAYTYPEITEM;
            }

            this.UpdateContentLocator();

            if (type == CmsConstants.DISPLAYTYPEITEM && !string.IsNullOrWhiteSpace(url))
            {
                var model = this.Service.GetCached(this.GetContentUrl(url));
                return this.View("Item", model);
            }
            else
            {
                var model = new EntityListConfiguration<TModel>(StrixPlatform.User);
                return this.View("List", model);
            }
        }

        /// <summary>
        /// Displays the tags for an entity as a partial.
        /// </summary>
        /// <param name="url">The url of the content to display the tags for</param>
        /// <returns>The tags partial</returns>
        [ChildActionOnly]
        [AllowAnonymous]
        public virtual ActionResult DisplayTags(string url)
        {
            return this.DisplayWidget(EntityServiceActions.AllowFixedTagging, url, "Tags", x => this.Service.GetTags(x));
        }

        /// <summary>
        /// Displays the comments for an entity as a partial.
        /// </summary>
        /// <param name="url">The url of the content to display the comments for</param>
        /// <returns>The comments partial</returns>
        [ChildActionOnly]
        [AllowAnonymous]
        public virtual ActionResult DisplayComments(string url)
        {
            return this.DisplayWidget(EntityServiceActions.AllowComments, url, "CommentForm", x => x);
        }

        #endregion

        /// <summary>
        /// Checks whether an entity with the specified name and id other than the entity itself already exists.
        /// </summary>
        /// <param name="value">The name value</param>
        /// <param name="id">The entity id</param>
        /// <returns>True if no other entity with this id and name exists, false otherwise</returns>
        [HttpPost]
        [StrixAuthorization(Roles = CmsRoleNames.CONTRIBUTORROLES)]
        public override JsonResult CheckName(string value, Guid? id)
        {
            return base.CheckName(value, id);
        }

        /// <summary>
        /// Gets an entity as json.
        /// </summary>
        /// <param name="id">The id of the entity to get</param>
        /// <returns>The entity</returns>
        [HttpPost]
        [AllowAnonymous]
        public override ActionResult Get(string id)
        {
            if (!this.CheckIsAccessAllowed())
            {
                return new HttpStatusCodeResult(401);
            }

            return this.Json(this.GetModel(id, null, null, true));
        }

        /// <summary>
        /// Gets the entity edit view.
        /// </summary>
        /// <returns>The edit view</returns>
        [StrixAuthorization(Roles = CmsRoleNames.CONTRIBUTORROLES)]
        public override ActionResult Edit()
        {
            return base.Edit();
        }

        /// <summary>
        /// Gets content asynchronously.
        /// </summary>
        /// <param name="entityId">The content entity id</param>
        /// <param name="culture">The content culture</param>
        /// <param name="versionNumber">The content version number</param>
        /// <returns>The content as Json</returns>
        [HttpPost]
        [AllowAnonymous]
        public virtual ActionResult GetEntity(string entityId, string culture, int? versionNumber)
        {
            if (string.IsNullOrWhiteSpace(entityId))
            {
                return null;
            }

            if (!this.CheckIsAccessAllowed())
            {
                return new HttpStatusCodeResult(401);
            }

            var model = this.GetModel(entityId, culture, versionNumber);
            return this.Json(model);
        }

        /// <summary>
        /// Gets a list of entity list models as Json, filtered using the search options.
        /// </summary>
        /// <param name="options">The search options</param>
        /// <returns>The list of entity list models as Json</returns>
        [HttpPost]
        [AllowAnonymous]
        public override ActionResult List(FilterOptions options)
        {
            if (!this.CheckIsAccessAllowed())
            {
                return new HttpStatusCodeResult(401);
            }

            return base.List(options);
        }

        /// <summary>
        /// Edits an entity.
        /// </summary>
        /// <param name="model">The entity view model</param>
        /// <returns>The edit result as Json</returns>
        [StrixAuthorization(Roles = CmsRoleNames.CONTRIBUTORROLES)]
        public override JsonResult Edit(TModel model)
        {
            return base.Edit(model);
        }

        [AllowAnonymous]
        public ActionResult Item(string url)
        {
            if (!this.CheckIsAccessAllowed())
            {
                return new HttpStatusCodeResult(401);
            }

            var model = this.Service.GetCached(url);
            return this.View(model);
        }

        #region Versions

        /// <summary>
        /// Gets a list of versions for an entity, using the specified filter, sort and page options.
        /// </summary>
        /// <param name="options">The specified filter, sort and page options to use</param>
        /// <param name="entityId">The id of the entity to get the version list for</param>
        /// <returns>The list of versions</returns>
        public JsonResult GetVersionList(FilterOptions options, Guid entityId)
        {
            return this.Json(this.Service.GetVersionList(entityId, null, options).DataRecords(options));
        }

        /// <summary>
        /// Restores an entity to the version specified or activates the specified draft.
        /// </summary>
        /// <param name="id">The id of the entity</param>
        /// <param name="versionNumber">The number of the version or draft to restore or activate</param>
        /// <param name="log">The message to write to the entity log</param>
        /// <returns>A Json result message</returns>
        [HttpPost]
        public virtual JsonResult RestoreVersion(Guid id, int versionNumber, string log)
        {
            var model = this.Service.RestoreVersion(id, versionNumber, log);
            return this.Json(model);
        }

        #endregion

        #region protected Methods

        protected bool CheckIsAccessAllowed()
        {
            var result = true;
            var map = EntityHelper.GetObjectMap(typeof(TModel));

            // Todo: use injected request and response
            // If anonymous access is not enabled for this entity type, return 401.
            if (!Request.IsAuthenticated && !EntityHelper.IsServiceActive(map.ContentType, EntityServiceActions.AllowAnonymousAccess))
            {
                result = false;

                if (Request.IsAjaxRequest())
                {
                    Response.SuppressFormsAuthenticationRedirect = true;
                }
            }

            return result;
        }

        protected TModel GetModel(string id, string culture = null, int? versionNumber = null, bool useFallBack = false)
        {
            Guid key;
            TModel model;

            if (string.IsNullOrWhiteSpace(id))
            {
                return this.Service.Get(null);
            }

            var map = EntityHelper.GetObjectMap(typeof(TModel));

            if (Guid.TryParse(id, out key))
            {
                if (versionNumber.HasValue)
                {
                    model = this.Service.Get(key, culture, versionNumber.Value);
                }
                else
                {
                    model = this.Service.Get(key, culture);
                }
            }
            else
            {
                if (versionNumber.HasValue)
                {
                    model = this.Service.Get(id, culture, versionNumber.Value);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(culture) && useFallBack)
                    {
                        model = this.Service.GetAny(id, culture, null);
                    }
                    else
                    {
                        model = this.Service.Get(id, culture);
                    }
                }
            }

            return model;
        }

        #endregion

        #region Private Methods

        private void UpdateContentLocator()
        {
            ViewBag.ModelType = typeof(TModel);
            var locator = PageRegistration.ContentLocators.FirstOrDefault(l => l.ContentTypeName == null);

            if (locator != null)
            {
                var map = EntityHelper.GetObjectMap(typeof(TModel));
                locator.ContentTypeName = map.ContentType.FullName;
            }
        }

        private string GetContentUrl(string url)
        {
            var childUrl = (string)HttpContext.Items[CmsConstants.CHILDURL];
            var itemUrl = (string)HttpContext.Items[CmsConstants.ITEMURL];

            if (!string.IsNullOrWhiteSpace(childUrl))
            {
                url = string.Format("{0}/{1}", url, childUrl);
            }
            else if (!string.IsNullOrWhiteSpace(itemUrl))
            {
                url = itemUrl;
            }

            return url;
        }

        private ActionResult DisplayWidget(string serviceName, string url, string view, Func<Guid, object> modelFunc)
        {
            var map = EntityHelper.GetObjectMap(typeof(TModel));

            if (EntityHelper.IsServiceActive(map.ContentType, serviceName))
            {
                url = url.Replace("&#47;", "/");
                var id = this.Service.GetId(url);

                if (id.HasValue)
                {
                    ViewBag.ModelType = typeof(TModel);
                    return this.View(view, modelFunc(id.Value));
                }
            }

            return null;
        }

        #endregion
    }
}
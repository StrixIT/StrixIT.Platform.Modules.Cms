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

#endregion Apache License

using StrixIT.Platform.Core;
using StrixIT.Platform.Web;
using System;
using System.Linq;
using System.Web.Mvc;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The base class for all controllers used for entities.
    /// </summary>
    /// <typeparam name="TModel">The view model type for the entity</typeparam>
    [StrixAuthorization(Roles = CmsRoleNames.EDITORROLES)]
    public abstract class EntityController<TModel> : BaseCrudController<Guid, TModel> where TModel : EntityViewModel, new()
    {
        #region Private Fields

        private ICmsServices _cmsServices;

        #endregion Private Fields

        #region Protected Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityController{TModel}"/> class.
        /// </summary>
        /// <param name="entityService">The entity service to use</param>
        /// <param name="commentService">The comment service to use</param>
        protected EntityController(IEntityService<TModel> service, ICmsServices cmsServices)
            : base(cmsServices.Environment, service)
        {
            _cmsServices = cmsServices;
        }

        #endregion Protected Constructors

        #region Protected Properties

        protected IEntityService<TModel> EntityService
        {
            get
            {
                return _service as IEntityService<TModel>;
            }
        }

        protected ICmsServices Services
        {
            get
            {
                return _cmsServices;
            }
        }

        #endregion Protected Properties

        #region Public Methods

        /// <summary>
        /// Checks whether an entity with the specified name and id other than the entity itself
        /// already exists.
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
        /// Gets the entity edit view.
        /// </summary>
        /// <returns>The edit view</returns>
        [StrixAuthorization(Roles = CmsRoleNames.CONTRIBUTORROLES)]
        public override ActionResult Edit()
        {
            return base.Edit();
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
        /// Gets the index view, which displays a list of entities.
        /// </summary>
        /// <returns>The index view</returns>
        public override ActionResult Index()
        {
            var config = new EntityListConfiguration<TModel>(Environment.User, _cmsServices.EntityHelper);
            return this.View(config);
        }

        #endregion Public Methods

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
                var model = EntityService.GetCached(this.GetContentUrl(url));
                return this.View("Item", model);
            }
            else
            {
                var model = new EntityListConfiguration<TModel>(Environment.User, _cmsServices.EntityHelper);
                return this.View("List", model);
            }
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

        /// <summary>
        /// Displays the tags for an entity as a partial.
        /// </summary>
        /// <param name="url">The url of the content to display the tags for</param>
        /// <returns>The tags partial</returns>
        [ChildActionOnly]
        [AllowAnonymous]
        public virtual ActionResult DisplayTags(string url)
        {
            return this.DisplayWidget(EntityServiceActions.AllowFixedTagging, url, "Tags", x => EntityService.GetTags(x));
        }

        #endregion Content

        [AllowAnonymous]
        public ActionResult Item(string url)
        {
            if (!this.CheckIsAccessAllowed())
            {
                return new HttpStatusCodeResult(401);
            }

            var model = EntityService.GetCached(url);
            return this.View(model);
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

        #region Versions

        /// <summary>
        /// Gets a list of versions for an entity, using the specified filter, sort and page options.
        /// </summary>
        /// <param name="options">The specified filter, sort and page options to use</param>
        /// <param name="entityId">The id of the entity to get the version list for</param>
        /// <returns>The list of versions</returns>
        public JsonResult GetVersionList(FilterOptions options, Guid entityId)
        {
            return this.Json(EntityService.GetVersionList(entityId, null, options).DataRecords(options));
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
            var model = EntityService.RestoreVersion(id, versionNumber, log);
            return this.Json(model);
        }

        #endregion Versions

        #region protected Methods

        protected bool CheckIsAccessAllowed()
        {
            var result = true;

            if (!Environment.MembershipActive)
            {
                return true;
            }

            var map = _cmsServices.EntityHelper.GetObjectMap(typeof(TModel));

            // Todo: use injected request and response If anonymous access is not enabled for this
            // entity type, return 401.
            if (!Request.IsAuthenticated && !_cmsServices.EntityHelper.IsServiceActive(map.ContentType, EntityServiceActions.AllowAnonymousAccess))
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
                return EntityService.Get(null);
            }

            var map = _cmsServices.EntityHelper.GetObjectMap(typeof(TModel));

            if (Guid.TryParse(id, out key))
            {
                if (versionNumber.HasValue)
                {
                    model = EntityService.Get(key, culture, versionNumber.Value);
                }
                else
                {
                    model = EntityService.Get(key, culture);
                }
            }
            else
            {
                if (versionNumber.HasValue)
                {
                    model = EntityService.Get(id, culture, versionNumber.Value);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(culture) && useFallBack)
                    {
                        model = EntityService.GetAny(id, culture, null);
                    }
                    else
                    {
                        model = EntityService.Get(id, culture);
                    }
                }
            }

            return model;
        }

        #endregion protected Methods

        #region Private Methods

        private ActionResult DisplayWidget(string serviceName, string url, string view, Func<Guid, object> modelFunc)
        {
            var map = _cmsServices.EntityHelper.GetObjectMap(typeof(TModel));

            if (_cmsServices.EntityHelper.IsServiceActive(map.ContentType, serviceName))
            {
                url = url.Replace("&#47;", "/");
                var id = EntityService.GetId(url);

                if (id.HasValue)
                {
                    ViewBag.ModelType = typeof(TModel);
                    return this.View(view, modelFunc(id.Value));
                }
            }

            return null;
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

        private void UpdateContentLocator()
        {
            ViewBag.ModelType = typeof(TModel);
            var locator = _cmsServices.PageRegistrator.ContentLocators.FirstOrDefault(l => l.ContentTypeName == null);

            if (locator != null)
            {
                var map = _cmsServices.EntityHelper.GetObjectMap(typeof(TModel));
                locator.ContentTypeName = map.ContentType.FullName;
            }
        }

        #endregion Private Methods
    }
}
#region Apache License

//-----------------------------------------------------------------------
// <copyright file="CmsAreaRegistration.cs" company="StrixIT">
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
using System.Web.Mvc;

namespace StrixIT.Platform.Modules.Cms
{
    public class CmsAreaRegistration : AreaRegistration
    {
        #region Public Properties

        public override string AreaName
        {
            get
            {
                return "Cms";
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var culture = StrixPlatform.DefaultCultureCode.ToLower();

            context.Routes.MapRoute(
                "SecureFiles",
                "Content/Uploads/Secure/{*url}",
                new { controller = "File", action = "GetFile", type = "Uploads", url = UrlParameter.Optional });

            context.Routes.MapRoute(
                "SecureThumbs",
                "Content/Thumbs/Secure/{*url}",
                new { controller = "File", action = "GetFile", type = "Thumbs", url = UrlParameter.Optional });

            context.Routes.MapLocalizedRoute(
                "CheckName",
                "{language}/{controller}/CheckName/{name}/{entityId}",
                new { language = culture, controller = "Home", action = "CheckName", name = UrlParameter.Optional, entityId = UrlParameter.Optional });

            context.Routes.MapRoute(
                "Images",
                "Image/{width}/{height}/{*url}",
                new { controller = "File", action = "GetImage", type = "Thumbs", width = UrlParameter.Optional, height = UrlParameter.Optional, url = UrlParameter.Optional });

            context.MapLocalizedRoute(
                "EntityAction",
                "{language}/Admin/Cms/EntityAction/{action}/{id}",
                new { language = culture, controller = "EntityAction", action = MvcConstants.INDEX, id = UrlParameter.Optional });

            context.MapLocalizedRoute(
                "Versioning",
                "{language}/Admin/Version/{action}/{id}",
                new { language = culture, controller = "Version", action = MvcConstants.INDEX, id = UrlParameter.Optional });

            context.MapLocalizedRoute(
                name: "Cms_Default",
                url: "{language}/Admin/Cms/{controller}/{action}/{id}",
                defaults: new { language = culture, controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        #endregion Public Methods
    }
}
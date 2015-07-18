//-----------------------------------------------------------------------
// <copyright file="CmsAreaRegistration.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Web.Mvc;
using StrixIT.Platform.Core;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    public class CmsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Cms";
            }
        }

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
    }
}
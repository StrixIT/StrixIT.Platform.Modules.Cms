//-----------------------------------------------------------------------
// <copyright file="AuthenticatedCacheAttribute.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// An attribute to allow caching when authentication is needed. It is used to prevent caching items when a user is authenticated,
    /// and then serving the cached file to a non-authenticated user.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AuthenticatedCacheAttribute : OutputCacheAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            var httpContext = filterContext.HttpContext;

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                // making cache location none means no cache
                this.Location = OutputCacheLocation.None;
            }
            else
            {
                // you can put your favorite cache location
                this.Location = OutputCacheLocation.Any;
            }

            httpContext.Response.Cache.AddValidationCallback(this.IgnoreUnAuthenticated, null);
            base.OnResultExecuting(filterContext);
        }

        private void IgnoreUnAuthenticated(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                validationStatus = HttpValidationStatus.Valid;
            }
            else
            {
                validationStatus = HttpValidationStatus.IgnoreThisRequest;
            }
        }
    }
}
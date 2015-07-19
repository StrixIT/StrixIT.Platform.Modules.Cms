#region Apache License
//-----------------------------------------------------------------------
// <copyright file="AuthenticatedCacheAttribute.cs" company="StrixIT">
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
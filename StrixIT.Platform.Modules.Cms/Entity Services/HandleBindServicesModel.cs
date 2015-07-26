#region Apache License

//-----------------------------------------------------------------------
// <copyright file="HandleBindServicesModel.cs" company="StrixIT">
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

using Newtonsoft.Json;
using StrixIT.Platform.Core;
using StrixIT.Platform.Web;
using System;
using System.IO;
using System.Web.Mvc;

namespace StrixIT.Platform.Modules.Cms
{
    public class HandleBindServicesModel : IHandlePlatformEvent<BindModelEvent>
    {
        #region Public Methods

        public void Handle(BindModelEvent args)
        {
            if (args.ModelBindingContext.ModelType == typeof(EntityServiceCollection))
            {
                args.Result = JsonConvert.DeserializeObject<EntityServiceCollection>(GetInputString(args.ControllerContext));
                args.IsBound = true;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static string GetInputString(ControllerContext controllerContext)
        {
            if (controllerContext.HttpContext.Request == null)
            {
                throw new InvalidOperationException("Request is not available");
            }

            if (controllerContext.HttpContext.Request.InputStream == null)
            {
                throw new InvalidOperationException("No request input stream is available");
            }

            string inputString;

            var stream = controllerContext.HttpContext.Request.InputStream;
            stream.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(stream))
            {
                inputString = reader.ReadToEnd();
            }

            return inputString;
        }

        #endregion Private Methods
    }
}
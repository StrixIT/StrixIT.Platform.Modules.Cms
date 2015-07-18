//-----------------------------------------------------------------------
// <copyright file="HandleBindServicesModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using StrixIT.Platform.Core;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    public class HandleBindServicesModel : IHandlePlatformEvent<BindModelEvent>
    {
        public void Handle(BindModelEvent args)
        {
            if (args.ModelBindingContext.ModelType == typeof(EntityServiceCollection))
            {
                args.Result = JsonConvert.DeserializeObject<EntityServiceCollection>(GetInputString(args.ControllerContext));
                args.IsBound = true;
            }
        }

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
    }
}
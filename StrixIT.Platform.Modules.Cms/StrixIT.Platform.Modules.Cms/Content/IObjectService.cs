//-----------------------------------------------------------------------
// <copyright file="IObjectService.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections;
using StrixIT.Platform.Core;
using StrixIT.Platform.Web;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface for the cms object service.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key for the object type this service is for</typeparam>
    /// <typeparam name="TModel">The type of the view model the service is for</typeparam>
    public interface IObjectService<TKey, TModel> : ICrudService<TKey, TModel> where TKey : struct where TModel : PlatformBaseViewModel
    {
    }
}

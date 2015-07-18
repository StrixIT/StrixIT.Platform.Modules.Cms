//-----------------------------------------------------------------------
// <copyright file="CacheEntityModelEvent.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// An event to edit the contents of an entity model before caching it.
    /// </summary>
    /// <typeparam name="TModel">The type of the view model</typeparam>
    public class CacheEntityModelEvent<TModel> : IPlatformEvent where TModel : EntityViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheEntityModelEvent{TModel}" /> class.
        /// </summary>
        /// <param name="model">The entity model to cache</param>
        public CacheEntityModelEvent(TModel model)
        {
            this.Model = model;
        }

        /// <summary>
        /// Gets the entity model to cache.
        /// </summary>
        public TModel Model { get; private set; }
    }
}

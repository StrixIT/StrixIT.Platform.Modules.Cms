#region Apache License
//-----------------------------------------------------------------------
// <copyright file="CacheEntityModelEvent.cs" company="StrixIT">
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

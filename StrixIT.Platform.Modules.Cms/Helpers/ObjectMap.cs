#region Apache License

//-----------------------------------------------------------------------
// <copyright file="ObjectMap.cs" company="StrixIT">
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

using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The class to map content types to view models.
    /// </summary>
    public class ObjectMap
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectMap"/> class.
        /// </summary>
        /// <param name="contentType">The content type</param>
        /// <param name="viewModelType">The view model type</param>
        /// <param name="listModelType">The list model type</param>
        public ObjectMap(Type contentType, Type viewModelType, Type listModelType)
        {
            this.ContentType = contentType;
            this.ViewModelType = viewModelType;
            this.ListModelType = listModelType;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the content type of this map.
        /// </summary>
        public Type ContentType { get; private set; }

        /// <summary>
        /// Gets the list model type of this map.
        /// </summary>
        public Type ListModelType { get; private set; }

        /// <summary>
        /// Gets the view model type of this map.
        /// </summary>
        public Type ViewModelType { get; private set; }

        #endregion Public Properties
    }
}
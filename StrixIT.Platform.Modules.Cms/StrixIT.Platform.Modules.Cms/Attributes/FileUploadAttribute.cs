#region Apache License
//-----------------------------------------------------------------------
// <copyright file="FileUploadAttribute.cs" company="StrixIT">
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

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// An attribute to mark a property as an image property. This will have the platform create a thumbnail for it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class FileUploadAttribute : Attribute
    {
        /// <summary>
        /// The file id property on the object.
        /// </summary>
        private string _idProperty;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileUploadAttribute" /> class.
        /// </summary>
        public FileUploadAttribute() : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileUploadAttribute" /> class.
        /// </summary>
        /// <param name="idProperty">The file id property on the object</param>
        public FileUploadAttribute(string idProperty)
        {
            this._idProperty = idProperty;
        }

        /// <summary>
        /// Gets the id property of the file on the entity.
        /// </summary>
        public string IdProperty
        {
            get
            {
                return this._idProperty;
            }
        }
    }
}
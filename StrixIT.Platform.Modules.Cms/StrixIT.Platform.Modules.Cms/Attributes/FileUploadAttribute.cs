//-----------------------------------------------------------------------
// <copyright file="FileUploadAttribute.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
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
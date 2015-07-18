//-----------------------------------------------------------------------
// <copyright file="ObjectMap.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The class to map content types to view models.
    /// </summary>
    public class ObjectMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectMap" /> class.
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

        /// <summary>
        /// Gets the view model type of this map.
        /// </summary>
        public Type ViewModelType { get; private set; }

        /// <summary>
        /// Gets the list model type of this map.
        /// </summary>
        public Type ListModelType { get; private set; }

        /// <summary>
        /// Gets the content type of this map.
        /// </summary>
        public Type ContentType { get; private set; }
    }
}
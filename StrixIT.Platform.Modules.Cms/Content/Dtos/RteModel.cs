#region Apache License

//-----------------------------------------------------------------------
// <copyright file="RteModel.cs" company="StrixIT">
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
    /// A view model for easily initializing the Rich Text Editor.
    /// </summary>
    public class RteModel
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RteModel"/> class.
        /// </summary>
        /// <param name="modelType">The type of the model the editor is for</param>
        /// <param name="fieldName">The entity property name the model is for</param>
        public RteModel(Type modelType, string fieldName) : this(modelType, fieldName, 35, 100)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RteModel"/> class.
        /// </summary>
        /// <param name="modelType">The type of the model the editor is for</param>
        /// <param name="fieldName">The entity property name the model is for</param>
        /// <param name="rows">The number of rows to use</param>
        /// <param name="columns">The number of columns to use</param>
        public RteModel(Type modelType, string fieldName, int rows, int columns)
        {
            this.ModelType = modelType;
            this.FieldName = fieldName;
            this.Rows = rows;
            this.Columns = columns;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the number of colums for the editor.
        /// </summary>
        public int Columns { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether only the comment options should be available. If
        /// not, the CMS settings are used.
        /// </summary>
        public bool CommentSettingsOnly { get; set; }

        /// <summary>
        /// Gets the name of the property of the entity the editor is for.
        /// </summary>
        public string FieldName { get; private set; }

        /// <summary>
        /// Gets or sets the name of the type of the model the editor is for.
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Gets the type of the model the editor is for.
        /// </summary>
        public Type ModelType { get; private set; }

        public string RequiredMessage { get; set; }

        /// <summary>
        /// Gets the number of rows for the editor.
        /// </summary>
        public int Rows { get; private set; }

        #endregion Public Properties
    }
}
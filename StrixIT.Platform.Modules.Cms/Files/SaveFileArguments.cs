#region Apache License
//-----------------------------------------------------------------------
// <copyright file="SaveFileArguments.cs" company="StrixIT">
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

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A helper class for saving files.
    /// </summary>
    public class SaveFileArguments
    {
        /// <summary>
        /// Gets or sets the name of the file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets a byte array containing the file data
        /// </summary>
        public byte[] FileData { get; set; }

        /// <summary>
        /// Gets or sets the maximum width the file can have. If the uploaded file is larger, it will be resized
        /// </summary>
        public int? MaxX { get; set; }

        /// <summary>
        /// Gets or sets the maximum height the file can have. If the uploaded file is larger, it will be resized
        /// </summary>
        public int? MaxY { get; set; }

        /// <summary>
        /// Gets or sets the custom width to resize the file to, if any
        /// </summary>
        public int? CustomX { get; set; }

        /// <summary>
        /// Gets or sets the custom height to resize the file to, if any
        /// </summary>
        public int? CustomY { get; set; }
    }
}
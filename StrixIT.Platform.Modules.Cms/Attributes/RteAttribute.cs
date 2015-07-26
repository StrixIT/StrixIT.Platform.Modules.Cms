#region Apache License
//-----------------------------------------------------------------------
// <copyright file="RteAttribute.cs" company="StrixIT">
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
    /// This property is used to indicate a property uses a Rich Text Editor to enter data, and the data entered may contain html and file paths 
    /// to files uploaded using the editor. These files are tracked in the system, to be able to identify uploaded files and use this information
    /// to prevent deleting files that are used in pieces of HTML.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RteAttribute : Attribute
    {
    }
}
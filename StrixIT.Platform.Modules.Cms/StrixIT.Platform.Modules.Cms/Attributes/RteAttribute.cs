//-----------------------------------------------------------------------
// <copyright file="RteAttribute.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
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
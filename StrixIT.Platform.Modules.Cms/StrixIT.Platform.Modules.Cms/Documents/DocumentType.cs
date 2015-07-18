//-----------------------------------------------------------------------
// <copyright file="DocumentType.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// An enumeration for the document types.
    /// </summary>
    [ClientEnum]
    public enum DocumentType
    {
        Unknown,
        Image,
        Video,
        Audio,
        Document
    }
}
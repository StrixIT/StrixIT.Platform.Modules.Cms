//-----------------------------------------------------------------------
// <copyright file="ContentCustomFieldValue.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A class to define custom field values for content.
    /// </summary>
    public class ContentCustomFieldValue : CustomFieldValue<EntityCustomField>
    {
        /// <summary>
        /// Gets or sets the id of the content this value belongs to.
        /// </summary>
        [StrixRequired]
        public Guid ContentId { get; set; }
    }
}

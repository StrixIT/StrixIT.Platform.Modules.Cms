//-----------------------------------------------------------------------
// <copyright file="HtmlListModel.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Newtonsoft.Json;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The list model for displaying a list of Html.
    /// </summary>
    public class HtmlListModel : EntityListModel
    {
        /// <summary>
        /// Gets or sets the Html body.
        /// </summary>
        [JsonIgnore]
        public string Body { get; set; }
    }
}
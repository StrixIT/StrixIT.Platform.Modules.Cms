//-----------------------------------------------------------------------
// <copyright file="HandlePrepareQuery.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Linq;
using System.Linq.Dynamic;
using StrixIT.Platform.Core;

// Todo: move to taxonomy module?
namespace StrixIT.Platform.Modules.Cms
{
    public class HandlePrepareQuery : IHandlePlatformEvent<PrepareQueryEvent>
    {
        public void Handle(PrepareQueryEvent args)
        {
            var tagsToSearchField = args.Filter.Filter.Filters.FirstOrDefault(f => f.Value != null && f.Value.ToLower().Contains("tag:"));

            if (tagsToSearchField != null)
            {
                var tags = tagsToSearchField.Value.ToLower().Replace("tag:", string.Empty).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToLower().ToArray();
                args.Query = args.Query.Where("Entity.Tags.Any(@0.Equals(Name.ToLower()))", tags).Sort(args.Filter, !args.Page);
            }
            else
            {
                args.Query = args.Query.Filter(args.Filter, !args.Page);
            }
        }
    }
}
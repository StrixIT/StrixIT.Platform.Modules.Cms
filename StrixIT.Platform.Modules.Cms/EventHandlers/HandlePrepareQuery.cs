#region Apache License

//-----------------------------------------------------------------------
// <copyright file="HandlePrepareQuery.cs" company="StrixIT">
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

using StrixIT.Platform.Core;
using System;
using System.Linq;
using System.Linq.Dynamic;

// Todo: move to taxonomy module?
namespace StrixIT.Platform.Modules.Cms
{
    public class HandlePrepareQuery : IHandlePlatformEvent<PrepareQueryEvent>
    {
        #region Public Methods

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

        #endregion Public Methods
    }
}
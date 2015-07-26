#region Apache License

//-----------------------------------------------------------------------
// <copyright file="GroupSaveHandler.cs" company="StrixIT">
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

namespace StrixIT.Platform.Modules.Cms
{
    public class GroupSaveHandler : IHandlePlatformEvent<GeneralEvent>
    {
        #region Public Methods

        public void Handle(GeneralEvent args)
        {
            if (args.EventName != "GroupCreateEvent" && args.EventName != "GroupUpdateEvent")
            {
                return;
            }

            var id = (Guid)args.Data["Id"];
            var name = (string)args.Data["GroupName"];

            var source = DependencyInjector.Get<IPlatformDataSource>(PlatformConstants.STRUCTUREMAPPRIVATE);

            var lookup = source.Query<GroupData>().FirstOrDefault(u => u.Id == id);

            if (lookup == null)
            {
                lookup = new GroupData(id, name);
            }

            lookup.Name = name;
            source.Save(lookup);
            source.SaveChanges();

            StrixCms.ClearGroupNameDictionary();
        }

        #endregion Public Methods
    }
}
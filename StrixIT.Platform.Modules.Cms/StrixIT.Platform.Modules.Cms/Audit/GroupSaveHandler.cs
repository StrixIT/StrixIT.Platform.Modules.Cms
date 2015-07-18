//-----------------------------------------------------------------------
// <copyright file="GroupSaveHandler.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Linq;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class GroupSaveHandler : IHandlePlatformEvent<GeneralEvent>
    {
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
    }
}
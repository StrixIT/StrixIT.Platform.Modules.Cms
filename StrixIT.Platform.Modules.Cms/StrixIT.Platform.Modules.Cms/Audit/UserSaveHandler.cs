//-----------------------------------------------------------------------
// <copyright file="UserSaveHandler.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Linq;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class UserSaveHandler : IHandlePlatformEvent<GeneralEvent>
    {
        public void Handle(GeneralEvent args)
        {
            if (args.EventName != "UserCreateEvent" && args.EventName != "UserUpdateEvent")
            {
                return;
            }

            var id = (Guid)args.Data["Id"];
            var name = (string)args.Data["UserName"];
            var email = (string)args.Data["UserEmail"];

            var source = DependencyInjector.Get<IPlatformDataSource>(PlatformConstants.STRUCTUREMAPPRIVATE);

            var lookup = source.Query<UserData>().FirstOrDefault(u => u.Id == id);

            if (lookup == null)
            {
                lookup = new UserData(id, name, email);
            }

            lookup.Name = name;
            lookup.Email = email;
            source.Save(lookup);
            source.SaveChanges();

            StrixCms.ClearUserNameDictionary();
        }
    }
}
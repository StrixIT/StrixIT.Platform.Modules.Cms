#region Apache License

//-----------------------------------------------------------------------
// <copyright file="UserSaveHandler.cs" company="StrixIT">
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
    public class UserSaveHandler : IHandlePlatformEvent<GeneralEvent>
    {
        #region Private Fields

        private IPlatformDataSource _dataSource;
        private IPlatformHelper _helper;

        #endregion Private Fields

        #region Public Constructors

        public UserSaveHandler(IPlatformDataSource dataSource, IPlatformHelper helper)
        {
            _dataSource = dataSource;
            _helper = helper;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Handle(GeneralEvent args)
        {
            if (args.EventName != "UserCreateEvent" && args.EventName != "UserUpdateEvent")
            {
                return;
            }

            var id = (Guid)args.Data["Id"];
            var name = (string)args.Data["UserName"];
            var email = (string)args.Data["UserEmail"];
            var lookup = _dataSource.Query<UserData>().FirstOrDefault(u => u.Id == id);

            if (lookup == null)
            {
                lookup = new UserData(id, name, email);
            }

            lookup.Name = name;
            lookup.Email = email;
            _dataSource.Save(lookup);
            _dataSource.SaveChanges();

            _helper.ClearUserNameDictionary();
        }

        #endregion Public Methods
    }
}
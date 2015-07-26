#region Apache License

//-----------------------------------------------------------------------
// <copyright file="DefaultPlatformHelper.cs" company="StrixIT">
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
using System.Collections.Generic;
using System.Linq;

namespace StrixIT.Platform.Modules.Cms
{
    public class DefaultPlatformHelper : IPlatformHelper
    {
        #region Private Fields

        private static object _groupNameLockObject = new object();
        private static Dictionary<Guid, string> _groupNames = new Dictionary<Guid, string>();
        private static Dictionary<string, IList<string>> _serviceProviderActions;
        private static object _userEmailLockObject = new object();
        private static Dictionary<Guid, string> _userEmails = new Dictionary<Guid, string>();
        private static object _userNameLockObject = new object();
        private static Dictionary<Guid, string> _userNames = new Dictionary<Guid, string>();
        private IPlatformDataSource _source;

        #endregion Private Fields

        #region Public Constructors

        public DefaultPlatformHelper(IPlatformDataSource source)
        {
            this._source = source;

            if (_serviceProviderActions == null)
            {
                _serviceProviderActions = new Dictionary<string, IList<string>>();
                _serviceProviderActions.Add("Access", new List<string> { EntityServiceActions.AllowAnonymousAccess });
                _serviceProviderActions.Add("Names", new List<string> { EntityServiceActions.AllowNonUniqueNames });
                //// _serviceProviderActions.Add("Auto Path", new List<string> { EntityServiceActions.UpdatePaths });
                _serviceProviderActions.Add("Comments", new List<string> { EntityServiceActions.AllowComments, EntityServiceActions.AllowNestedComments/*, EntityServiceActions.ModerateComments*/ });
                _serviceProviderActions.Add("Rich Text Editor", new List<string> { EntityServiceActions.RteAdvanced, EntityServiceActions.RteFileUpload, EntityServiceActions.HtmlEditor });
                _serviceProviderActions.Add("Taxonomy", new List<string> { EntityServiceActions.AllowFixedTagging/*, EntityServiceActions.AllowFreeTagging*/ });
                _serviceProviderActions.Add("Translations", new List<string> { EntityServiceActions.Translations });
                //// _serviceProviderActions.Add("Trash bin", new List<string> { EntityServiceActions.Trashbin });
                _serviceProviderActions.Add("Versioning", new List<string> { EntityServiceActions.AutomaticVersions/*, EntityServiceActions.Drafts*/ });
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public IDictionary<string, IList<string>> Services
        {
            get
            {
                return _serviceProviderActions;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void AddService(string name, IList<string> actions)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (!_serviceProviderActions.ContainsKey(name.ToLower()))
            {
                _serviceProviderActions.Add(name, actions);
            }
        }

        public void ClearGroupNameDictionary()
        {
            _groupNames.Clear();
        }

        public void ClearUserNameDictionary()
        {
            _userNames.Clear();
        }

        public string GetGroupName(Guid id)
        {
            if (_groupNames.Count == 0)
            {
                lock (_groupNameLockObject)
                {
                    if (_userNames.Count == 0)
                    {
                        _groupNames = this._source.Query<GroupData>().ToDictionary(k => k.Id, v => v.Name);
                    }
                }
            }

            if (!_groupNames.ContainsKey(id))
            {
                Logger.Log(string.Format("No group name found for id {0}", id), LogLevel.Error);
                return null;
            }

            return _groupNames[id];
        }

        public string GetUserEmail(Guid id)
        {
            if (_userEmails.Count == 0)
            {
                lock (_userEmailLockObject)
                {
                    if (_userEmails.Count == 0)
                    {
                        _userEmails = this._source.Query<UserData>().ToDictionary(k => k.Id, v => v.Email);
                    }
                }
            }

            if (!_userEmails.ContainsKey(id))
            {
                Logger.Log(string.Format("No email found for id {0}", id), LogLevel.Error);
                return null;
            }

            return _userEmails[id];
        }

        public string GetUserName(Guid id)
        {
            if (id == Guid.Empty)
            {
                return null;
            }

            if (_userNames.Count == 0)
            {
                lock (_userNameLockObject)
                {
                    if (_userNames.Count == 0)
                    {
                        _userNames = this._source.Query<UserData>().ToDictionary(k => k.Id, v => v.Name);
                    }
                }
            }

            if (!_userNames.ContainsKey(id))
            {
                Logger.Log(string.Format("No user name found for id {0}", id), LogLevel.Error);
                return null;
            }

            return _userNames[id];
        }

        #endregion Public Methods
    }
}
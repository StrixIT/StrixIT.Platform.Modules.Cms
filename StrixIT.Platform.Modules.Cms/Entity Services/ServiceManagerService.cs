#region Apache License

//-----------------------------------------------------------------------
// <copyright file="ServiceManagerService.cs" company="StrixIT">
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

namespace StrixIT.Platform.Modules.Cms
{
    public class ServiceManagerService : IServiceManagerService
    {
        #region Private Fields

        private IPlatformDataSource _dataSource;

        private IEntityServiceManager _manager;

        #endregion Private Fields

        #region Public Constructors

        public ServiceManagerService(IPlatformDataSource dataSource, IEntityServiceManager manager)
        {
            this._dataSource = dataSource;
            this._manager = manager;
        }

        #endregion Public Constructors

        #region Public Methods

        public EntityServiceCollection GetServiceActionRecords()
        {
            var result = this._manager.GetManagerActionRecords();
            return result;
        }

        public bool SaveActionRecords(EntityServiceCollection records)
        {
            var result = this._manager.SaveActions(records);
            this._dataSource.SaveChanges();
            return result;
        }

        #endregion Public Methods
    }
}
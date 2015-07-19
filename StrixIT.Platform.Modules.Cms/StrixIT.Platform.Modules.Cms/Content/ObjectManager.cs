#region Apache License
//-----------------------------------------------------------------------
// <copyright file="ObjectManager.cs" company="StrixIT">
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
#endregion

using System;
using System.Linq;
using System.Linq.Dynamic;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class ObjectManager : IObjectManager
    {
        private IPlatformDataSource _dataSource;

        public ObjectManager(IPlatformDataSource dataSource)
        {
            this._dataSource = dataSource;
        }

        protected IPlatformDataSource DataSource
        {
            get
            {
                return this._dataSource;
            }
        }

        public T Get<T>(object id) where T : class
        {
            return this.Get(typeof(T), id, null) as T;
        }

        public T Get<T>(object id, string includes) where T : class
        {
            return this.Get(typeof(T), id, includes) as T;
        }

        public virtual object Get(Type objectType, object id)
        {
            return this.Get(objectType, id, null);
        }

        public virtual object Get(Type objectType, object id, string includes)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException("objectType");
            }

            if (id == null)
            {
                var newObject = Activator.CreateInstance(objectType);
                return newObject;
            }

            return this.Query(objectType, includes).Where("Id.Equals(@0)", id).GetFirst();
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return this.Query<T>(null);
        }

        public virtual IQueryable<T> Query<T>(string includes) where T : class
        {
            return this.Query(typeof(T), includes).Cast<T>();
        }

        public IQueryable Query(Type objectType)
        {
            return this.Query(objectType, null);
        }

        public virtual IQueryable Query(Type objectType, string includes)
        {
            if (objectType == null)
            {
                throw new ArgumentNullException("objectType");
            }

            var allIncludes = string.Join(", ", EntityHelper.GetFileProperties(objectType));

            if (!string.IsNullOrWhiteSpace(includes))
            {
                allIncludes += ", " + includes;
            }

            return this._dataSource.Query(objectType, allIncludes);
        }

        public virtual T Save<T>(T entity) where T : class
        {
            return this._dataSource.Save(entity);
        }

        public virtual void Delete<T>(T entity) where T : class
        {
            this._dataSource.Delete(entity);
        }
    }
}

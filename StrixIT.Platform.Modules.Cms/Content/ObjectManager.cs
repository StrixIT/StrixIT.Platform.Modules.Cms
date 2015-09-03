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

#endregion Apache License

using StrixIT.Platform.Core;
using System;
using System.Linq;
using System.Linq.Dynamic;

namespace StrixIT.Platform.Modules.Cms
{
    public class ObjectManager : IObjectManager
    {
        #region Private Fields

        private IPlatformDataSource _dataSource;
        private IEntityHelper _entityHelper;

        #endregion Private Fields

        #region Public Constructors

        public ObjectManager(IPlatformDataSource dataSource, IEntityHelper entityHelper)
        {
            this._dataSource = dataSource;
            _entityHelper = entityHelper;
        }

        #endregion Public Constructors

        #region Protected Properties

        protected IPlatformDataSource DataSource
        {
            get
            {
                return this._dataSource;
            }
        }

        protected IEntityHelper EntityHelper
        {
            get
            {
                return _entityHelper;
            }
        }

        #endregion Protected Properties

        #region Public Methods

        public virtual void Delete<T>(T entity) where T : class
        {
            this._dataSource.Delete(entity);
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

            var allIncludes = string.Join(", ", _entityHelper.GetFileProperties(objectType));

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

        #endregion Public Methods
    }
}
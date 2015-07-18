//-----------------------------------------------------------------------
// <copyright file="ObjectManager.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
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

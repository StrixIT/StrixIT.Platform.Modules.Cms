#region Apache License

//-----------------------------------------------------------------------
// <copyright file="ObjectService.cs" company="StrixIT">
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
using System.Collections;
using System.Linq.Dynamic;

namespace StrixIT.Platform.Modules.Cms
{
    public class ObjectService<TKey, TModel> : IObjectService<TKey, TModel>
        where TKey : struct
        where TModel : PlatformBaseViewModel, new()
    {
        #region Private Fields

        private IPlatformDataSource _dataSource;

        private IObjectManager _objectManager;

        #endregion Private Fields

        #region Public Constructors

        public ObjectService(IPlatformDataSource dataSource, IObjectManager objectManager)
        {
            this._dataSource = dataSource;
            this._objectManager = objectManager;
        }

        #endregion Public Constructors

        #region Protected Properties

        protected IObjectManager Manager
        {
            get
            {
                return this._objectManager;
            }
        }

        #endregion Protected Properties

        #region Public Methods

        public void Delete(TKey id)
        {
            this.Delete(id, true);
        }

        public virtual void Delete(TKey id, bool saveChanges)
        {
            var map = EntityHelper.GetObjectMap(typeof(TModel));
            var entity = this._objectManager.Get(map.ContentType, id);
            this._objectManager.Delete(entity);

            if (saveChanges)
            {
                this._dataSource.SaveChanges();
            }
        }

        public virtual bool Exists(string name, TKey? id)
        {
            return this.Exists(name, id, null);
        }

        public virtual TModel Get(TKey? id)
        {
            var map = EntityHelper.GetObjectMap(typeof(TModel));
            object entity;

            if (!id.HasValue)
            {
                entity = new TModel();
            }
            else
            {
                entity = this.Manager.Get(map.ContentType, id);
            }

            var model = entity.Map<TModel>();
            return model;
        }

        public virtual TModel Get(object id)
        {
            var map = EntityHelper.GetObjectMap(typeof(TModel));
            object entity;

            if (id == null)
            {
                entity = new TModel();
            }
            else
            {
                entity = this.Manager.Get(map.ContentType, id);
            }

            var model = entity.Map<TModel>();
            return model;
        }

        public virtual IEnumerable List(FilterOptions filter)
        {
            var map = EntityHelper.GetObjectMap(typeof(TModel));
            var content = this.Manager.Query(map.ContentType).Filter(filter).Map(map.ListModelType);
            var list = content.Map(map.ListModelType);
            return list;
        }

        public SaveResult<TModel> Save(TModel model)
        {
            return this.Save(model, true);
        }

        public virtual SaveResult<TModel> Save(TModel model, bool saveChanges)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            var result = new SaveResult<TModel>();

            if (typeof(TModel).HasProperty("Name"))
            {
                if (this.Exists((string)typeof(TModel).GetPropertyValue("Name"), (TKey?)model.GetPropertyValue("Id")))
                {
                    return new SaveResult<TModel> { Success = false, Message = StrixIT.Platform.Modules.Cms.Resources.Interface.NameNotUnique };
                }
            }

            var map = EntityHelper.GetObjectMap(typeof(TModel));
            var entity = this.Manager.Save(model.Map(map.ContentType));

            result = new SaveResult<TModel>(entity != null, entity.Map<TModel>());

            if (result.Success && saveChanges)
            {
                this.SaveChanges();

                var fileIncludes = EntityHelper.GetFileProperties(map.ContentType);

                if (!fileIncludes.IsEmpty())
                {
                    result.Entity = this.Manager.Get(map.ContentType, this._dataSource.GetKeyValue<TKey>(entity));
                }
            }

            return result;
        }

        #endregion Public Methods

        #region Protected Methods

        protected bool Exists(string name, TKey? id, string idPropertyName)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (typeof(TModel).HasProperty("Name"))
            {
                var map = EntityHelper.GetObjectMap(typeof(TModel));
                var keyIsDefault = id == null || id.Equals(default(TKey));
                var query = this._objectManager.Query(map.ContentType).Where("Name.ToLower() == @0", name.ToLower());

                if (!keyIsDefault)
                {
                    idPropertyName = string.IsNullOrWhiteSpace(idPropertyName) ? "Id" : idPropertyName;
                    query = query.Where(string.Format("!{0}.Equals(@0)", idPropertyName), id);
                }

                return query.Any();
            }

            return false;
        }

        protected void SaveChanges()
        {
            this._dataSource.SaveChanges();
        }

        #endregion Protected Methods
    }
}
#region Apache License

//-----------------------------------------------------------------------
// <copyright file="DefaultEntityHelper.cs" company="StrixIT">
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
using StructureMap;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace StrixIT.Platform.Modules.Cms
{
    public class DefaultEntityHelper : IEntityHelper
    {
        #region Private Fields

        private static ConcurrentBag<EntityType> _entityTypeList;
        private static bool _isInitialized = false;
        private static object _lockObject = new object();
        private static ConcurrentBag<ObjectMap> _objectMaps = new ConcurrentBag<ObjectMap>();
        private IUserContext _user;

        #endregion Private Fields

        #region Public Constructors

        [DefaultConstructor]
        public DefaultEntityHelper(IUserContext user) : this(user, null) { }

        public DefaultEntityHelper(IUserContext user, IList<EntityType> entityTypes)
        {
            _user = user;

            if (entityTypes != null)
            {
                _entityTypeList = new ConcurrentBag<EntityType>(entityTypes);
            }

            if (!_isInitialized)
            {
                Init();
            }
        }

        #endregion Public Constructors

        #region Public Properties

        public IList<EntityType> EntityTypes
        {
            get
            {
                return _entityTypeList.ToList();
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void ActivateServices(Type entityType, IEnumerable<string> actions)
        {
            if (actions == null)
            {
                throw new ArgumentNullException("actions");
            }

            if (!typeof(IContent).IsAssignableFrom(entityType))
            {
                return;
            }

            entityType = GetNonProxyType(entityType);
            var type = this.EntityTypes.FirstOrDefault(t => t.Name == entityType.FullName);

            foreach (var action in actions)
            {
                var serviceAction = type.EntityTypeServiceActions.FirstOrDefault(s => s.GroupId == _user.GroupId && s.Action.ToLower() == action.ToLower());

                if (serviceAction == null)
                {
                    type.EntityTypeServiceActions.Add(new EntityTypeServiceAction { GroupId = _user.GroupId, Action = action });
                }
            }
        }

        public void DeactivateServices(Type entityType, IEnumerable<string> actions)
        {
            if (actions == null)
            {
                throw new ArgumentNullException("actions");
            }

            if (!typeof(IContent).IsAssignableFrom(entityType))
            {
                return;
            }

            entityType = GetNonProxyType(entityType);
            var type = this.EntityTypes.FirstOrDefault(t => t.Name == entityType.FullName);

            foreach (var action in actions)
            {
                var serviceAction = type.EntityTypeServiceActions.FirstOrDefault(s => s.GroupId == _user.GroupId && s.Action.ToLower() == action.ToLower());

                if (serviceAction != null)
                {
                    type.EntityTypeServiceActions.Remove(serviceAction);
                }
            }
        }

        public Type GetEntityType(Guid entityTypeId)
        {
            var type = this.EntityTypes.FirstOrDefault(t => t.Id == entityTypeId);

            if (type == null)
            {
                throw new ArgumentException(string.Format("No entity type found for id {0}", entityTypeId));
            }

            return ModuleManager.LoadedAssemblies.SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.FullName == type.Name);
        }

        public Type GetEntityType(string entityTypeName)
        {
            var type = this.EntityTypes.FirstOrDefault(t => t.Name.ToLower() == entityTypeName.ToLower());

            if (type == null)
            {
                throw new ArgumentException(string.Format("No entity type found for name {0}", entityTypeName));
            }

            return ModuleManager.LoadedAssemblies.SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.FullName == type.Name);
        }

        public Guid GetEntityTypeId(Type entityType)
        {
            var type = this.EntityTypes.FirstOrDefault(t => t.Name == entityType.FullName);

            if (type == null)
            {
                throw new ArgumentException(string.Format("No entity type found for type {0}", entityType.FullName));
            }

            return type.Id;
        }

        public string[] GetFileIdProperties(Type entityType)
        {
            return this.GetFileProperties(entityType, true);
        }

        public string[] GetFileProperties(Type entityType)
        {
            return this.GetFileProperties(entityType, false);
        }

        public ObjectMap GetObjectMap(Type entityType)
        {
            ObjectMap map;

            if (typeof(ContentBase).IsAssignableFrom(entityType))
            {
                map = _objectMaps.FirstOrDefault(m => m.ContentType == entityType);
            }
            else
            {
                map = _objectMaps.FirstOrDefault(m => m.ViewModelType == entityType);

                if (map == null)
                {
                    map = _objectMaps.FirstOrDefault(m => m.ListModelType == entityType);
                }
            }

            if (map == null)
            {
                return new ObjectMap(entityType, entityType, entityType);
            }

            return map;
        }

        public bool IsServiceActive(Type entityType, string action)
        {
            if (!typeof(IContent).IsAssignableFrom(entityType))
            {
                return false;
            }

            entityType = GetNonProxyType(entityType);
            var type = this.EntityTypes.FirstOrDefault(t => t.Name == entityType.FullName);
            return type.EntityTypeServiceActions != null && type.EntityTypeServiceActions.Any(a => a.GroupId == _user.GroupId && a.Action.ToLower() == action.ToLower());
        }

        #endregion Public Methods

        #region Private Methods

        private static void CreateObjectMaps()
        {
            var types = new List<Type>();

            foreach (var assembly in ModuleManager.LoadedAssemblies)
            {
                types.AddRange(assembly.GetTypes().Where(ty => ty.IsClass
                                                               && !ty.IsAbstract
                                                               && !ty.IsGenericType
                                                               && typeof(object).IsAssignableFrom(ty)));
            }

            foreach (var type in types)
            {
                var viewModelType = ModuleManager.LoadedAssemblies.SelectMany(a => a.GetTypes().Where(t => t.Name.ToLower() == type.Name.ToLower() + "viewmodel")).FirstOrDefault();
                var listModelType = ModuleManager.LoadedAssemblies.SelectMany(a => a.GetTypes().Where(t => t.Name.ToLower() == type.Name.ToLower() + "listmodel")).FirstOrDefault();
                viewModelType = viewModelType != null ? viewModelType : listModelType;
                listModelType = listModelType != null ? listModelType : viewModelType;

                // Todo: throw error here if a view or list model type occurs more than once?
                if (viewModelType != null || listModelType != null)
                {
                    _objectMaps.Add(new ObjectMap(type, viewModelType, listModelType));
                }
            }
        }

        private static Type GetNonProxyType(Type type)
        {
            if (type.Namespace == PlatformConstants.ENTITYFRAMEWORKPROXYTYPE)
            {
                type = type.BaseType;
            }

            return type;
        }

        private static void GetOrCreateEntityTypes()
        {
            var types = new List<Type>();

            // Create or retrieve all entity types.
            foreach (var assembly in ModuleManager.LoadedAssemblies)
            {
                types.AddRange(assembly.GetTypes().Where(ty => ty.IsClass
                                                               && !ty.IsAbstract
                                                               && !ty.IsGenericType
                                                               && typeof(IContent).IsAssignableFrom(ty)));
            }

            if (_entityTypeList.IsEmpty())
            {
                _entityTypeList = new ConcurrentBag<EntityType>();
                var source = DependencyInjector.Get<IPlatformDataSource>(PlatformConstants.STRUCTUREMAPPRIVATE);
                var entityTypes = source.Query<EntityType>().Include(e => e.EntityTypeServiceActions).ToList();
                bool typeAdded = false;

                foreach (Type type in types)
                {
                    var existingType = entityTypes.Where(et => et.Name.Equals(GetNonProxyType(type).FullName)).FirstOrDefault();

                    if (existingType == null)
                    {
                        var newType = new EntityType();
                        newType.Id = Guid.NewGuid();
                        newType.Name = type.FullName;
                        newType.EntityTypeServiceActions = new List<EntityTypeServiceAction>();
                        source.Save(newType);
                        _entityTypeList.Add(newType);
                        typeAdded = true;
                    }
                    else
                    {
                        _entityTypeList.Add(existingType);
                    }
                }

                if (typeAdded)
                {
                    source.SaveChanges();
                }
            }
        }

        private static void Init()
        {
            lock (_lockObject)
            {
                if (_isInitialized)
                {
                    return;
                }

                GetOrCreateEntityTypes();
                CreateObjectMaps();
                _isInitialized = true;
            }
        }

        private string[] GetFileProperties(Type entityType, bool getIdProperties)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException("entityType");
            }

            var fileProperties = entityType.GetProperties().Where(p =>
            {
                var attr = p.GetAttribute<FileUploadAttribute>(); return attr != null && string.IsNullOrWhiteSpace(attr.IdProperty);
            }).Select(pr => pr.Name + (getIdProperties ? "Id" : string.Empty)).ToArray();

            var nonDefaultFileProperties = entityType.GetProperties().Where(p => { var attr = p.GetAttribute<FileUploadAttribute>(); return attr != null && !string.IsNullOrWhiteSpace(attr.IdProperty); })
                .SelectMany(p => { return getIdProperties ? p.CustomAttributes.Where(a => a.AttributeType == typeof(FileUploadAttribute)).Cast<FileUploadAttribute>().Select(a => a.IdProperty) : new string[] { p.Name }; });

            return fileProperties.Union(nonDefaultFileProperties).Distinct().ToArray();
        }

        #endregion Private Methods
    }
}
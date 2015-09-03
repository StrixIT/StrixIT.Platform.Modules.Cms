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
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrixIT.Platform.Modules.Cms
{
    public class EntityHelper : IEntityHelper
    {
        #region Private Fields

        private CmsService _cmsService;
        private IUserContext _user;

        #endregion Private Fields

        #region Public Constructors

        public EntityHelper(CmsService cmsService, IUserContext user)
        {
            _cmsService = cmsService;
            _user = user;
        }

        #endregion Public Constructors

        #region Public Properties

        public IList<EntityType> EntityTypes
        {
            get
            {
                return _cmsService.EntityTypes.ToList();
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

            return DependencyInjector.GetLoadedAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.FullName == type.Name);
        }

        public Type GetEntityType(string entityTypeName)
        {
            var type = this.EntityTypes.FirstOrDefault(t => t.Name.ToLower() == entityTypeName.ToLower());

            if (type == null)
            {
                throw new ArgumentException(string.Format("No entity type found for name {0}", entityTypeName));
            }

            return DependencyInjector.GetLoadedAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.FullName == type.Name);
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
            var maps = _cmsService.ObjectMaps;
            ObjectMap map;

            if (typeof(ContentBase).IsAssignableFrom(entityType))
            {
                map = maps.FirstOrDefault(m => m.ContentType == entityType);
            }
            else
            {
                map = maps.FirstOrDefault(m => m.ViewModelType == entityType);

                if (map == null)
                {
                    map = maps.FirstOrDefault(m => m.ListModelType == entityType);
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

        private static Type GetNonProxyType(Type type)
        {
            if (type.Namespace == PlatformConstants.ENTITYFRAMEWORKPROXYTYPE)
            {
                type = type.BaseType;
            }

            return type;
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
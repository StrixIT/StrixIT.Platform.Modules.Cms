#region Apache License

//-----------------------------------------------------------------------
// <copyright file="EntityServiceManager.cs" company="StrixIT">
// Copyright 2015 StrixIT. Author R.G. Schurgers MA MSc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use file except in compliance with the License.
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
using System.Data.Entity;
using System.Linq;

namespace StrixIT.Platform.Modules.Cms
{
    public class EntityServiceManager : IEntityServiceManager
    {
        #region Private Fields

        private ICmsData _cmsData;

        #endregion Private Fields

        #region Public Constructors

        public EntityServiceManager(ICmsData cmsData)
        {
            _cmsData = cmsData;
        }

        #endregion Public Constructors

        #region Public Methods

        public EntityServiceCollection GetManagerActionRecords()
        {
            var result = new EntityServiceCollection();

            foreach (var type in _cmsData.EntityHelper.EntityTypes.OrderBy(t => t.Name))
            {
                var records = new List<ServiceActionRecord>();

                foreach (var service in _cmsData.PlatformHelper.Services)
                {
                    foreach (var action in service.Value)
                    {
                        var existingRecord = type.EntityTypeServiceActions != null ?
                            type.EntityTypeServiceActions.FirstOrDefault(r => r.Action == action) :
                            null;

                        records.Add(new ServiceActionRecord
                        {
                            Id = existingRecord != null ? existingRecord.Id : 0,
                            EntityTypeId = type.Id,
                            Action = action,
                            Service = service.Key,
                            Selected = existingRecord != null
                        });
                    }
                }

                result.Add(new Tuple<string, Guid, IList<ServiceActionRecord>>(type.Name.Split('.').Last().Replace("Entity", string.Empty), type.Id, records));
            }

            return result;
        }

        public bool SaveActions(EntityServiceCollection records)
        {
            if (records == null)
            {
                throw new ArgumentNullException("records");
            }

            bool result = true;

            Guid[] typeIds = records.Select(re => re.Item2).ToArray();
            var groupId = _cmsData.Environment.User.GroupId;
            List<EntityType> entityTypes = _cmsData.DataSource.Query<EntityType>().Include(t => t.EntityTypeServiceActions).Where(e => typeIds.Contains(e.Id)).ToList();

            foreach (var entityType in entityTypes)
            {
                var actions = entityType.EntityTypeServiceActions.Where(s => s.GroupId == groupId).ToList();
                _cmsData.DataSource.Delete(actions);

                foreach (var action in actions)
                {
                    entityType.EntityTypeServiceActions.Remove(action);
                }
            }

            foreach (var group in records)
            {
                SaveRecord(group, entityTypes, groupId);
            }

            return result;
        }

        #endregion Public Methods

        #region Private Methods

        private void SaveRecord(Tuple<string, Guid, IList<ServiceActionRecord>> group, List<EntityType> entityTypes, Guid groupId)
        {
            var entityType = entityTypes.First(e => e.Id == group.Item2);
            var type = _cmsData.EntityHelper.GetEntityType(entityType.Id);
            var selectedEntries = group.Item3.Where(i => i.Selected);
            var deselectedEntries = group.Item3.Where(i => !i.Selected);

            foreach (var record in selectedEntries)
            {
                entityType.EntityTypeServiceActions.Add(new EntityTypeServiceAction() { EntityTypeId = record.EntityTypeId, Action = record.Action, GroupId = groupId });
            }

            _cmsData.EntityHelper.DeactivateServices(type, deselectedEntries.Select(e => e.Action));
            _cmsData.EntityHelper.ActivateServices(type, selectedEntries.Select(e => e.Action));
        }

        #endregion Private Methods
    }
}
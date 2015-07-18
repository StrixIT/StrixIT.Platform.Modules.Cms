//-----------------------------------------------------------------------
// <copyright file="ServiceManagerService.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace StrixIT.Platform.Modules.Cms
{
    public class ServiceManagerService : IServiceManagerService
    {
        private IPlatformDataSource _dataSource;

        private IEntityServiceManager _manager;

        public ServiceManagerService(IPlatformDataSource dataSource, IEntityServiceManager manager)
        {
            this._dataSource = dataSource;
            this._manager = manager;
        }

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
    }
}
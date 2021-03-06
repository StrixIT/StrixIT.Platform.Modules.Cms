﻿////------------------------------------------------------------------------------
//// <auto-generated>
////     This code was not generated by a tool. but for stylecop suppression.
//// </auto-generated>
////------------------------------------------------------------------------------
using Moq;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms.Tests
{
    public class EntityManagerMock
    {
        #region Private Fields

        private Mock<ICacheService> _cacheMock = new Mock<ICacheService>();
        private DataSourceMock _dataSourceMock = new DataSourceMock();
        private IEntityManager _manager;

        #endregion Private Fields

        #region Public Constructors

        public EntityManagerMock()
        {
            _dataSourceMock.RegisterData<PlatformEntity>(EntityServicesTestData.Entities);
            _dataSourceMock.RegisterData<News>(EntityServicesTestData.Content);
            _manager = new EntityManager(_dataSourceMock.Mock.Object, _cacheMock.Object);
        }

        #endregion Public Constructors

        #region Public Properties

        public Mock<ICacheService> CacheMock
        {
            get
            {
                return _cacheMock;
            }
        }

        public DataSourceMock DataSourceMock
        {
            get
            {
                return _dataSourceMock;
            }
        }

        public IEntityManager EntityManager
        {
            get
            {
                return _manager;
            }
        }

        #endregion Public Properties
    }
}
﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was not generated by a tool. but for stylecop suppression.
// </auto-generated>
//------------------------------------------------------------------------------
using System.Linq;
using Moq;
using System.Collections.Generic;
using StrixIT.Platform.Modules.Cms;

namespace StrixIT.Platform.Modules.Cms.Tests
{
    public class TaxonomyManagerMock
    {
        private ITaxonomyManager _manager;
        private DataSourceMock _dataSourceMock = new DataSourceMock();

        public TaxonomyManagerMock()
        {
            _manager = new TaxonomyManager(_dataSourceMock.Mock.Object);
        }

        public ITaxonomyManager TaxonomyManager
        {
            get
            {
                return _manager;
            }
        }

        public DataSourceMock DataSourceMock
        {
            get
            {
                return _dataSourceMock;
            }
        }
    }
}
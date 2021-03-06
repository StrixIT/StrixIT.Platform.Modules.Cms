﻿////------------------------------------------------------------------------------
//// <auto-generated>
////     This code was not generated by a tool. but for stylecop suppression.
//// </auto-generated>
////------------------------------------------------------------------------------
using Moq;
using StrixIT.Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrixIT.Platform.Modules.Cms.Tests
{
    public class DataSourceMock
    {
        #region Private Fields

        private Dictionary<Type, List<object>> _dataLists = new Dictionary<Type, List<object>>();
        private Mock<IPlatformDataSource> _dataSourceMock = new Mock<IPlatformDataSource>();

        #endregion Private Fields

        #region Public Properties

        public Mock<IPlatformDataSource> Mock
        {
            get
            {
                return _dataSourceMock;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void AddDataItem<T>(T item)
        {
            var type = typeof(T);

            if (!_dataLists.ContainsKey(type))
            {
                throw new KeyNotFoundException();
            }

            _dataLists[type].Add(item);
        }

        public List<T> DataList<T>()
        {
            return _dataLists[typeof(T)].Cast<T>().ToList();
        }

        public void RegisterData<T>(IEnumerable<T> data, Func<T, object> keySelector = null) where T : class
        {
            var type = typeof(T);

            if (!_dataLists.ContainsKey(type))
            {
                _dataLists.Add(type, new List<T>().Cast<object>().ToList());
            }

            _dataLists[type] = data.Cast<object>().ToList();
            _dataSourceMock.Setup(s => s.Query<T>()).Returns(_dataLists[type].Cast<T>().AsQueryable());
            _dataSourceMock.Setup(s => s.Query<T>()).Returns(_dataLists[type].Cast<T>().AsQueryable());
            _dataSourceMock.Setup(s => s.Query(typeof(T))).Returns(_dataLists[type].Cast<T>().AsQueryable());
            _dataSourceMock.Setup(s => s.Query(typeof(T), It.IsAny<string>())).Returns(_dataLists[type].Cast<T>().AsQueryable());
            _dataSourceMock.Setup(s => s.GetModifiedPropertyValues(It.IsAny<object>())).Returns(new List<ModifiedPropertyValue>());
        }

        #endregion Public Methods
    }
}
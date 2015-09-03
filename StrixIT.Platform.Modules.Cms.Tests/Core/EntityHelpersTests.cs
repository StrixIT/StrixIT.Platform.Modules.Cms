﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was not generated by a tool. but for stylecop suppression.
// </auto-generated>
//------------------------------------------------------------------------------
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StrixIT.Platform.Core;
using StrixIT.Platform.Framework;
using System.Collections.Generic;
using System.Linq;

namespace StrixIT.Platform.Modules.Cms.Tests
{
    [TestClass]
    public class EntityHelperTests
    {
        #region Private Fields

        private Mock<IUserContext> _userMock;

        #endregion Private Fields

        #region Public Methods

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            CmsInitializer.ConfigureEntityMaps();
        }

        [TestMethod]
        public void GetEntityTypeIdShouldReturnCorrectId()
        {
            var helper = GetEntityHelper();
            var id = helper.GetEntityTypeId(typeof(Html));
            Assert.AreEqual(EntityServicesTestData.HtmlEntityTypeId, id);
        }

        [TestMethod]
        public void GetTypeUsingEntityIdShouldReturnCorrectType()
        {
            var helper = GetEntityHelper();
            var type = helper.GetEntityType(EntityServicesTestData.HtmlEntityTypeId);
            Assert.AreEqual(typeof(Html), type);
        }

        [TestMethod]
        public void IsServiceActiveShouldReturnFalseForNonActiveService()
        {
            var helper = GetEntityHelper();
            var result = helper.IsServiceActive(typeof(Html), "Versioning");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsServiceActiveShouldReturnTrueForActiveService()
        {
            var helper = GetEntityHelper();
            var htmlEntityType = helper.EntityTypes.First(e => e.Name == typeof(Html).FullName);
            _userMock.Setup(u => u.GroupId).Returns(htmlEntityType.EntityTypeServiceActions.First().GroupId);
            var result = helper.IsServiceActive(typeof(Html), "Translations");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MappingAnEntityToAListModelShouldMapTheUrl()
        {
            var helper = GetEntityHelper();
            var content = EntityServicesTestData.FirstNewsContentEn.Map<NewsListModel>();
            var result = content.Map<NewsListModel>();
            Assert.AreEqual(EntityServicesTestData.FirstNewsContentEn.Entity.Url, result.Url);
        }

        [TestMethod]
        public void ObjectMapShouldReturnProperObjectMap()
        {
            var helper = GetEntityHelper();
            var map = helper.GetObjectMap(typeof(News));
            Assert.AreEqual(typeof(News), map.ContentType);
            Assert.AreEqual(typeof(NewsViewModel), map.ViewModelType);
            Assert.AreEqual(typeof(NewsListModel), map.ListModelType);
        }

        #endregion Public Methods

        #region Private Methods

        private EntityHelper GetEntityHelper()
        {
            var dataSourceMock = new Mock<IPlatformDataSource>();
            var cmsService = new CmsService(dataSourceMock.Object, EntityServicesTestData.EntityTypes);
            _userMock = new Mock<IUserContext>();
            var helper = new EntityHelper(cmsService, _userMock.Object);
            return helper;
        }

        #endregion Private Methods
    }
}
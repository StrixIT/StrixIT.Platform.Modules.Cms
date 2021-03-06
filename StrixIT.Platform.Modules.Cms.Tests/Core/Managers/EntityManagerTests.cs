﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was not generated by a tool. but for stylecop suppression.
// </auto-generated>
//------------------------------------------------------------------------------
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StrixIT.Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrixIT.Platform.Modules.Cms.Tests
{
    [TestClass]
    public class EntityManagerTests
    {
        #region Private Fields

        private List<Mock> _mocks;

        #endregion Private Fields

        #region Public Methods

        [TestCleanup]
        public void Cleanup()
        {
            Logger.LoggingService = null;
        }

        [TestInitialize]
        public void Init()
        {
            _mocks = TestHelpers.MockUtilities();
            Logger.LoggingService = new Mock<ILoggingService>().Object;
        }

        #endregion Public Methods

        #region CheckName

        [TestMethod()]
        public void CheckNameShouldReturnFalseForExistingNameAndDifferentId()
        {
            var mock = new EntityManagerMock();
            var manager = mock.EntityManager;
            var existingName = EntityServicesTestData.FirstNewsContentEn.Name;
            var result = manager.IsNameAvailable(typeof(News), existingName, Guid.NewGuid());
            Assert.AreEqual(false, result);
        }

        [TestMethod()]
        public void CheckNameShouldReturnTrueForExistingNameAndSameId()
        {
            var mock = new EntityManagerMock();
            var manager = mock.EntityManager;
            var existingName = EntityServicesTestData.FirstNewsContentEn.Name;
            var result = manager.IsNameAvailable(typeof(News), existingName, EntityServicesTestData.FirstNewsEntityId);
            Assert.AreEqual(true, result);
        }

        [TestMethod()]
        public void CheckNameShouldReturnTrueForNewNameAndSameId()
        {
            var mock = new EntityManagerMock();
            var manager = mock.EntityManager;
            var newName = "New Name";
            var result = manager.IsNameAvailable(typeof(News), newName, EntityServicesTestData.FirstNewsEntityId);
            Assert.AreEqual(true, result);
        }

        #endregion CheckName

        #region New

        [TestMethod]
        public void GetNewEntityShouldReturnProperlyFilledEntity()
        {
            var mock = new EntityManagerMock();
            var news = mock.EntityManager.Get<News>(null);
            Assert.IsNotNull(news);
            Assert.IsNotNull(news.Culture);
            Assert.AreNotEqual(0, news.VersionNumber);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetNewEntityWithNonGuidKeyShouldThrowNoSupportedException()
        {
            var mock = new EntityManagerMock();
            var news = mock.EntityManager.Get(typeof(News), 1);
        }

        [TestMethod]
        public void GetNewObjectShouldReturnNewObject()
        {
            var mock = new EntityManagerMock();
            var result = mock.EntityManager.Get(typeof(File), null);
            Assert.IsNotNull(result);
        }

        #endregion New

        #region Get

        [TestMethod()]
        public void GetEntityByIdAndCultureShouldReturnCorrectEntity()
        {
            var mock = new EntityManagerMock();
            var manager = mock.EntityManager;
            var entity = manager.Get<News>(EntityServicesTestData.FirstNewsEntityId, "nl");
            Assert.IsNotNull(entity);
            Assert.AreEqual("first-news", entity.Entity.Url);
            Assert.AreEqual("Eerste nieuwsbericht", entity.Name);
            Assert.AreEqual("nl", entity.Culture);
            Assert.AreEqual(1, entity.VersionNumber);
        }

        [TestMethod()]
        public void GetEntityByIdCultureAndVersionShouldReturnCorrectEntity()
        {
            var mock = new EntityManagerMock();
            var manager = mock.EntityManager;
            var entity = manager.Get<News>(EntityServicesTestData.FirstNewsEntityId, "en", 2);
            Assert.IsNotNull(entity);
            Assert.AreEqual("first-news", entity.Entity.Url);
            Assert.AreEqual("First news", entity.Name);
            Assert.AreEqual("en", entity.Culture);
            Assert.AreEqual(2, entity.VersionNumber);
        }

        [TestMethod()]
        public void GetEntityByIdShouldReturnCorrectEntity()
        {
            var mock = new EntityManagerMock();
            var manager = mock.EntityManager;
            var entity = manager.Get<News>(EntityServicesTestData.FirstNewsEntityId);
            Assert.IsNotNull(entity);
            Assert.AreEqual("first-news", entity.Entity.Url);
            Assert.AreEqual("First news", entity.Name);
            Assert.AreEqual("en", entity.Culture);
        }

        [TestMethod()]
        public void GetEntityByUrlShouldReturnCorrectEntity()
        {
            var mock = new EntityManagerMock();
            var manager = mock.EntityManager;
            var entity = manager.Get<News>(EntityServicesTestData.FirstNewsContentEn.Entity.Url);
            Assert.IsNotNull(entity);
            Assert.AreEqual("first-news", entity.Entity.Url);
            Assert.AreEqual("First news", entity.Name);
            Assert.AreEqual("en", entity.Culture);
            Assert.AreEqual(1, entity.VersionNumber);
        }

        #endregion Get

        #region Query

        [TestMethod()]
        public void QueryByTagShouldReturnAllNonDeletedCurrentContentWithTheCurrentCultureFilteredByTag()
        {
            var mock = new EntityManagerMock();
            var manager = mock.EntityManager;
            var result = manager.QueryByTag<News>("Test");
            Assert.AreEqual(1, result.Count());
        }

        [TestMethod()]
        public void QueryShouldReturnAllNonDeletedContent()
        {
            var mock = new EntityManagerMock();
            var manager = mock.EntityManager;
            var result = manager.Query<News>();
            Assert.AreEqual(7, result.Count());
        }

        #endregion Query

        #region Process Path

        [TestMethod()]
        public void ProcessPathShouldAddIndexForAlreadyExistingPath()
        {
            var mock = new EntityManagerMock();
            IContent news = null;
            mock.DataSourceMock.Mock.Setup(m => m.Save<IContent>(It.IsAny<IContent>())).Returns<IContent>(n => { news = n; return n; });
            var newsA = GetNew<News>();
            newsA.Body = "News";
            newsA.Entity.Url = "test";
            var newsB = GetNew<News>();
            newsB.Body = "News";
            newsB.Entity.Url = "test-2";
            mock.DataSourceMock.RegisterData<News>(new List<News> { newsA, newsB });
            var entity = GetNew<News>();
            entity.Name = "Test";
            entity.Body = "News";
            mock.EntityManager.Save(entity);
            string expected = "test-3";
            string actual = news.Entity.Url;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ProcessPathShouldCreateCorrectUrl()
        {
            var mock = new EntityManagerMock();
            IContent news = null;
            mock.DataSourceMock.Mock.Setup(m => m.Save<IContent>(It.IsAny<IContent>())).Returns<IContent>(n => { news = n; return n; });
            var entity = GetNew<News>();
            entity.Name = "Testing";
            entity.Body = "News";
            mock.EntityManager.Save(entity);
            string expected = "testing";
            string actual = news.Entity.Url;
            Assert.AreEqual(expected, actual);
        }

        #endregion Process Path

        #region Save

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AnExceptionShouldBeThrownWhenCreatingANewContentTranslationWithoutTranslationsEnabled()
        {
            var mock = new EntityManagerMock();
            var entity = EntityServicesTestData.FirstNewsContentEn;
            entity.Culture = "fr";
            var result = mock.EntityManager.Save(entity);
        }

        [TestMethod()]
        public void ForANewContentTranslationANewContentBasedOnTheDefaultCultureShouldBeCreated()
        {
            var mock = new EntityManagerMock();
            IContent translation = null;
            var helperMock = _mocks.First(m => m.GetType().Equals(typeof(Mock<IEntityHelper>))) as Mock<IEntityHelper>;
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.Translations)).Returns(true);
            mock.DataSourceMock.Mock.Setup(m => m.Save<IContent>(It.IsAny<IContent>())).Returns<IContent>(n => { translation = n; return n; });
            var entity = EntityServicesTestData.FirstNewsContentEn;
            entity.Culture = "fr";
            entity.Name = "Francais";
            entity.Body = "La Marseillaise";
            var result = mock.EntityManager.Save(entity);
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.Translations)).Returns(false);
            mock.DataSourceMock.Mock.Verify(d => d.Save<IContent>(It.IsAny<IContent>()), Times.Once());
            Assert.IsNotNull(result);
            Assert.IsNotNull(translation);
            Assert.AreEqual(EntityServicesTestData.FirstNewsEntityId, translation.EntityId);
            Assert.AreEqual("fr", translation.Culture);
            Assert.AreEqual("Francais", translation.Name);
            Assert.AreEqual("La Marseillaise", ((News)translation).Body);
        }

        [TestMethod()]
        public void ForANewContentTranslationANewContentBasedOnTheFirstAvailableCultureWhenNoTranslationIsPresentForTheDefaultCultureShouldBeCreated()
        {
            var mock = new EntityManagerMock();
            IContent translation = null;
            var helperMock = _mocks.First(m => m.GetType().Equals(typeof(Mock<IEntityHelper>))) as Mock<IEntityHelper>;
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.Translations)).Returns(true);
            mock.DataSourceMock.Mock.Setup(m => m.Save<IContent>(It.IsAny<IContent>())).Returns<IContent>(n => { translation = n; return n; });
            var entity = EntityServicesTestData.ThirdNewsContentDe;
            entity.Culture = "fr";
            entity.Name = "Francais";
            entity.Body = "La Marseillaise";
            var result = mock.EntityManager.Save(entity);
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.Translations)).Returns(false);
            mock.DataSourceMock.Mock.Verify(d => d.Save<IContent>(It.IsAny<IContent>()), Times.Once());
            Assert.IsNotNull(result);
            Assert.IsNotNull(translation);
            Assert.AreEqual(EntityServicesTestData.ThirdNewsEntityId, translation.EntityId);
            Assert.AreEqual("fr", translation.Culture);
            Assert.AreEqual("Francais", translation.Name);
            Assert.AreEqual("La Marseillaise", ((News)translation).Body);
        }

        [TestMethod()]
        public void ForNewContentANewEntityShouldBeCreated()
        {
            var mock = new EntityManagerMock();
            IContent news = null;
            mock.DataSourceMock.Mock.Setup(m => m.Save<IContent>(It.IsAny<IContent>())).Returns<IContent>(n => { news = n; return n; });
            var entity = mock.EntityManager.Get<News>(null);
            entity.Name = "Testing";
            entity.Body = "News";
            var result = mock.EntityManager.Save(entity);
            mock.DataSourceMock.Mock.Verify(d => d.Save<IContent>(It.IsAny<IContent>()), Times.Once());
            Assert.IsNotNull(result);
            Assert.IsNotNull(news.Entity);
            Assert.AreNotEqual(Guid.Empty, news.EntityId);
        }

        [TestMethod()]
        public void UpdatingAModifiedContentItemWithVersioningActiveShouldCreateANewVersion()
        {
            var mock = new EntityManagerMock();
            IContent version = null;
            var helperMock = _mocks.First(m => m.GetType().Equals(typeof(Mock<IEntityHelper>))) as Mock<IEntityHelper>;
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.AutomaticVersions)).Returns(true);
            mock.DataSourceMock.Mock.Setup(m => m.GetModifiedPropertyValues(It.IsAny<News>())).Returns(new List<ModifiedPropertyValue> { new ModifiedPropertyValue() });
            mock.DataSourceMock.Mock.Setup(m => m.Save<IContent>(It.IsAny<IContent>())).Returns<IContent>(n => { version = n; return n; });
            var entity = EntityServicesTestData.FirstNewsContentEn;
            entity.Body = "This is a new version of this news.";
            entity.PublishedOn = DateTime.Now;
            var result = mock.EntityManager.Save(entity);
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.Translations)).Returns(false);
            mock.DataSourceMock.Mock.Verify(d => d.Save<IContent>(It.IsAny<IContent>()), Times.Once());
            Assert.IsNotNull(result);
            Assert.IsNotNull(version);
            Assert.AreEqual(EntityServicesTestData.FirstNewsEntityId, version.EntityId);
            Assert.AreEqual("This is a new version of this news.", ((News)version).Body);
            Assert.AreEqual(2, version.VersionNumber);
            Assert.IsTrue(version.IsCurrentVersion);
        }

        #endregion Save

        #region Delete

        [TestMethod]
        public void DeleteAnEntityByIdDeletesTheEntityAndAllItsContentWhenTrashIsNotActive()
        {
            var mock = new EntityManagerMock();
            mock.EntityManager.Delete<News>(EntityServicesTestData.SecondNewsEntityId);
            mock.DataSourceMock.Mock.Verify(d => d.Delete(It.IsAny<PlatformEntity>()), Times.Once());
        }

        [TestMethod]
        public void DeleteAnEntityByIdMarksTheEntityContentAsDeletedWhenTrashIsActive()
        {
            var mock = new EntityManagerMock();
            var helperMock = _mocks.First(m => m.GetType().Equals(typeof(Mock<IEntityHelper>))) as Mock<IEntityHelper>;
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.Trashbin)).Returns(true);
            mock.EntityManager.Delete<News>(EntityServicesTestData.SecondNewsEntityId);
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.Trashbin)).Returns(false);
            var undeleted = mock.DataSourceMock.DataList<News>().Any(n => n.EntityId == EntityServicesTestData.SecondNewsEntityId && !n.DeletedOn.HasValue);
            Assert.IsFalse(undeleted);
        }

        [TestMethod]
        public void DeleteContentByCultureAndVersionNumberDeletesTheContentForThatCultureAndVersionNumberWhenAlreadyMarkedAsDeletedWhenTrashIsActive()
        {
            var mock = new EntityManagerMock();
            var helperMock = _mocks.First(m => m.GetType().Equals(typeof(Mock<IEntityHelper>))) as Mock<IEntityHelper>;
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.Trashbin)).Returns(true);
            mock.EntityManager.Delete<News>(EntityServicesTestData.FirstNewsEntityId, "en", 3);
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.Trashbin)).Returns(false);
            mock.DataSourceMock.Mock.Verify(d => d.Delete(It.IsAny<IContent>()), Times.Once());
        }

        [TestMethod]
        public void DeleteContentByCultureAndVersionNumberDeletesTheContentForThatCultureAndVersionNumberWhenTrashIsNotActive()
        {
            var mock = new EntityManagerMock();
            mock.EntityManager.Delete<News>(EntityServicesTestData.FirstNewsEntityId, "en", 1);
            mock.DataSourceMock.Mock.Verify(d => d.Delete(It.IsAny<IContent>()), Times.Once());
        }

        [TestMethod]
        public void DeleteContentByCultureDeletesAllTheContentForThatCultureWhenTrashIsNotActive()
        {
            var mock = new EntityManagerMock();
            mock.EntityManager.Delete<News>(EntityServicesTestData.FirstNewsEntityId, "en");
            mock.DataSourceMock.Mock.Verify(d => d.Delete(It.IsAny<object>()), Times.Exactly(3));
        }

        [TestMethod]
        public void DeleteContentByCultureMarksAsDeletedAllTheContentForThatCultureWhenTrashIsActive()
        {
            var mock = new EntityManagerMock();
            var helperMock = _mocks.First(m => m.GetType().Equals(typeof(Mock<IEntityHelper>))) as Mock<IEntityHelper>;
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.Trashbin)).Returns(true);
            mock.EntityManager.Delete<News>(EntityServicesTestData.FirstNewsEntityId, "en");
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.Trashbin)).Returns(false);
            var undeleted = mock.DataSourceMock.DataList<News>().Any(n => n.EntityId == EntityServicesTestData.FirstNewsEntityId && !n.DeletedOn.HasValue && n.Culture == "en");
            Assert.IsFalse(undeleted);
        }

        #endregion Delete

        #region Private Methods

        private static T GetNew<T>() where T : class, IContent
        {
            var currentUserId = Guid.NewGuid();
            var groupId = Guid.NewGuid();

            var entity = new PlatformEntity();
            entity.EntityTypeId = EntityHelper.GetEntityTypeId(typeof(T));
            entity.GroupId = groupId;
            entity.OwnerUserId = currentUserId;

            var content = Activator.CreateInstance(typeof(T)) as T;
            content.Culture = "en";
            content.VersionNumber = 1;
            content.IsCurrentVersion = true;

            content.CreatedByUserId = currentUserId;
            content.UpdatedByUserId = currentUserId;
            content.CreatedOn = DateTime.Now;
            content.UpdatedOn = content.CreatedOn;
            content.EntityId = entity.Id;
            content.Entity = entity;

            return content;
        }

        #endregion Private Methods
    }
}
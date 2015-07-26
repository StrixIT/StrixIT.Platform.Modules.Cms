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
    public class EntityServiceTests
    {
        #region Private Fields

        private List<Mock> _mocks;

        #endregion Private Fields

        #region Public Methods

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            CmsInitializer.ConfigureEntityMaps();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Logger.LoggingService = null;
        }

        [TestInitialize]
        public void Init()
        {
            _mocks = TestHelpers.MockUtilities();
            var entityHelperMock = _mocks.First(m => m.GetType().Equals(typeof(Mock<IEntityHelper>))) as Mock<IEntityHelper>;
            entityHelperMock.Setup(m => m.GetObjectMap(typeof(News))).Returns(new ObjectMap(typeof(News), typeof(NewsViewModel), typeof(NewsListModel)));
            entityHelperMock.Setup(m => m.GetObjectMap(typeof(NewsViewModel))).Returns(new ObjectMap(typeof(News), typeof(NewsViewModel), typeof(NewsListModel)));
            entityHelperMock.Setup(m => m.GetObjectMap(typeof(NewsListModel))).Returns(new ObjectMap(typeof(News), typeof(NewsViewModel), typeof(NewsListModel)));
            entityHelperMock.Setup(m => m.GetEntityType(It.IsAny<Guid>())).Returns(typeof(News));
            Logger.LoggingService = new Mock<ILoggingService>().Object;
        }

        #endregion Public Methods

        #region IsNameAvailable

        [TestMethod]
        public void ExistsShouldReturnFalseForAvailableName()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            var result = mock.EntityService.Exists("New Name", EntityServicesTestData.FirstNewsContentEnId);
            Assert.IsFalse(result);
        }

        #endregion IsNameAvailable

        #region Get

        [TestMethod]
        public void GetByIdWithCultureAndNonExistentVersionNumberShouldReturnEmptyViewModelWithEntityIdSet()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            var model = mock.EntityService.Get(EntityServicesTestData.FirstNewsEntityId, "en", 4);
            Assert.IsNotNull(model);
            Assert.AreEqual(EntityServicesTestData.FirstNewsEntityId, model.EntityId);
        }

        [TestMethod]
        public void GetByIdWithCultureAndVersionNumberShouldReturnExistingEntity()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            var model = mock.EntityService.Get(EntityServicesTestData.FirstNewsEntityId, "en", 2);
            Assert.IsNotNull(model);
            Assert.AreEqual("en", model.Culture);
            Assert.IsFalse(model.IsCurrentVersion);
            Assert.AreEqual(2, model.VersionNumber);
        }

        [TestMethod]
        public void GetByUrlWithCultureAndVersionNumberShouldReturnExistingEntity()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            var model = mock.EntityService.Get(EntityServicesTestData.FirstNewsContentEn.Entity.Url, "en", 2);
            Assert.IsNotNull(model);
            Assert.AreEqual("en", model.Culture);
            Assert.IsFalse(model.IsCurrentVersion);
            Assert.AreEqual(2, model.VersionNumber);
        }

        [TestMethod]
        public void GetNewShouldReturnProperlyFilledViewModel()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            var model = mock.EntityService.Get(null);
            Assert.IsNotNull(model);
            Assert.IsNotNull(model.Culture);
            Assert.IsTrue(model.IsCurrentVersion);
            Assert.AreNotEqual(0, model.VersionNumber);
        }

        #endregion Get

        #region Cache

        [TestMethod]
        public void GetFromCacheShouldHitDatabaseFirstTimeAndRetrieveFromCacheSecondTime()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            var entity = mock.EntityManagerMock.DataSourceMock.DataList<News>().First(n => n.Entity.Url == EntityServicesTestData.FirstNewsContentEn.Entity.Url);
            var url = entity.Entity.Url;
            var helperMock = _mocks.First(m => m.GetType().Equals(typeof(Mock<IEntityHelper>))) as Mock<IEntityHelper>;
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.Translations)).Returns(true);
            var model = mock.EntityService.GetCached(url);
            mock.CacheMock.Setup(c => c[string.Format(CmsConstants.CONTENTPERCULTURE, typeof(NewsViewModel).Name)]).Returns(new List<Tuple<string, string, dynamic>> { new Tuple<string, string, dynamic>(url, "en", entity.Map<NewsViewModel>()) });
            var model2 = mock.EntityService.GetCached(url);
            mock.CacheMock.Setup(c => c[string.Format(CmsConstants.CONTENTPERCULTURE, typeof(NewsViewModel).Name)]).Returns((List<Tuple<string, string, dynamic>>)null);
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.Translations)).Returns(false);
            Assert.IsNotNull(model);
            Assert.IsNotNull(model2);
            Assert.AreEqual(3, model.AvailableCultures.Count());
            // The database is hit twice. Once to get the entity, once to get the available cultures.
            mock.EntityManagerMock.DataSourceMock.Mock.Verify(d => d.Query(typeof(News), It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void GetNonExistentEntityFromCacheShouldReturnViewModelWithUrlSet()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            var model = mock.EntityService.GetCached("test");
            Assert.IsNotNull(model);
            Assert.IsNotNull(model.Culture);
            Assert.IsTrue(model.IsCurrentVersion);
            Assert.AreNotEqual(0, model.VersionNumber);
            Assert.AreEqual("test", model.Url);
        }

        #endregion Cache

        #region List

        [TestMethod]
        public void GetListShouldReturnCorrectlyFilledList()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            var list = mock.EntityService.List(new FilterOptions());
            Assert.IsNotNull(list);
            Assert.AreEqual(2, list.Length());
        }

        #endregion List

        #region Save

        [TestMethod]
        public void SaveExistingEntityShouldCorrectlySaveExistingEntity()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            IContent news = null;
            mock.EntityManagerMock.DataSourceMock.Mock.Setup(d => d.Save(It.IsAny<IContent>())).Returns<IContent>(n => { news = n; return n; });
            var model = EntityServicesTestData.FirstNewsContentEn.Map<NewsViewModel>();
            var result = mock.EntityService.Save(model);
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void SaveNewEntityShouldCorrectlyCreateNewEntity()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            IContent news = null;
            mock.EntityManagerMock.DataSourceMock.Mock.Setup(d => d.Save(It.IsAny<IContent>())).Returns<IContent>(n => { news = n; return n; });
            var model = new NewsViewModel { Name = "Test", Body = "Test", Url = "Test" };
            var result = mock.EntityService.Save(model);
            Assert.IsTrue(result.Success);
            Assert.AreEqual("test", news.Entity.Url);
        }

        #endregion Save

        #region Delete

        [TestMethod]
        public void DeletingAnExistingEntityShouldHitDelete()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            mock.EntityService.Delete(EntityServicesTestData.FirstNewsEntityId);
            mock.EntityManagerMock.DataSourceMock.Mock.Verify(d => d.Delete<PlatformEntity>(It.IsAny<PlatformEntity>()), Times.Once());
        }

        [TestMethod]
        public void DeletingANonExistingEntityShouldNotHitDelete()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            mock.EntityService.Delete(Guid.NewGuid());
            mock.EntityManagerMock.DataSourceMock.Mock.Verify(d => d.Delete<PlatformEntity>(It.IsAny<PlatformEntity>()), Times.Never());
        }

        #endregion Delete

        #region Versions

        [TestMethod]
        public void GetVersionListShouldReturnProperlyFilledList()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            var platformHelperMock = _mocks.First(m => m.GetType().Equals(typeof(Mock<IPlatformHelper>))) as Mock<IPlatformHelper>;
            platformHelperMock.Setup(m => m.GetUserName(It.IsAny<Guid>())).Returns("TestUser");
            var list = mock.EntityService.GetVersionList(EntityServicesTestData.FirstNewsEntityId, null, null);
            Assert.IsNotNull(list);
            Assert.AreEqual(2, list.Count());
            Assert.AreEqual(2, list.First().VersionNumber);
            Assert.IsNotNull(list.Last().CreatedBy);
        }

        [TestMethod]
        public void RestoreVerionListShouldSetCorrectEntityVersionToIsCurrentAndWriteToLog()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            var helperMock = _mocks.First(m => m.GetType().Equals(typeof(Mock<IEntityHelper>))) as Mock<IEntityHelper>;
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.AutomaticVersions)).Returns(true);
            mock.EntityManagerMock.DataSourceMock.Mock.Setup(m => m.Save<IContent>(It.IsAny<IContent>())).Returns<IContent>(n => { return n; });
            var data = mock.EntityManagerMock.DataSourceMock.DataList<News>().Where(e => e.EntityId == EntityServicesTestData.FirstNewsEntityId && e.Culture == "en");
            var firstVersion = data.First(e => e.VersionNumber == 1);
            var secondVersion = data.First(e => e.VersionNumber == 2);
            var ThirdVersion = data.First(e => e.VersionNumber == 3);
            firstVersion.IsCurrentVersion = false;
            firstVersion.PublishedOn = DateTime.Now.AddDays(-10);
            secondVersion.IsCurrentVersion = true;
            ThirdVersion.DeletedOn = null;
            mock.EntityManagerMock.DataSourceMock.Mock.Setup(d => d.GetModifiedPropertyValues(It.IsAny<News>())).Returns(() => { return new List<ModifiedPropertyValue>() { new ModifiedPropertyValue { PropertyName = "Body", NewValue = "", OldValue = "" } }; });
            var model = mock.EntityService.RestoreVersion(EntityServicesTestData.FirstNewsEntityId, 1, "Test message");
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.Translations)).Returns(false);
            Assert.IsNotNull(model);
            Assert.IsTrue(model.IsCurrentVersion);
            Assert.AreEqual(3, model.VersionNumber);
            Assert.AreEqual(4, ThirdVersion.VersionNumber);
            Assert.IsFalse(secondVersion.IsCurrentVersion);
            Assert.AreEqual("This is our first news item", model.Body);
            Assert.AreEqual("Test message", model.VersionLog);
        }

        #endregion Versions

        #region Update File Tags

        [TestMethod()]
        public void UpdateFileTagsForRteShouldAddNewTagToNewFile()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            mock.EntityManagerMock.DataSourceMock.RegisterData<File>(EntityServicesTestData.Files);
            mock.TaxonomyManagerMock.DataSourceMock.RegisterData<File>(EntityServicesTestData.Files);
            mock.EntityManagerMock.DataSourceMock.Mock.Setup(m => m.Save<IContent>(It.IsAny<IContent>())).Returns<IContent>(n => { return n; });
            var entity = GetNew<News, NewsViewModel>();
            entity.Name = "Testing";
            entity.Body = string.Format(@"<p>News with a diver image<p><img src=""{0}.jpg"" />", EntityServicesTestData.DiverImageId);
            var file = mock.TaxonomyManagerMock.DataSourceMock.DataList<File>().First(f => f.Id == EntityServicesTestData.DiverImageId);
            mock.EntityService.Save(entity);
            Assert.AreEqual(1, file.Tags.Count());
            Assert.IsNotNull(file.Tags.First());
        }

        [TestMethod()]
        public void UpdateFileTagsForRteShouldAddNewTagToNewFileAndNoneToFileThatHasTheTagAlready()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            mock.EntityManagerMock.DataSourceMock.RegisterData<File>(EntityServicesTestData.Files);
            mock.TaxonomyManagerMock.DataSourceMock.RegisterData<File>(EntityServicesTestData.Files);
            mock.EntityManagerMock.DataSourceMock.Mock.Setup(m => m.Save<IContent>(It.IsAny<IContent>())).Returns<IContent>(n => { return n; });
            var helperMock = _mocks.First(m => m.GetType().Equals(typeof(Mock<IEntityHelper>))) as Mock<IEntityHelper>;
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.AutomaticVersions)).Returns(true);
            mock.EntityManagerMock.DataSourceMock.Mock.Setup(m => m.GetModifiedPropertyValues(It.IsAny<News>())).Returns(new List<ModifiedPropertyValue> { new ModifiedPropertyValue() });
            var entity = mock.EntityManagerMock.DataSourceMock.DataList<News>().First().Map<NewsViewModel>();
            var fileA = mock.TaxonomyManagerMock.DataSourceMock.DataList<File>().First(f => f.Id == EntityServicesTestData.DiverImageId);
            var fileB = mock.TaxonomyManagerMock.DataSourceMock.DataList<File>().First(f => f.Id == EntityServicesTestData.KarateImageId);
            fileA.Tags.Add(new Term { Name = (typeof(News) + "_" + entity.Id.ToString()).ToLower() });
            entity.Name = "Testing";
            entity.Body = string.Format(@"<p>News with a diver image</p><img src=""{0}.jpg"" /><p>Also a karate image</p><img src=""{1}.png"" />", EntityServicesTestData.DiverImageId, EntityServicesTestData.KarateImageId);
            mock.EntityService.Save(entity);
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.AutomaticVersions)).Returns(false);
            Assert.AreEqual(1, fileA.Tags.Count());
            Assert.IsNotNull(fileA.Tags.First());
            Assert.AreEqual(1, fileB.Tags.Count());
            Assert.IsNotNull(fileB.Tags.First());
        }

        [TestMethod()]
        public void UpdateFileTagsForRteShouldRemoveFileTagsFromFileWhenNoLongerUsedInEntity()
        {
            var mock = new EntityServiceMock<NewsViewModel>();
            mock.EntityManagerMock.DataSourceMock.RegisterData<File>(EntityServicesTestData.Files);
            mock.TaxonomyManagerMock.DataSourceMock.RegisterData<File>(EntityServicesTestData.Files);
            mock.TaxonomyManagerMock.DataSourceMock.RegisterData<Term>(EntityServicesTestData.Terms);
            var helperMock = _mocks.First(m => m.GetType().Equals(typeof(Mock<IEntityHelper>))) as Mock<IEntityHelper>;
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.AutomaticVersions)).Returns(true);
            mock.EntityManagerMock.DataSourceMock.Mock.Setup(m => m.GetModifiedPropertyValues(It.IsAny<News>())).Returns(new List<ModifiedPropertyValue> { new ModifiedPropertyValue() });
            mock.EntityManagerMock.DataSourceMock.Mock.Setup(m => m.Save<IContent>(It.IsAny<IContent>())).Returns<IContent>(n => { return n; });
            var fileA = mock.TaxonomyManagerMock.DataSourceMock.DataList<File>().First(f => f.Id == EntityServicesTestData.DiverImageId);
            var fileB = mock.TaxonomyManagerMock.DataSourceMock.DataList<File>().First(f => f.Id == EntityServicesTestData.KarateImageId);
            var entity = mock.EntityManagerMock.DataSourceMock.DataList<News>().First();
            entity.Body = string.Format(@"<p>News with a diver image</p><img src=""{0}.jpg"" /><p>Also a karate image</p><img src=""{1}.png"" />", EntityServicesTestData.DiverImageId, EntityServicesTestData.KarateImageId);
            var model = entity.Map<NewsViewModel>();
            model.Summary = null;
            var tag = mock.TaxonomyManagerMock.DataSourceMock.DataList<Term>().First(t => t.Name == EntityServicesTestData.FirstNewsEntityId.ToString() + "_" + EntityServicesTestData.FirstNewsContentEnId.ToString());
            tag.TaggedFiles.Add(fileA);
            tag.TaggedFiles.Add(fileB);
            model.Name = "Testing";
            model.Body = string.Format(@"<p>News with a diver image</p><img src=""{0}.jpg"" />", EntityServicesTestData.DiverImageId);
            mock.EntityService.Save(model);
            helperMock.Setup(m => m.IsServiceActive(typeof(News), EntityServiceActions.AutomaticVersions)).Returns(false);
            Assert.AreEqual(1, fileA.Tags.Count());
            Assert.IsNotNull(fileA.Tags.First());
            Assert.AreEqual(0, fileB.Tags.Count());
        }

        #endregion Update File Tags

        #region Private Methods

        private static V GetNew<T, V>() where T : class, IContent
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

            return content.Map<V>();
        }

        #endregion Private Methods
    }
}
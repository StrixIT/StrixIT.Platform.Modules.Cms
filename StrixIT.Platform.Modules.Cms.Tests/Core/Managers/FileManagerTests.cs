﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was not generated by a tool. but for stylecop suppression.
// </auto-generated>
//------------------------------------------------------------------------------
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace StrixIT.Platform.Modules.Cms.Tests
{
    [TestClass]
    public class FileManagerTests
    {
        [TestMethod()]
        public void IsImageShouldReturnTrueForImageExtensionTest()
        {
            var manager = GetManager();
            string extension = "jpg";
            bool expected = true;
            bool actual;
            actual = manager.IsImage(extension);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void IsImageShouldReturnFalseForNonImageExtensionTest()
        {
            var manager = GetManager();
            string extension = "mpg";
            bool expected = false;
            bool actual;
            actual = manager.IsImage(extension);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void IsAllowedShouldReturnTrueForAllowedFileType()
        {
            var manager = GetManager();
            var result = manager.IsFileAllowed("test.jpg");
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsAllowedShouldReturnFalseForNonAllowedFileType()
        {
            var manager = GetManager();
            var result = manager.IsFileAllowed("test.doc");
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void IsAllowedShouldReturnTrueWhenExtensionIsPassedAsAdditionalAllowedExtension()
        {
            var manager = GetManager();
            var result = manager.IsFileAllowed("test.doc", new string[] { "doc" });
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsImageShouldReturnTrueForImageExtension()
        {
            var manager = GetManager();
            var result = manager.IsImage("test.jpg");
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void IsImageShouldReturnFalseForNonImageExtension()
        {
            var manager = GetManager();
            var result = manager.IsImage("test.docx");
            Assert.IsFalse(result);
        }

        private IFileManager GetManager()
        {
            return new FileManager(new Mock<IPlatformDataSource>().Object, new Mock<IImageConverter>().Object);
        }
    }
}
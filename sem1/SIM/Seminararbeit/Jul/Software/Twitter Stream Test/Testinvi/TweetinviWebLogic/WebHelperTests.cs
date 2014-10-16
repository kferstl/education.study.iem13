using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.WebLogic;

namespace Testinvi.TweetinviWebLogic
{
    [TestClass]
    public class WebHelperTests
    {
        private FakeClassBuilder<WebHelper> _fakeBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<WebHelper>();
        }

        [TestMethod]
        public void GetResponseStream_UrlIsNull_ReturnsNull()
        {
            // Arrange
            var webHelper = CreateWebHelper();

            // Act
            var result = webHelper.GetResponseStream((string)null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetResponseStream_UrlIsEmpty_ReturnsNull()
        {
            // Arrange
            var webHelper = CreateWebHelper();

            // Act
            var result = webHelper.GetResponseStream(String.Empty);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetResponseStream_UrlIsGoogle_ReturnsAStream()
        {
            // Arrange
            var webHelper = CreateWebHelper();

            // Act
            var result = webHelper.GetResponseStream("http://www.google.com");

            // Assert
            Assert.IsNotNull(result);
        }

        public WebHelper CreateWebHelper()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
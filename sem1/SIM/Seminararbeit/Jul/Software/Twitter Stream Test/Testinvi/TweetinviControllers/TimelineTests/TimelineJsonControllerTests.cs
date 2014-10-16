using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Timeline;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Testinvi.TweetinviControllers.TimelineTests
{
    [TestClass]
    public class TimelineJsonControllerTests
    {
        private FakeClassBuilder<TimelineJsonController> _fakeBuilder;
        private Fake<ITimelineQueryGenerator> _fakeTimelineQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;
        private Fake<IUserFactory> _fakeUserFactory;
        private Fake<IFactory<IHomeTimelineRequestParameters>> _fakeHomeTimelineRequestParameterFactory;
        private Fake<IFactory<IUserTimelineRequestParameters>> _fakeUserTimelineRequestParameterFactory;
        private Fake<IFactory<IMentionsTimelineRequestParameters>> _fakeMentionsTimelineRequestParameterFactory;

        private IHomeTimelineRequestParameters _fakeHomeTimelineRequestParameters;
        private IUserTimelineRequestParameters _fakeUserTimelineRequestParameters;
        private IMentionsTimelineRequestParameters _fakeMentionsTimelineRequestParameters;

        private int _maximuNumberOfTweets;
        private string _expectedQuery;
        private string _expectedResult;
        private string _userName;
        private long _userId;

        private IUser _fakeUser;
        private IUserDTO _fakeUserDTO;
        private IUserIdentifier _fakeUserIdentifier;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TimelineJsonController>();
            _fakeTimelineQueryGenerator = _fakeBuilder.GetFake<ITimelineQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
            _fakeUserFactory = _fakeBuilder.GetFake<IUserFactory>();
            _fakeHomeTimelineRequestParameterFactory = _fakeBuilder.GetFake<IFactory<IHomeTimelineRequestParameters>>();
            _fakeUserTimelineRequestParameterFactory = _fakeBuilder.GetFake<IFactory<IUserTimelineRequestParameters>>();
            _fakeMentionsTimelineRequestParameterFactory = _fakeBuilder.GetFake<IFactory<IMentionsTimelineRequestParameters>>();
        
            InitData();

            _fakeTwitterAccessor.CallsTo(x => x.ExecuteJsonGETQuery(_expectedQuery)).Returns(_expectedResult);
            
            _fakeUserFactory.CallsTo(x => x.GetUserIdentifierFromUser(_fakeUser)).Returns(_fakeUserIdentifier);
            _fakeUserFactory.CallsTo(x => x.GenerateUserIdentifierFromScreenName(_userName)).Returns(_fakeUserIdentifier);
            _fakeUserFactory.CallsTo(x => x.GenerateUserIdentifierFromId(_userId)).Returns(_fakeUserIdentifier);

            _fakeHomeTimelineRequestParameterFactory.CallsTo(x => x.Create()).Returns(_fakeHomeTimelineRequestParameters);
            _fakeUserTimelineRequestParameterFactory.CallsTo(x => x.Create()).Returns(_fakeUserTimelineRequestParameters);
            _fakeMentionsTimelineRequestParameterFactory.CallsTo(x => x.Create()).Returns(_fakeMentionsTimelineRequestParameters);
        }

        private void InitData()
        {
            _fakeHomeTimelineRequestParameters = A.Fake<IHomeTimelineRequestParameters>();
            _fakeUserTimelineRequestParameters = A.Fake<IUserTimelineRequestParameters>();
            _fakeMentionsTimelineRequestParameters = A.Fake<IMentionsTimelineRequestParameters>();

            _maximuNumberOfTweets = TestHelper.GenerateRandomInt();
            _expectedQuery = TestHelper.GenerateString();
            _expectedResult = TestHelper.GenerateString();
            _userName = TestHelper.GenerateString();
            _userId = TestHelper.GenerateRandomLong();

            _fakeUser = A.Fake<IUser>();
            _fakeUserDTO = A.Fake<IUserDTO>();
            _fakeUserIdentifier = _fakeUserDTO;
        }

        #region GetHomeTimeline

        [TestMethod]
        public void GetHomeTimeline_FromMaximumNumber_ReturnsExpectedResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();

            _fakeTimelineQueryGenerator.CallsTo(x => x.GetHomeTimelineQuery(_fakeHomeTimelineRequestParameters)).Returns(_expectedQuery);

            // Act
            var result = controller.GetHomeTimeline(_maximuNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _expectedResult);
            Assert.AreEqual(_fakeHomeTimelineRequestParameters.MaximumNumberOfTweetsToRetrieve, _maximuNumberOfTweets);
        }

        [TestMethod]
        public void GetHomeTimeline_FromTimelineRequestParameter_ReturnsExpectedResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();

            _fakeTimelineQueryGenerator.CallsTo(x => x.GetHomeTimelineQuery(_fakeHomeTimelineRequestParameters)).Returns(_expectedQuery);

            // Act
            var result = controller.GetHomeTimeline(_fakeHomeTimelineRequestParameters);

            // Assert
            Assert.AreEqual(result, _expectedResult);
        }

        #endregion

        #region GetMentionsTimeline

        [TestMethod]
        public void GetMentionsTimeline_ExcludeRepliesIsTrue_ReturnsTAResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();
            var maximumTweets = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedResult = GetQueryResult(expectedQuery);

            _fakeTimelineQueryGenerator.CallsTo(x => x.GetMentionsTimelineQuery(_fakeMentionsTimelineRequestParameters)).Returns(expectedQuery);

            // Act
            var result = controller.GetMentionsTimeline(maximumTweets);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetMentionsTimeline_ExcludeRepliesIsFalse_ReturnsTAResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();
            var maximumTweets = TestHelper.GenerateRandomInt();
            var expectedQuery = TestHelper.GenerateString();
            var expectedResult = GetQueryResult(expectedQuery);

            _fakeTimelineQueryGenerator.CallsTo(x => x.GetMentionsTimelineQuery(_fakeMentionsTimelineRequestParameters)).Returns(expectedQuery);

            // Act
            var result = controller.GetMentionsTimeline(maximumTweets);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region GetUserTimeline

        [TestMethod]
        public void GetUserTimelineWithUser_FromUser_ReturnsResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();

            _fakeTimelineQueryGenerator.CallsTo(x => x.GetUserTimelineQuery(_fakeUserTimelineRequestParameters)).Returns(_expectedQuery);

            // Act
            var result = controller.GetUserTimeline(_fakeUser, _maximuNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _expectedResult);
            VerifyUserTimelineRequestParameters();
        }

        [TestMethod]
        public void GetUserTimelineWithUser_FromUserDTO_ReturnsResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();

            _fakeTimelineQueryGenerator.CallsTo(x => x.GetUserTimelineQuery(_fakeUserTimelineRequestParameters)).Returns(_expectedQuery);

            // Act
            var result = controller.GetUserTimeline(_fakeUserDTO, _maximuNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _expectedResult);
            VerifyUserTimelineRequestParameters();
        }

        [TestMethod]
        public void GetUserTimelineWithUser_FromUserName_ReturnsResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();

            _fakeTimelineQueryGenerator.CallsTo(x => x.GetUserTimelineQuery(_fakeUserTimelineRequestParameters)).Returns(_expectedQuery);

            // Act
            var result = controller.GetUserTimeline(_userName, _maximuNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _expectedResult);
            VerifyUserTimelineRequestParameters();
        }

        [TestMethod]
        public void GetUserTimelineWithUser_FromUserId_ReturnsResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();

            _fakeTimelineQueryGenerator.CallsTo(x => x.GetUserTimelineQuery(_fakeUserTimelineRequestParameters)).Returns(_expectedQuery);

            // Act
            var result = controller.GetUserTimeline(_userId, _maximuNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _expectedResult);
            VerifyUserTimelineRequestParameters();
        }

        [TestMethod]
        public void GetUserTimeline_FromTimelineRequestParameter_ReturnsExpectedResult()
        {
            // Arrange
            var controller = CreateTimelineJsonController();

            _fakeTimelineQueryGenerator.CallsTo(x => x.GetUserTimelineQuery(_fakeUserTimelineRequestParameters)).Returns(_expectedQuery);

            // Act
            var result = controller.GetUserTimeline(_fakeUserTimelineRequestParameters);

            // Assert
            Assert.AreEqual(result, _expectedResult);
        }

        private void VerifyUserTimelineRequestParameters()
        {
            Assert.AreEqual(_fakeUserTimelineRequestParameters.UserIdentifier, _fakeUserIdentifier);
            Assert.AreEqual(_fakeUserTimelineRequestParameters.MaximumNumberOfTweetsToRetrieve, _maximuNumberOfTweets);
        }

        #endregion

        private string GetQueryResult(string query)
        {
            var result = TestHelper.GenerateString();
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, result);
            return result;
        }

        public TimelineJsonController CreateTimelineJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
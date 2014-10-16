using System.Collections.Generic;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Timeline;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Testinvi.TweetinviControllers.TimelineTests
{
    [TestClass]
    public class TimelineControllerTests
    {
        private FakeClassBuilder<TimelineController> _fakeBuilder;
        private Fake<ITweetFactory> _fakeTweetFactory;
        private Fake<IUserFactory> _fakeUserFactory;
        private Fake<ITimelineQueryExecutor> _fakeTimelineQueryExecutor;
        private Fake<IFactory<IUserTimelineRequestParameters>> _fakeUserTimelineRequestParameterFactory;
        private Fake<IFactory<IMentionsTimelineRequestParameters>> _fakeMentionsTimelineRequestParameterFactory;

        private IHomeTimelineRequestParameters _fakeHomeTimelineRequestParameters;
        private IUserTimelineRequestParameters _fakeUserTimelineRequestParameters;
        private IMentionsTimelineRequestParameters _fakeMentionsTimelineRequestParameters;

        private int _maximuNumberOfTweets;
        private IEnumerable<ITweetDTO> _resultDTO;
        private IEnumerable<ITweet> _result;
        private string _userName;
        private long _userId;

        private IUser _fakeUser;
        private IUserDTO _fakeUserDTO;
        private IUserIdentifier _fakeUserIdentifier;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TimelineController>();
            _fakeTweetFactory = _fakeBuilder.GetFake<ITweetFactory>();
            _fakeUserFactory = _fakeBuilder.GetFake<IUserFactory>();
            _fakeTimelineQueryExecutor = _fakeBuilder.GetFake<ITimelineQueryExecutor>();
            _fakeUserTimelineRequestParameterFactory = _fakeBuilder.GetFake<IFactory<IUserTimelineRequestParameters>>();
            _fakeMentionsTimelineRequestParameterFactory = _fakeBuilder.GetFake<IFactory<IMentionsTimelineRequestParameters>>();

            InitData();

            _fakeUserFactory.CallsTo(x => x.GetUserIdentifierFromUser(_fakeUser)).Returns(_fakeUserIdentifier);
            _fakeUserFactory.CallsTo(x => x.GenerateUserIdentifierFromScreenName(_userName)).Returns(_fakeUserIdentifier);
            _fakeUserFactory.CallsTo(x => x.GenerateUserIdentifierFromId(_userId)).Returns(_fakeUserIdentifier);
            _fakeUserTimelineRequestParameterFactory.CallsTo(x => x.Create()).Returns(_fakeUserTimelineRequestParameters);
            _fakeMentionsTimelineRequestParameterFactory.CallsTo(x => x.Create()).Returns(_fakeMentionsTimelineRequestParameters);
        }

        private void InitData()
        {
            _fakeHomeTimelineRequestParameters = A.Fake<IHomeTimelineRequestParameters>();
            _fakeUserTimelineRequestParameters = A.Fake<IUserTimelineRequestParameters>();
            _fakeMentionsTimelineRequestParameters = A.Fake<IMentionsTimelineRequestParameters>();

            _maximuNumberOfTweets = TestHelper.GenerateRandomInt();
            _resultDTO = new List<ITweetDTO>();
            _result = new List<ITweet>();
            _userName = TestHelper.GenerateString();
            _userId = TestHelper.GenerateRandomLong();

            _fakeUser = A.Fake<IUser>();
            _fakeUserDTO = A.Fake<IUserDTO>();
            _fakeUserIdentifier = _fakeUserDTO;
        }

        #region Get HomeTimeline

        [TestMethod]
        public void GetHomeTimeline_WithTimelineRequestParameter_ReturnsGeneratedObjectsFromQueryExecutorDTOs()
        {
            // Arrange
            var controller = CreateTimelineController();

            _fakeTimelineQueryExecutor.CallsTo(x => x.GetHomeTimeline(_fakeHomeTimelineRequestParameters)).Returns(_resultDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(_resultDTO)).Returns(_result);

            // Act
            var result = controller.GetHomeTimeline(_fakeHomeTimelineRequestParameters);

            // Assert
            Assert.AreEqual(result, _result);
        }

        #endregion

        #region Get UserTimeline

        [TestMethod]
        public void GetUserTimelineWithUser_FromUser_ReturnsResult()
        {
            // Arrange
            var controller = CreateTimelineController();

            _fakeTimelineQueryExecutor.CallsTo(x => x.GetUserTimeline(_fakeUserTimelineRequestParameters)).Returns(_resultDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(_resultDTO)).Returns(_result);

            // Act
            var result = controller.GetUserTimeline(_fakeUser, _maximuNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _result);
            VerifyUserTimelineRequestParameters();
        }

        [TestMethod]
        public void GetUserTimelineWithUser_FromUserDTO_ReturnsResult()
        {
            // Arrange
            var controller = CreateTimelineController();

            _fakeTimelineQueryExecutor.CallsTo(x => x.GetUserTimeline(_fakeUserTimelineRequestParameters)).Returns(_resultDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(_resultDTO)).Returns(_result);

            // Act
            var result = controller.GetUserTimeline(_fakeUserDTO, _maximuNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _result);
            VerifyUserTimelineRequestParameters();
        }

        [TestMethod]
        public void GetUserTimelineWithUser_FromUserName_ReturnsResult()
        {
            // Arrange
            var controller = CreateTimelineController();

            _fakeTimelineQueryExecutor.CallsTo(x => x.GetUserTimeline(_fakeUserTimelineRequestParameters)).Returns(_resultDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(_resultDTO)).Returns(_result);

            // Act
            var result = controller.GetUserTimeline(_userName, _maximuNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _result);
            VerifyUserTimelineRequestParameters();
        }

        [TestMethod]
        public void GetUserTimelineWithUser_FromUserId_ReturnsResult()
        {
            // Arrange
            var controller = CreateTimelineController();

            _fakeTimelineQueryExecutor.CallsTo(x => x.GetUserTimeline(_fakeUserTimelineRequestParameters)).Returns(_resultDTO);
            _fakeTweetFactory.CallsTo(x => x.GenerateTweetsFromDTO(_resultDTO)).Returns(_result);

            // Act
            var result = controller.GetUserTimeline(_userId, _maximuNumberOfTweets);

            // Assert
            Assert.AreEqual(result, _result);
            VerifyUserTimelineRequestParameters();
        }

        private void VerifyUserTimelineRequestParameters()
        {
            Assert.AreEqual(_fakeUserTimelineRequestParameters.UserIdentifier, _fakeUserIdentifier);
            Assert.AreEqual(_fakeUserTimelineRequestParameters.MaximumNumberOfTweetsToRetrieve, _maximuNumberOfTweets);
        }

        #endregion

        #region Get MentionsTimeline

        [TestMethod]
        public void VerifyGetMentionsTimeline_ExcludeReplies_ReturnsGeneratedObjectsFromQueryExecutorDTOs()
        {
            // Arrange
            var controller = CreateTimelineController();
            var nbTweetsLimit = TestHelper.GenerateRandomInt();
            var expectedDTOResult = new List<ITweetDTO>();
            var expectedResult = new List<IMention>();

            _fakeTimelineQueryExecutor.CallsTo(x => x.GetMentionsTimeline(_fakeMentionsTimelineRequestParameters)).Returns(expectedDTOResult);
            _fakeTweetFactory.CallsTo(x => x.GenerateMentionsFromDTO(expectedDTOResult)).Returns(expectedResult);

            // Act
            var result = controller.GetMentionsTimeline(nbTweetsLimit);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        public TimelineController CreateTimelineController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
using System;
using System.Text;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Controllers.Timeline;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Testinvi.TweetinviControllers.TimelineTests
{
    [TestClass]
    public class TimelineQueryGeneratorTests
    {
        private FakeClassBuilder<TimelineQueryGenerator> _fakeBuilder;
        private Fake<IUserQueryValidator> _fakeUserQueryValidator;
        private Fake<IUserQueryParameterGenerator> _fakeUserQueryParameterGenerator;
        private Fake<IQueryParameterGenerator> _fakeQueryParameterGenerator;
        private Fake<ITimelineQueryParameterGenerator> _fakeTimelineQueryParameterGenerator;

        private IHomeTimelineRequestParameters _fakeHomeTimelineRequestParameters;
        private IUserTimelineRequestParameters _fakeUserTimelineRequestParameters;
        private IMentionsTimelineRequestParameters _fakeMentionsTimelineRequestParameters;

        private int _maximumNumberOfTweetsParameterValue;

        private IUserIdentifier _fakeUserIdentifier;
        private string _userIdentifierParameter;
        private string _includeRTSParameter;
        private string _excludeRepliesParameter;
        private string _includeContributorDetailsParameter;
        private string _maximumNumberOfTweetsParameter;
        private string _trimUserParameter;
        private string _sinceIdParameter;
        private string _maxIdParameter;
        private string _includeDetailsParameter;

        private string _expectedTimelineQuery;
        private string _expectedUserTimelineQuery;
        private string _expectedMentionsTimelineQuery;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TimelineQueryGenerator>();
            _fakeUserQueryParameterGenerator = _fakeBuilder.GetFake<IUserQueryParameterGenerator>();
            _fakeQueryParameterGenerator = _fakeBuilder.GetFake<IQueryParameterGenerator>();
            _fakeUserQueryValidator = _fakeBuilder.GetFake<IUserQueryValidator>();
            
            _fakeTimelineQueryParameterGenerator = _fakeBuilder.GetFake<ITimelineQueryParameterGenerator>();

            Init();

            _fakeTimelineQueryParameterGenerator.CallsTo(x => x.GenerateExcludeRepliesParameter(It.IsAny<bool>())).Returns(_excludeRepliesParameter);
            _fakeTimelineQueryParameterGenerator.CallsTo(x => x.GenerateIncludeContributorDetailsParameter(It.IsAny<bool>())).Returns(_includeContributorDetailsParameter);

            _fakeUserQueryParameterGenerator.CallsTo(x => x.GenerateIdOrScreenNameParameter(_fakeUserIdentifier, "user_id", "screen_name")).Returns(_userIdentifierParameter);
            _fakeTimelineQueryParameterGenerator.CallsTo(x => x.GenerateIncludeRTSParameter(It.IsAny<bool>())).Returns(_includeRTSParameter);
            
            _fakeQueryParameterGenerator.CallsTo(x => x.GenerateCountParameter(_maximumNumberOfTweetsParameterValue)).Returns(_maximumNumberOfTweetsParameter);
            _fakeQueryParameterGenerator.CallsTo(x => x.GenerateTrimUserParameter(It.IsAny<bool>())).Returns(_trimUserParameter);
            _fakeQueryParameterGenerator.CallsTo(x => x.GenerateSinceIdParameter(It.IsAny<long>())).Returns(_sinceIdParameter);
            _fakeQueryParameterGenerator.CallsTo(x => x.GenerateMaxIdParameter(It.IsAny<long>())).Returns(_maxIdParameter);
            _fakeQueryParameterGenerator.CallsTo(x => x.GenerateIncludeEntitiesParameter(It.IsAny<bool>())).Returns(_includeDetailsParameter);
        }

        private void Init()
        {
            _maximumNumberOfTweetsParameterValue = TestHelper.GenerateRandomInt();
            _fakeUserIdentifier = A.Fake<IUserIdentifier>();

            _fakeHomeTimelineRequestParameters = A.Fake<IHomeTimelineRequestParameters>();
            _fakeHomeTimelineRequestParameters.CallsTo(x => x.MaximumNumberOfTweetsToRetrieve).Returns(_maximumNumberOfTweetsParameterValue);

            _fakeUserTimelineRequestParameters = A.Fake<IUserTimelineRequestParameters>();
            _fakeUserTimelineRequestParameters.CallsTo(x => x.MaximumNumberOfTweetsToRetrieve).Returns(_maximumNumberOfTweetsParameterValue);
            _fakeUserTimelineRequestParameters.CallsTo(x => x.UserIdentifier).Returns(_fakeUserIdentifier);

            _fakeMentionsTimelineRequestParameters = A.Fake<IMentionsTimelineRequestParameters>();
            _fakeMentionsTimelineRequestParameters.CallsTo(x => x.MaximumNumberOfTweetsToRetrieve).Returns(_maximumNumberOfTweetsParameterValue);

            _userIdentifierParameter = TestHelper.GenerateString();
            _includeRTSParameter = TestHelper.GenerateString();
            _excludeRepliesParameter = TestHelper.GenerateString();
            _includeContributorDetailsParameter = TestHelper.GenerateString();
            _maximumNumberOfTweetsParameter = TestHelper.GenerateString();
            _trimUserParameter = TestHelper.GenerateString();
            _sinceIdParameter = TestHelper.GenerateString();
            _maxIdParameter = TestHelper.GenerateString();
            _includeDetailsParameter = TestHelper.GenerateString();

            var queryParameterBuilder = new StringBuilder();
            
            queryParameterBuilder.Append(_includeContributorDetailsParameter);
            queryParameterBuilder.Append(_maximumNumberOfTweetsParameter);
            queryParameterBuilder.Append(_trimUserParameter);
            queryParameterBuilder.Append(_sinceIdParameter);
            queryParameterBuilder.Append(_maxIdParameter);
            queryParameterBuilder.Append(_includeDetailsParameter);

            var homeQueryParameter = _excludeRepliesParameter + queryParameterBuilder;
            var userQueryParameter = _userIdentifierParameter + _includeRTSParameter + _excludeRepliesParameter + queryParameterBuilder;

            _expectedTimelineQuery = String.Format(Resources.Timeline_GetHomeTimeline, homeQueryParameter);
            _expectedUserTimelineQuery = String.Format(Resources.Timeline_GetUserTimeline, userQueryParameter);
            _expectedMentionsTimelineQuery = String.Format(Resources.Timeline_GetMentionsTimeline, queryParameterBuilder);
        }

        #region GetHomeTimelineQuery

        [TestMethod]
        public void GetHomeTimelineQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTimelineQueryGenerator();

            // Act
            var result = queryGenerator.GetHomeTimelineQuery(_fakeHomeTimelineRequestParameters);

            // Assert
            Assert.AreEqual(result, _expectedTimelineQuery);
        }

        #endregion

        #region GetUserTimelineQuery

        [TestMethod]
        public void GetUserTimelineFromNullParameter_ThrowArgumentException()
        {
            // Arrange
            var queryGenerator = CreateTimelineQueryGenerator();

            // Act
            try
            {
                queryGenerator.GetUserTimelineQuery(null);
            }
            catch (ArgumentException)
            {
                return;
            }

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void GetUserTimelineFromInvalidUserIdentifier_ThrowArgumentException()
        {
            // Arrange
            var queryGenerator = CreateTimelineQueryGenerator();

            _fakeUserQueryValidator.ArrangeCanUserBeIdentified(_fakeUserIdentifier, false);

            // Act
            try
            {
                queryGenerator.GetUserTimelineQuery(_fakeUserTimelineRequestParameters);
            }
            catch (ArgumentException)
            {
                return;
            }

            // Assert
            Assert.Fail();
        }

        [TestMethod]
        public void GetUserTimelineFromExistingUserIdentifier_ReturnsQuery()
        {
            // Arrange
            var queryGenerator = CreateTimelineQueryGenerator();
            _fakeUserQueryValidator.ArrangeCanUserBeIdentified(_fakeUserIdentifier, true);

            // Act
            var result = queryGenerator.GetUserTimelineQuery(_fakeUserTimelineRequestParameters);

            // Assert
            Assert.AreEqual(result, _expectedUserTimelineQuery);
        }

        #endregion

        #region GetMentionsTimelineQuery

        [TestMethod]
        public void GetMentionsTimelineQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTimelineQueryGenerator();

            // Act
            var result = queryGenerator.GetMentionsTimelineQuery(_fakeMentionsTimelineRequestParameters);

            // Assert
            Assert.AreEqual(result, _expectedMentionsTimelineQuery);
        }

        #endregion

        public TimelineQueryGenerator CreateTimelineQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
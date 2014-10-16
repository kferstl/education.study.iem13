﻿using System.Collections.Generic;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Timeline;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Testinvi.TweetinviControllers.TimelineTests
{
    [TestClass]
    public class TimelineQueryExecutorTests
    {
        private FakeClassBuilder<TimelineQueryExecutor> _fakeBuilder;
        private Fake<ITimelineQueryGenerator> _fakeTimelineQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        private IHomeTimelineRequestParameters _fakeHomeTimelineRequestParameters;
        private IUserTimelineRequestParameters _fakeUserTimelineRequestParameters;
        private IMentionsTimelineRequestParameters _fakeMentionsTimelineRequestParameters;

        private string _expectedQuery;
        private IEnumerable<ITweetDTO> _expectedResult;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TimelineQueryExecutor>();
            _fakeTimelineQueryGenerator = _fakeBuilder.GetFake<ITimelineQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();

            InitData();

            // Do not initialize the query result for the different timeline from the ctor as it could false the result
            // Do not do : _fakeTimelineQueryGenerator.CallsTo(x => x.GetUserTimelineQuery(_fakeUserTimelineRequestParameters)).Returns(_expectedQuery);
        }

        private void InitData()
        {
            _expectedQuery = TestHelper.GenerateString();
            _expectedResult = GetQueryResult<IEnumerable<ITweetDTO>>(_expectedQuery);

            _fakeHomeTimelineRequestParameters = A.Fake<IHomeTimelineRequestParameters>();
            _fakeUserTimelineRequestParameters = A.Fake<IUserTimelineRequestParameters>();
            _fakeMentionsTimelineRequestParameters = A.Fake<IMentionsTimelineRequestParameters>();
        }

        #region GetHomeTimeline

        [TestMethod]
        public void GetHomeTimeline_FromHomeTimelineRequest_ReturnsExpectedResult()
        {
            // Arrange
            var queryExecutor = CreateTimelineQueryExecutor();

            _fakeTimelineQueryGenerator.CallsTo(x => x.GetHomeTimelineQuery(_fakeHomeTimelineRequestParameters)).Returns(_expectedQuery);

            // Act
            var result = queryExecutor.GetHomeTimeline(_fakeHomeTimelineRequestParameters);

            // Assert
            Assert.AreEqual(result, _expectedResult);
        }

        #endregion

        #region GetMentionsTimeline

        [TestMethod]
        public void GetMentionsTimeline_FromHomeTimelineRequest_ReturnsExpectedResult()
        {
            // Arrange
            var queryExecutor = CreateTimelineQueryExecutor();

            _fakeTimelineQueryGenerator.CallsTo(x => x.GetMentionsTimelineQuery(_fakeMentionsTimelineRequestParameters)).Returns(_expectedQuery);

            // Act
            var result = queryExecutor.GetMentionsTimeline(_fakeMentionsTimelineRequestParameters);

            // Assert
            Assert.AreEqual(result, _expectedResult);
        }

        #endregion

        #region GetUserTimeline

        [TestMethod]
        public void GetUserTimeline_FromUserTimelineRequest_ReturnsExpectedResult()
        {
            // Arrange
            var queryExecutor = CreateTimelineQueryExecutor();

            _fakeTimelineQueryGenerator.CallsTo(x => x.GetUserTimelineQuery(_fakeUserTimelineRequestParameters)).Returns(_expectedQuery);

            // Act
            var result = queryExecutor.GetUserTimeline(_fakeUserTimelineRequestParameters);

            // Assert
            Assert.AreEqual(result, _expectedResult);
        }

        #endregion

        private T GetQueryResult<T>(string query) where T : class
        {
            var result = A.Fake<T>();
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(query, result);
            return result;
        }

        public TimelineQueryExecutor CreateTimelineQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
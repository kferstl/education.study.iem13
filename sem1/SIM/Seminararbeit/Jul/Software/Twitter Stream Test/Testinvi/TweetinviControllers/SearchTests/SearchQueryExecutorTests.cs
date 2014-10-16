using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Search;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models.Parameters;

namespace Testinvi.TweetinviControllers.SearchTests
{
    [TestClass]
    public class SearchQueryExecutorTests
    {
        private FakeClassBuilder<SearchQueryExecutor> _fakeBuilder;
        private Fake<ISearchQueryGenerator> _fakeSearchQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;
        private Fake<ISearchQueryHelper> _fakeSearchQueryHelper;
        private Fake<ITweetHelper> _fakeTweetHelper;

        private string _searchQuery;
        private string _httpQuery;
        private string _statusesJson;
        private ITweetDTO _originalTweetDTO;
        private ITweetDTO _retweetDTO;
        private JObject _jObject;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<SearchQueryExecutor>();
            _fakeSearchQueryGenerator = _fakeBuilder.GetFake<ISearchQueryGenerator>();
            _fakeSearchQueryHelper = _fakeBuilder.GetFake<ISearchQueryHelper>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
            _fakeTweetHelper = _fakeBuilder.GetFake<ITweetHelper>();

            _searchQuery = TestHelper.GenerateString();
            _httpQuery = TestHelper.GenerateString();
            _statusesJson = TestHelper.GenerateString();
            _originalTweetDTO = GenerateTweetDTO(true);
            _retweetDTO = GenerateTweetDTO(false);

            _jObject = new JObject();
            _jObject["statuses"] = _statusesJson;
        }

        #region Search Tweet
        
        [TestMethod]
        public void SearchTweet_BasedOnQuery_ReturnsTwitterAccessorStatuses()
        {
            // Arrange
            var queryExecutor = CreateSearchQueryExecutor();
            var tweetDTOs = new List<ITweetDTO> { A.Fake<ITweetDTO>() };

            _fakeSearchQueryGenerator.CallsTo(x => x.GetSearchTweetsQuery(_searchQuery)).Returns(_httpQuery);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(_httpQuery, _jObject);
            _fakeSearchQueryHelper.CallsTo(x => x.GetTweetsFromJsonObject(_jObject)).Returns(tweetDTOs);

            // Act
            var result = queryExecutor.SearchTweets(_searchQuery);

            // Assert
            Assert.AreEqual(result, tweetDTOs);
        }

        [TestMethod]
        public void SearchTweet_BasedOnSearchParameters_ReturnsTwitterAccessorStatuses()
        {
            // Arrange
            var queryExecutor = CreateSearchQueryExecutor();
            var searchQueryParameter = A.Fake<ITweetSearchParameters>();
            var tweetDTOs = new List<ITweetDTO> { A.Fake<ITweetDTO>() };

            _fakeSearchQueryGenerator.CallsTo(x => x.GetSearchTweetsQuery(searchQueryParameter)).Returns(_httpQuery);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(_httpQuery, _jObject);
            _fakeSearchQueryHelper.CallsTo(x => x.GetTweetsFromJsonObject(_jObject)).Returns(tweetDTOs);

            // Act
            var result = queryExecutor.SearchTweets(searchQueryParameter);

            // Assert
            Assert.AreEqual(result, tweetDTOs);
        }

        [TestMethod]
        public void SearchTweet_FilterOriginal_FilterTheResults()
        {
            // Arrange
            var queryExecutor = CreateSearchQueryExecutor();
            var searchParameter = A.Fake<ITweetSearchParameters>();
            searchParameter.CallsTo(x => x.SearchQuery).Returns(_searchQuery);
            searchParameter.CallsTo(x => x.TweetSearchFilter).Returns(TweetSearchFilter.OriginalTweetsOnly);

            List<ITweetDTO> matchingTweetDTOs = new List<ITweetDTO>
            {
                _originalTweetDTO,
                _retweetDTO
            };

            _fakeSearchQueryGenerator.CallsTo(x => x.GetSearchTweetsQuery(searchParameter)).Returns(_httpQuery);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(_httpQuery, _jObject);
            _fakeSearchQueryHelper.CallsTo(x => x.GetTweetsFromJsonObject(_jObject)).Returns(matchingTweetDTOs);

            // Act
            var result = queryExecutor.SearchTweets(searchParameter);

            // Assert
            Assert.IsTrue(result.Contains(_originalTweetDTO));
            Assert.IsFalse(result.Contains(_retweetDTO));
        }

        [TestMethod]
        public void SearchTweet_FilterRetweets_FilterTheResults()
        {
            // Arrange
            var queryExecutor = CreateSearchQueryExecutor();
            var searchParameter = A.Fake<ITweetSearchParameters>();
            searchParameter.CallsTo(x => x.SearchQuery).Returns(_searchQuery);
            searchParameter.CallsTo(x => x.TweetSearchFilter).Returns(TweetSearchFilter.RetweetsOnly);

            List<ITweetDTO> matchingTweetDTOs = new List<ITweetDTO>
            {
                _originalTweetDTO,
                _retweetDTO
            };

            _fakeSearchQueryGenerator.CallsTo(x => x.GetSearchTweetsQuery(searchParameter)).Returns(_httpQuery);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(_httpQuery, _jObject);
            _fakeSearchQueryHelper.CallsTo(x => x.GetTweetsFromJsonObject(_jObject)).Returns(matchingTweetDTOs);

            // Act
            var result = queryExecutor.SearchTweets(searchParameter);

            // Assert
            Assert.IsFalse(result.Contains(_originalTweetDTO));
            Assert.IsTrue(result.Contains(_retweetDTO));
        } 

        #endregion

        private ITweetDTO GenerateTweetDTO(bool isOriginalTweet)
        {
            var tweetDTO = A.Fake<ITweetDTO>();
            tweetDTO.CallsTo(x => x.RetweetedTweetDTO).Returns(isOriginalTweet ? null : A.Fake<ITweetDTO>());
            return tweetDTO;
        }

        public SearchQueryExecutor CreateSearchQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
﻿using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Geo;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Tweet;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Testinvi.TweetinviControllers.TweetTests
{
    [TestClass]
    public class TweetQueryGeneratorTests
    {
        private FakeClassBuilder<TweetQueryGenerator> _fakeBuilder;
        private Fake<IGeoQueryGenerator> _fakeGeoQueryGenerator;
        private Fake<ITweetQueryValidator> _fakeTweetQueryValidator;
        private Fake<ITwitterStringFormatter> _fakeTwitterStringFormatter;

        private string _expectedTweetParameter;
        private string _expectedPlaceIdParameter;
        private string _expectedCoordinatesParameter;
        
        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TweetQueryGenerator>();
            _fakeGeoQueryGenerator = _fakeBuilder.GetFake<IGeoQueryGenerator>();
            _fakeTweetQueryValidator = _fakeBuilder.GetFake<ITweetQueryValidator>();
            _fakeTwitterStringFormatter = _fakeBuilder.GetFake<ITwitterStringFormatter>();
        }

        #region GetPublishTweetQuery

        [TestMethod]
        public void GetPublishTweetQuery_TweetCannotBePublished_ExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweet = GenerateTweet(false, false, false);

            // Act
            var result = queryGenerator.GetPublishTweetQuery(tweet);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetPublishTweetQuery_TweetCanBePublished_ExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweet = GenerateTweet(true, false, false);

            // Act
            var result = queryGenerator.GetPublishTweetQuery(tweet);

            // Assert
            var expectedQuery = String.Format(Resources.Tweet_Publish, _expectedTweetParameter);
            Assert.AreEqual(result, expectedQuery);
        }

        [TestMethod]
        public void GetPublishTweetQuery_WithPlaceId_ExpectedQuery()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryGenerator();
            var tweet = GenerateTweet(true, true, false);

            // Act
            var result = queryExecutor.GetPublishTweetQuery(tweet);

            // Assert
            var expectedBaseQuery = String.Format(Resources.Tweet_Publish, _expectedTweetParameter);
            var expectedPlaceIdParameter = GeneratePlaceIdParameter();

            Assert.AreEqual(result, expectedBaseQuery + expectedPlaceIdParameter);
        }

        [TestMethod]
        public void GetPublishTweetQuery_WithPlaceIdButInvalid_ReturnsNull()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryGenerator();
            var tweet = GenerateTweet(false, true, false);

            // Act
            var result = queryExecutor.GetPublishTweetQuery(tweet);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetPublishTweetQuery_WithCoordinates_ExpectedQuery()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryGenerator();
            var tweet = GenerateTweet(true, false, true);

            // Act
            var result = queryExecutor.GetPublishTweetQuery(tweet);

            // Assert
            var expectedBaseQuery = String.Format(Resources.Tweet_Publish, _expectedTweetParameter);
            var expectedCoordinatesParameter = GenerateCoordinatesParameter();

            Assert.AreEqual(result, expectedBaseQuery + expectedCoordinatesParameter);
        }

        [TestMethod]
        public void GetPublishTweetQuery_WithCoordinatesButInvalid_ReturnsNull()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryGenerator();
            var tweet = GenerateTweet(false, false, true);

            // Act
            var result = queryExecutor.GetPublishTweetQuery(tweet);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetPublishTweetQuery_WithPlaceIdAndCoordinates_ExpectedQuery()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryGenerator();
            var tweet = GenerateTweet(true, true, true);

            // Act
            var result = queryExecutor.GetPublishTweetQuery(tweet);

            // Assert
            var expectedBaseQuery = String.Format(Resources.Tweet_Publish, _expectedTweetParameter);
            var expectedPlaceIdParameter = GeneratePlaceIdParameter();
            var expectedCoordinatesParameter = GenerateCoordinatesParameter();

            Assert.IsTrue(result.StartsWith(expectedBaseQuery));
            Assert.IsTrue(result.Contains(expectedPlaceIdParameter));
            Assert.IsTrue(result.Contains(expectedCoordinatesParameter));
        }

        [TestMethod]
        public void GetPublishTweetQuery_WithPlaceIdAndCoordinatesButInvalid_ReturnsNull()
        {
            // Arrange
            var queryExecutor = CreateTweetQueryGenerator();
            var tweet = GenerateTweet(false, true, true);

            // Act
            var result = queryExecutor.GetPublishTweetQuery(tweet);

            // Assert
            Assert.AreEqual(result, null);
        }

        #endregion

        #region GetPublishTweetInReplyToQuery

        [TestMethod]
        public void GetPublishTweetInReplyToQuery_ReturnsExpectedResults()
        {
            VerifyPublishTweetInReplyToQuery(false, false, false, false, false);
            VerifyPublishTweetInReplyToQuery(false, false, false, true, false);
            VerifyPublishTweetInReplyToQuery(false, false, true, false, false);
            VerifyPublishTweetInReplyToQuery(false, false, true, true, false);

            VerifyPublishTweetInReplyToQuery(false, true, false, false, false);
            VerifyPublishTweetInReplyToQuery(false, true, false, true, false);
            VerifyPublishTweetInReplyToQuery(false, true, true, false, false);
            VerifyPublishTweetInReplyToQuery(false, true, true, true, false);

            VerifyPublishTweetInReplyToQuery(true, false, false, false, false);
            VerifyPublishTweetInReplyToQuery(true, false, false, true, false);
            VerifyPublishTweetInReplyToQuery(true, false, true, false, false);
            VerifyPublishTweetInReplyToQuery(true, false, true, true, false);

            VerifyPublishTweetInReplyToQuery(true, true, false, false, true);
            VerifyPublishTweetInReplyToQuery(true, true, false, true, true);
            VerifyPublishTweetInReplyToQuery(true, true, true, false, true);
            VerifyPublishTweetInReplyToQuery(true, true, true, true, true);
        }

        public void VerifyPublishTweetInReplyToQuery(
            bool canNewTweetBePublished,
            bool isTweetToReplyToPublished,
            bool tweetContainsPlaceId,
            bool tweetContainsCoordinates,
            bool isValid)
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var newTweet = GenerateTweet(canNewTweetBePublished, tweetContainsPlaceId, tweetContainsCoordinates);
            var tweetToReplyTo = GeneratePublishedTweet(isTweetToReplyToPublished);

            // Act
            var result = queryGenerator.GetPublishTweetInReplyToQuery(newTweet, tweetToReplyTo);

            // Assert
            if (isValid)
            {
                string expectedQuery = String.Format(Resources.Tweet_PublishInReplyTo, _expectedTweetParameter, tweetToReplyTo.Id);

                if (tweetContainsPlaceId)
                {
                    expectedQuery += GeneratePlaceIdParameter();
                }

                if (tweetContainsCoordinates)
                {
                    expectedQuery += GenerateCoordinatesParameter();
                }

                Assert.AreEqual(result, expectedQuery);
            }
            else
            {
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public void GetPublishTweetInReplyToIdQuery_ReturnsExpectedResults()
        {
            VerifyPublishTweetInReplyToIdQuery(false, false, false, false);
            VerifyPublishTweetInReplyToIdQuery(false, false, true, false);
            VerifyPublishTweetInReplyToIdQuery(false, true, false, false);
            VerifyPublishTweetInReplyToIdQuery(false, true, true, false);

            VerifyPublishTweetInReplyToIdQuery(true, false, false, true);
            VerifyPublishTweetInReplyToIdQuery(true, false, true, true);
            VerifyPublishTweetInReplyToIdQuery(true, true, false, true);
            VerifyPublishTweetInReplyToIdQuery(true, true, true, true);
        }

        public void VerifyPublishTweetInReplyToIdQuery(
            bool canNewTweetBePublished,
            bool tweetContainsPlaceId,
            bool tweetContainsCoordinates,
            bool isValid)
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var newTweet = GenerateTweet(canNewTweetBePublished, tweetContainsPlaceId, tweetContainsCoordinates);
            var tweetToReplyToId = TestHelper.GenerateRandomLong();

            // Act
            var result = queryGenerator.GetPublishTweetInReplyToQuery(newTweet, tweetToReplyToId);

            // Assert
            if (isValid)
            {
                string expectedQuery = String.Format(Resources.Tweet_PublishInReplyTo, _expectedTweetParameter, tweetToReplyToId);

                if (tweetContainsPlaceId)
                {
                    expectedQuery += GeneratePlaceIdParameter();
                }

                if (tweetContainsCoordinates)
                {
                    expectedQuery += GenerateCoordinatesParameter();
                }

                Assert.AreEqual(result, expectedQuery);
            }
            else
            {
                Assert.IsNull(result);
            }
        }

        #endregion

        #region GetPublishRetweetQuery

        [TestMethod]
        public void GetPublishRetweetQuery_RetweetingTweetUnpublished_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweet = A.Fake<ITweetDTO>();

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToRetweet)).Returns(false);

            // Act
            var result = queryGenerator.GetPublishRetweetQuery(tweetToRetweet);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetPublishRetweetQuery_RetweetingTweetPublished_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweetId = TestHelper.GenerateRandomLong();
            var tweetToRetweet = A.Fake<ITweetDTO>();
            tweetToRetweet.CallsTo(x => x.Id).Returns(tweetToRetweetId);

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToRetweet)).Returns(true);

            // Act
            var result = queryGenerator.GetPublishRetweetQuery(tweetToRetweet);

            // Assert
            var expectedResult = String.Format(Resources.Tweet_Retweet_Publish, tweetToRetweetId);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetPublishRetweetQuery_WithTweetId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweetId = TestHelper.GenerateRandomLong();

            // Act
            var result = queryGenerator.GetPublishRetweetQuery(tweetToRetweetId);

            // Assert
            var expectedResult = String.Format(Resources.Tweet_Retweet_Publish, tweetToRetweetId);
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region GetRetweetsQuery

        [TestMethod]
        public void GetRetweetsQuery_RetweetingTweetUnpublished_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweet = A.Fake<ITweetDTO>();

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToRetweet)).Returns(false);

            // Act
            var result = queryGenerator.GetRetweetsQuery(tweetToRetweet);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetRetweetsQuery_RetweetingTweetPublished_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweetId = TestHelper.GenerateRandomLong();
            var tweetToRetweet = A.Fake<ITweetDTO>();
            tweetToRetweet.CallsTo(x => x.Id).Returns(tweetToRetweetId);

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToRetweet)).Returns(true);

            // Act
            var result = queryGenerator.GetRetweetsQuery(tweetToRetweet);

            // Assert
            var expectedResult = String.Format(Resources.Tweet_Retweet_GetRetweets, tweetToRetweetId);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetRetweetsQuery_WithTweetId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweetId = TestHelper.GenerateRandomLong();

            // Act
            var result = queryGenerator.GetRetweetsQuery(tweetToRetweetId);

            // Assert
            var expectedResult = String.Format(Resources.Tweet_Retweet_GetRetweets, tweetToRetweetId);
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region GetDestroyTweetQuery

        [TestMethod]
        public void GetDestroyTweetQuery_TweetCannotBeDestroyed_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToDestroy = A.Fake<ITweetDTO>();

            _fakeTweetQueryValidator.CallsTo(x => x.CanTweetDTOBeDestroyed(tweetToDestroy)).Returns(false);

            // Act
            var result = queryGenerator.GetDestroyTweetQuery(tweetToDestroy);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetDestroyTweetQuery_TweetCanBeDestroyed_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToDestroyId = TestHelper.GenerateRandomLong();
            var tweetToDestroy = A.Fake<ITweetDTO>();
            tweetToDestroy.CallsTo(x => x.Id).Returns(tweetToDestroyId);

            _fakeTweetQueryValidator.CallsTo(x => x.CanTweetDTOBeDestroyed(tweetToDestroy)).Returns(true);

            // Act
            var result = queryGenerator.GetDestroyTweetQuery(tweetToDestroy);

            // Assert
            var expectedResult = String.Format(Resources.Tweet_Destroy, tweetToDestroyId);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetDestroyTweetQuery_WithTweetId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToDestroyId = TestHelper.GenerateRandomLong();

            // Act
            var result = queryGenerator.GetDestroyTweetQuery(tweetToDestroyId);

            // Assert
            var expectedResult = String.Format(Resources.Tweet_Destroy, tweetToDestroyId);
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region GetFavouriteTweetQuery

        [TestMethod]
        public void GetFavouriteTweetQuery_TweetNotPublished_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweet = A.Fake<ITweetDTO>();

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToRetweet)).Returns(false);

            // Act
            var result = queryGenerator.GetFavouriteTweetQuery(tweetToRetweet);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetFavouriteTweetQuery_TweetPublished_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToFavouriteId = TestHelper.GenerateRandomLong();
            var tweetToFavourite = A.Fake<ITweetDTO>();
            tweetToFavourite.CallsTo(x => x.Id).Returns(tweetToFavouriteId);

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToFavourite)).Returns(true);

            // Act
            var result = queryGenerator.GetFavouriteTweetQuery(tweetToFavourite);

            // Assert
            var expectedResult = String.Format(Resources.Tweet_Favorite_Create, tweetToFavouriteId);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetFavouriteTweetQuery_WithTweetId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToFavouriteId = TestHelper.GenerateRandomLong();

            // Act
            var result = queryGenerator.GetFavouriteTweetQuery(tweetToFavouriteId);

            // Assert
            var expectedResult = String.Format(Resources.Tweet_Favorite_Create, tweetToFavouriteId);
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region GetUnFavouriteTweetQuery

        [TestMethod]
        public void GetUnFavouriteTweetQuery_TweetNotPublished_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweet = A.Fake<ITweetDTO>();

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToRetweet)).Returns(false);

            // Act
            var result = queryGenerator.GetUnFavouriteTweetQuery(tweetToRetweet);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetUnFavouriteTweetQuery_TweetPublished_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToUnFavouriteId = TestHelper.GenerateRandomLong();
            var tweetToUnFavourite = A.Fake<ITweetDTO>();
            tweetToUnFavourite.CallsTo(x => x.Id).Returns(tweetToUnFavouriteId);

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToUnFavourite)).Returns(true);

            // Act
            var result = queryGenerator.GetUnFavouriteTweetQuery(tweetToUnFavourite);

            // Assert
            var expectedResult = String.Format(Resources.Tweet_Favorite_Destroy, tweetToUnFavouriteId);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetUnFavouriteTweetQuery_WithTweetId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToUnFavouriteId = TestHelper.GenerateRandomLong();

            // Act
            var result = queryGenerator.GetUnFavouriteTweetQuery(tweetToUnFavouriteId);

            // Assert
            var expectedResult = String.Format(Resources.Tweet_Favorite_Destroy, tweetToUnFavouriteId);
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        #region GetGenerateOEmbedTweetQuery

        [TestMethod]
        public void GetGenerateOEmbedTweetQuery_TweetNotPublished_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweet = A.Fake<ITweetDTO>();

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweetToRetweet)).Returns(false);

            // Act
            var result = queryGenerator.GetGenerateOEmbedTweetQuery(tweetToRetweet);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetGenerateOEmbedTweetQuery_TweetPublished_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetId = TestHelper.GenerateRandomLong();
            var tweet = A.Fake<ITweetDTO>();
            tweet.CallsTo(x => x.Id).Returns(tweetId);

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweet)).Returns(true);

            // Act
            var result = queryGenerator.GetGenerateOEmbedTweetQuery(tweet);

            // Assert
            var expectedResult = String.Format(Resources.Tweet_GenerateOEmbed, tweetId);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetGenerateOEmbedTweetQuery_WithTweetId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTweetQueryGenerator();
            var tweetToRetweetId = TestHelper.GenerateRandomLong();

            // Act
            var result = queryGenerator.GetGenerateOEmbedTweetQuery(tweetToRetweetId);

            // Assert
            var expectedResult = String.Format(Resources.Tweet_GenerateOEmbed, tweetToRetweetId);
            Assert.AreEqual(result, expectedResult);
        }

        #endregion

        private ITweetDTO GenerateTweet(
            bool canTweetBePublished, 
            bool hasPlaceIdParameter, 
            bool hasCoordinatesParameter)
        {
            var tweet = A.Fake<ITweetDTO>();
            var tweetId = TestHelper.GenerateRandomLong();
            var text = TestHelper.GenerateString();
            var placeId = TestHelper.GenerateString();
            var coordinates = A.Fake<ICoordinates>();

            tweet.CallsTo(x => x.Text).Returns(text);
            tweet.CallsTo(x => x.Id).Returns(tweetId);
            tweet.CallsTo(x => x.PlaceId).Returns(placeId);
            tweet.CallsTo(x => x.Coordinates).Returns(coordinates);

            _expectedTweetParameter = TestHelper.GenerateString();
            _fakeTwitterStringFormatter.CallsTo(x => x.TwitterEncode(text)).Returns(_expectedTweetParameter);
            _expectedPlaceIdParameter = hasPlaceIdParameter ? TestHelper.GenerateString() : null;
            _fakeGeoQueryGenerator.CallsTo(x => x.GeneratePlaceIdParameter(tweet.PlaceId)).Returns(_expectedPlaceIdParameter);
            _expectedCoordinatesParameter = hasCoordinatesParameter ? TestHelper.GenerateString() : null;
            _fakeGeoQueryGenerator.CallsTo(x => x.GenerateGeoParameter(coordinates)).Returns(_expectedCoordinatesParameter);
            _fakeTweetQueryValidator.CallsTo(x => x.CanTweetDTOBePublished(tweet)).Returns(canTweetBePublished);

            return tweet;
        }

        private ITweetDTO GeneratePublishedTweet(bool hasTweetBeenPublished = false)
        {
            var tweet = A.Fake<ITweetDTO>();
            var tweetId = TestHelper.GenerateRandomLong();
            
            tweet.CallsTo(x => x.Id).Returns(tweetId);

            _fakeTweetQueryValidator.CallsTo(x => x.IsTweetPublished(tweet)).Returns(hasTweetBeenPublished);

            return tweet;
        }

        private string GeneratePlaceIdParameter()
        {
            return String.Format("&{0}", _expectedPlaceIdParameter);
        }

        private string GenerateCoordinatesParameter()
        {
            return String.Format("&{0}", _expectedCoordinatesParameter);
        }

        public TweetQueryGenerator CreateTweetQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
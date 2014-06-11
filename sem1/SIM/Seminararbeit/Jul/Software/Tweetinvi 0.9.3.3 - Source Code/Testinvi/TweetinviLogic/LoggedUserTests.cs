﻿using System.Collections.Generic;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.oAuth;
using Tweetinvi.Logic;

namespace Testinvi.TweetinviLogic
{
    [TestClass]
    public class LoggedUserTests
    {
        private FakeClassBuilder<LoggedUser> _fakeBuilder;
        private Fake<ICredentialsAccessor> _fakeCredentialsAccessor;
        private Fake<ITimelineController> _fakeTimelineController;
        private Fake<IFriendshipController> _fakeFriendshipController;
        private Fake<ISavedSearchController> _fakeSavedSearchController;
        private Fake<IMessageController> _fakeMessageController;
        private Fake<ITweetFactory> _fakeTweetFactory;
        private Fake<ITweetController> _fakeTweetController;
        private Fake<IAccountController> _fakeAccountController;

        private ILoggedUser _loggedUser;
        private IOAuthCredentials _loggedUserCredentials;
        private IOAuthCredentials _currentCredentials;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<LoggedUser>();
            _fakeCredentialsAccessor = _fakeBuilder.GetFake<ICredentialsAccessor>();
            _fakeTimelineController = _fakeBuilder.GetFake<ITimelineController>();
            _fakeFriendshipController = _fakeBuilder.GetFake<IFriendshipController>();
            _fakeSavedSearchController = _fakeBuilder.GetFake<ISavedSearchController>();
            _fakeMessageController = _fakeBuilder.GetFake<IMessageController>();
            _fakeTweetFactory = _fakeBuilder.GetFake<ITweetFactory>();
            _fakeTweetController = _fakeBuilder.GetFake<ITweetController>();
            _fakeAccountController = _fakeBuilder.GetFake<IAccountController>();

            InitData();
        }
        
        #region GetHomeTimeline

        [TestMethod]
        public void GetHomeTimeline_CurrentCredentialsAreNotLoggedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var nbTweets = TestHelper.GenerateRandomInt();

            IOAuthCredentials startOperationWithCredentials = null;
            _fakeTimelineController.CallsTo(x => x.GetHomeTimeline(nbTweets)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _loggedUser.GetHomeTimeline(nbTweets);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _loggedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }
        
        #endregion

        #region GetMentionsTimeline

        [TestMethod]
        public void GetMentionsTimeline_CurrentCredentialsAreNotLoggedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var nbTweets = TestHelper.GenerateRandomInt();

            IOAuthCredentials startOperationWithCredentials = null;
            _fakeTimelineController.CallsTo(x => x.GetMentionsTimeline(nbTweets)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _loggedUser.GetMentionsTimeline(nbTweets);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _loggedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }
        
        #endregion

        #region GetRelationshipStatesWith

        [TestMethod]
        public void GetRelationshipStatesWith_CurrentCredentialsAreNotLoggedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var users = A.Fake<IEnumerable<IUser>>();

            IOAuthCredentials startOperationWithCredentials = null;
            _fakeFriendshipController.CallsTo(x => x.GetRelationshipStatesWith(users)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _loggedUser.GetRelationshipStatesWith(users);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _loggedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }
        
        #endregion

        #region GetRelationshipStatesAssociatedWith
        
        [TestMethod]
        public void GetRelationshipStatesAssociatedWith_CurrentCredentialsAreNotLoggedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var users = A.Fake<IEnumerable<IUser>>();

            IOAuthCredentials startOperationWithCredentials = null;
            _fakeFriendshipController.CallsTo(x => x.GetRelationshipStatesAssociatedWith(users)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _loggedUser.GetRelationshipStatesAssociatedWith(users);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _loggedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        } 

        #endregion

        #region GetUsersRequestingFriendship
        
        [TestMethod]
        public void GetUsersRequestingFriendship_CurrentCredentialsAreNotLoggedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            IOAuthCredentials startOperationWithCredentials = null;
            _fakeFriendshipController.CallsTo(x => x.GetUsersRequestingFriendship()).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _loggedUser.GetUsersRequestingFriendship();

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _loggedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        } 

        #endregion

        #region GetUsersYouRequestedToFollow
        
        [TestMethod]
        public void GetUsersYouRequestedToFollow_CurrentCredentialsAreNotLoggedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            IOAuthCredentials startOperationWithCredentials = null;
            _fakeFriendshipController.CallsTo(x => x.GetUsersYouRequestedToFollow()).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _loggedUser.GetUsersYouRequestedToFollow();

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _loggedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        } 

        #endregion

        #region FollowUser

        [TestMethod]
        public void FollowUser_CurrentCredentialsAreNotLoggedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var user = A.Fake<IUser>();

            IOAuthCredentials startOperationWithCredentials = null;
            _fakeFriendshipController.CallsTo(x => x.CreateFriendshipWith(user)).ReturnsLazily(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
                return true;
            });
          
            // Act
            _loggedUser.FollowUser(user);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _loggedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        } 

        #endregion

        #region FollowUser

        [TestMethod]
        public void UnFollowUser_CurrentCredentialsAreNotLoggedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var user = A.Fake<IUser>();

            IOAuthCredentials startOperationWithCredentials = null;
            _fakeFriendshipController.CallsTo(x => x.DestroyFriendshipWith(user)).ReturnsLazily(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
                return true;
            });

            // Act
            _loggedUser.UnFollowUser(user);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _loggedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region UpdateRelationshipAuthorizationsWith

        [TestMethod]
        public void UpdateRelationshipAuthorizationsWith_CurrentCredentialsAreNotLoggedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var user = A.Fake<IUser>();

            IOAuthCredentials startOperationWithCredentials = null;
            _fakeFriendshipController.CallsTo(x => x.UpdateRelationshipAuthorizationsWith(user, true, true)).ReturnsLazily(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
                return true;
            });

            // Act
            _loggedUser.UpdateRelationshipAuthorizationsWith(user, true, true);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _loggedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region GetSavedSearches

        [TestMethod]
        public void GetSavedSearches_CurrentCredentialsAreNotLoggedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            IOAuthCredentials startOperationWithCredentials = null;
            _fakeSavedSearchController.CallsTo(x => x.GetSavedSearches()).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _loggedUser.GetSavedSearches();

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _loggedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region GetLatestMessagesReceived

        [TestMethod]
        public void GetLatestMessagesReceived_CurrentCredentialsAreNotLoggedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var nbMessages = TestHelper.GenerateRandomInt();

            IOAuthCredentials startOperationWithCredentials = null;
            _fakeMessageController.CallsTo(x => x.GetLatestMessagesReceived(nbMessages)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _loggedUser.GetLatestMessagesReceived(nbMessages);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _loggedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region GetLatestMessagesSent

        [TestMethod]
        public void GetLatestMessagesSent_CurrentCredentialsAreNotLoggedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var nbMessages = TestHelper.GenerateRandomInt();

            IOAuthCredentials startOperationWithCredentials = null;
            _fakeMessageController.CallsTo(x => x.GetLatestMessagesSent(nbMessages)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _loggedUser.GetLatestMessagesSent(nbMessages);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _loggedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region PublishMessage

        [TestMethod]
        public void PublishMessage_CurrentCredentialsAreNotLoggedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var messageDTO = A.Fake<IMessageDTO>();
            var message = A.Fake<IMessage>();
            message.CallsTo(x => x.MessageDTO).Returns(messageDTO);

            IOAuthCredentials startOperationWithCredentials = null;
            _fakeMessageController.CallsTo(x => x.PublishMessage(messageDTO)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _loggedUser.PublishMessage(message);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _loggedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region PublishTweet

        [TestMethod]
        public void PublishTweetText_CurrentCredentialsAreNotLoggedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var tweetText = TestHelper.GenerateString();
            var tweet = A.Fake<ITweet>();

            IOAuthCredentials startOperationWithCredentials = null;
            _fakeTweetFactory.CallsTo(x => x.CreateTweet(tweetText)).Returns(tweet);
            _fakeTweetController.CallsTo(x => x.PublishTweet(tweet)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _loggedUser.PublishTweet(tweetText);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _loggedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        [TestMethod]
        public void PublishTweet_CurrentCredentialsAreNotLoggedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var tweet = A.Fake<ITweet>();

            IOAuthCredentials startOperationWithCredentials = null;
            _fakeTweetController.CallsTo(x => x.PublishTweet(tweet)).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _loggedUser.PublishTweet(tweet);

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _loggedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        #region GetAccountSettings

        [TestMethod]
        public void GetAccountSettings_CurrentCredentialsAreNotLoggedUserCredentials_OperationPerformedWithAppropriateCredentials()
        {
            // Arrange
            var messageDTO = A.Fake<IMessageDTO>();
            var message = A.Fake<IMessage>();
            message.CallsTo(x => x.MessageDTO).Returns(messageDTO);

            IOAuthCredentials startOperationWithCredentials = null;
            _fakeAccountController.CallsTo(x => x.GetLoggedUserSettings()).Invokes(() =>
            {
                startOperationWithCredentials = _fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials;
            });

            // Act
            _loggedUser.GetAccountSettings();

            // Assert
            Assert.AreEqual(startOperationWithCredentials, _loggedUserCredentials);
            Assert.AreEqual(_fakeCredentialsAccessor.FakedObject.CurrentThreadCredentials, _currentCredentials);
        }

        #endregion

        private void InitData()
        {
            _loggedUserCredentials = A.Fake<IOAuthCredentials>();
            _fakeCredentialsAccessor.CallsTo(x => x.CurrentThreadCredentials).Returns(_loggedUserCredentials);
            _loggedUser = CreateLoggedUser();
            
            _currentCredentials = A.Fake<IOAuthCredentials>();
            _fakeCredentialsAccessor.CallsTo(x => x.CurrentThreadCredentials).Returns(_currentCredentials);
        }

        public LoggedUser CreateLoggedUser()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
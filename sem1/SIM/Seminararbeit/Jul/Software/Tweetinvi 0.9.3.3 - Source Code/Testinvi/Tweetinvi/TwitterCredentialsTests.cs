﻿using System.Configuration;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi;
using Tweetinvi.Core.Interfaces.oAuth;

namespace Testinvi.Tweetinvi
{
    [TestClass]
    public class TwitterCredentialsTests
    {
        [TestMethod]
        public void ThreadCredentialsSetCorrectly()
        {
            // Arrange
            var credentials1 = TwitterCredentials.CreateCredentials(
                ConfigurationManager.AppSettings["token_AccessToken"],
                ConfigurationManager.AppSettings["token_AccessTokenSecret"],
                ConfigurationManager.AppSettings["token_ConsumerKey"],
                ConfigurationManager.AppSettings["token_ConsumerSecret"]);

            var credentials2 = TwitterCredentials.CreateCredentials(
                ConfigurationManager.AppSettings["token2_AccessToken"],
                ConfigurationManager.AppSettings["token2_AccessTokenSecret"],
                ConfigurationManager.AppSettings["token2_ConsumerKey"],
                ConfigurationManager.AppSettings["token2_ConsumerSecret"]);

            bool credentials2Set = false;
            bool thread1Initialized = false;

            // Act
            TwitterCredentials.Credentials = credentials1;
            AssertAreCredentialsEquals(TwitterCredentials.Credentials, credentials1);

            var thread = new Thread(() =>
            {
                AssertAreCredentialsEquals(TwitterCredentials.Credentials, credentials1);
                thread1Initialized = true;

                // ReSharper disable once AccessToModifiedClosure
                while (!credentials2Set)
                {
                    Thread.Sleep(5);
                }

                AssertAreCredentialsEquals(TwitterCredentials.Credentials, credentials1);
                TwitterCredentials.Credentials = credentials2;
                AssertAreCredentialsEquals(TwitterCredentials.Credentials, credentials2);
            });

            thread.Start();

            while (!thread1Initialized)
            {
                Thread.Sleep(5);
            }

            TwitterCredentials.Credentials = credentials2;
            AssertAreCredentialsEquals(TwitterCredentials.Credentials, credentials2);

            Thread t2 = new Thread(() =>
            {
                AssertAreCredentialsEquals(TwitterCredentials.Credentials, credentials2);
                TwitterCredentials.Credentials = credentials1;
                AssertAreCredentialsEquals(TwitterCredentials.Credentials, credentials1);
            });

            t2.Start();
            t2.Join();

            credentials2Set = true;

            thread.Join();

            AssertAreCredentialsEquals(TwitterCredentials.Credentials, credentials1);
        }

        [TestMethod]
        public void ApplicationCredentialsDefinedOnFirstSet()
        {
            // Arrange
            var credentials = GenerateTokenCredentials();

            // Act
            TwitterCredentials.Credentials = credentials;

            // Assert
            Assert.AreEqual(credentials, TwitterCredentials.Credentials);
        }

        private IOAuthCredentials GenerateTokenCredentials()
        {
            return null;
        }

        private void AssertAreCredentialsEquals(IOAuthCredentials credentials1, IOAuthCredentials credentials2)
        {
            Assert.AreEqual(credentials1.AccessToken, credentials2.AccessToken);
            Assert.AreEqual(credentials1.AccessTokenSecret, credentials2.AccessTokenSecret);
            Assert.AreEqual(credentials1.ConsumerKey, credentials2.ConsumerKey);
            Assert.AreEqual(credentials1.ConsumerSecret, credentials2.ConsumerSecret);
        }
    }
}
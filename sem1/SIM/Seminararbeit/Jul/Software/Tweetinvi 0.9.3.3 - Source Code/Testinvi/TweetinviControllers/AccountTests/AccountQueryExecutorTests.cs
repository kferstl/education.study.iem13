﻿using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Account;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;

namespace Testinvi.TweetinviControllers.AccountTests
{
    [TestClass]
    public class AccountQueryExecutorTests
    {
        private FakeClassBuilder<AccountQueryExecutor> _fakeBuilder;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;
        private Fake<IAccountQueryGenerator> _fakeAccountQueryGenerator;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<AccountQueryExecutor>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
            _fakeAccountQueryGenerator = _fakeBuilder.GetFake<IAccountQueryGenerator>();
        }

        [TestMethod]
        public void GetLoggedUserSettings_ReturnsAccessorJsonResult()
        {
            string query = Guid.NewGuid().ToString();
            var queryResult = A.Fake<IAccountSettingsDTO>();

            // Arrange
            var controller = CreateAccountQueryExecutor();

            ArrangeGetLoggedUserAccountSettingsQuery(query);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(query, queryResult);

            // Act
            var result = controller.GetLoggedUserAccountSettings();

            // Assert
            Assert.AreEqual(result, queryResult);
        }

        private void ArrangeGetLoggedUserAccountSettingsQuery(string query)
        {
            _fakeAccountQueryGenerator
                .CallsTo(x => x.GetLoggedUserAccountSettingsQuery())
                .Returns(query);
        }

        public AccountQueryExecutor CreateAccountQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
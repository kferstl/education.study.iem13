﻿using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Account;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Testinvi.TweetinviControllers.AccountTests
{
    [TestClass]
    public class AccountJsonControllerTests
    {
        private FakeClassBuilder<AccountJsonController> _fakeBuilder;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;
        private Fake<IAccountQueryGenerator> _fakeAccountQueryGenerator;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<AccountJsonController>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
            _fakeAccountQueryGenerator = _fakeBuilder.GetFake<IAccountQueryGenerator>();
        }

        [TestMethod]
        public void GetLoggedUserSettingsJson_ReturnsAccessorJsonResult()
        {
            string query = Guid.NewGuid().ToString();
            string jsonResult = Guid.NewGuid().ToString();

            // Arrange
            var controller = CreateAccountJsonController();

            ArrangeGetLoggedUserAccountSettingsQuery(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, jsonResult);

            // Act
            var result = controller.GetLoggedUserSettingsJson();

            // Assert
            Assert.AreEqual(result, jsonResult);
        }

        private void ArrangeGetLoggedUserAccountSettingsQuery(string query)
        {
            _fakeAccountQueryGenerator
                .CallsTo(x => x.GetLoggedUserAccountSettingsQuery())
                .Returns(query);
        }

        public AccountJsonController CreateAccountJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}
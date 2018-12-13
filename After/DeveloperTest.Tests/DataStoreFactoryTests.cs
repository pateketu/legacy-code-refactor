using System;
using System.Collections.Generic;
using System.Linq;
using DeveloperTest.Config;
using DeveloperTest.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace DeveloperTest.Tests
{
    [TestClass]
    public class DataStoreFactoryTests
    {
        private IDataStoreFactory _target;

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Should_throwAnExceptionWhenNoDataStoreConfigIsSet()
        {
            _target = new DataStoreFactory(new ConfigProvider(), Enumerable.Empty<IDataStore>());

            _target.Get();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Should_throwAnExceptionWhenNoDataStoreIsAvailable()
        {
            var stubConfigProvider = MockRepository.GenerateStub<IConfigProvider>();
            stubConfigProvider.Stub(x => x.GetAppSettings(Arg<string>.Is.Anything)).Return("UnknowDataStore");
            _target = new DataStoreFactory(stubConfigProvider, Enumerable.Empty<IDataStore>());

            _target.Get();

        }

        [TestMethod]
        public void Should_ReturnExpectedDataStore()
        {
            var stubConfigProvider = MockRepository.GenerateStub<IConfigProvider>();
            stubConfigProvider.Stub(x => x.GetAppSettings(Arg<string>.Is.Anything)).Return("Account");

            _target = new DataStoreFactory(stubConfigProvider, new List<IDataStore>(){ new AccountDataStore(), new BackupAccountDataStore() });

            var dataStore = _target.Get();
            Assert.IsInstanceOfType(dataStore, typeof(AccountDataStore));

        }
    }
}

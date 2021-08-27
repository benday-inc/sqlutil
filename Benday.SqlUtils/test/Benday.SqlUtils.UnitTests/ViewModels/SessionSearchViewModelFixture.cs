using Benday.SqlUtils.Presentation.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Benday.SqlUtils.UnitTests.ViewModels
{
    [TestClass]
    public class SessionSearchViewModelFixture : ViewModelFixtureBase
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
            _DatabaseConnectionStringRepositoryInstance = null;
        }

        private SessionSearchViewModel _SystemUnderTest;
        public SessionSearchViewModel SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new SessionSearchViewModel(
                        MessageManagerInstance,
                        DatabaseConnectionStringRepositoryInstance,
                        new MockTelemetryService());
                }

                return _SystemUnderTest;
            }
        }

        private MockMessageManager _messageManagerInstance;
        public MockMessageManager MessageManagerInstance
        {
            get
            {
                if (_messageManagerInstance == null)
                {
                    _messageManagerInstance = new MockMessageManager();
                }

                return _messageManagerInstance;
            }
        }

        private MockDatabaseConnectionStringRepository _DatabaseConnectionStringRepositoryInstance;
        public MockDatabaseConnectionStringRepository DatabaseConnectionStringRepositoryInstance
        {
            get
            {
                if (_DatabaseConnectionStringRepositoryInstance == null)
                {
                    _DatabaseConnectionStringRepositoryInstance =
                        new MockDatabaseConnectionStringRepository();

                    _DatabaseConnectionStringRepositoryInstance.Add(
                        Guid.NewGuid().ToString(), "connection 1",
                        _ValidConnectionStringUseIntegratedSecurity);

                    _DatabaseConnectionStringRepositoryInstance.Add(
                        Guid.NewGuid().ToString(), "connection 2",
                        _ValidConnectionStringUserNamePassword);
                }

                return _DatabaseConnectionStringRepositoryInstance;
            }
        }

        [TestMethod]
        public void WhenInitializedThenDatabaseConnectionsArePopulated()
        {
            Assert.IsNotNull(SystemUnderTest.DatabaseConnections, "DatabaseConnections was null.");
            Assert.AreNotEqual<int>(0, SystemUnderTest.DatabaseConnections.Count,
                "Wrong number of items.");
        }

        [TestMethod]
        public void WhenInitializedThenFieldPropertiesAreNotNull()
        {
            Assert.IsNotNull(SystemUnderTest.SearchStatus, "SearchStatus is null.");
            Assert.IsNotNull(SystemUnderTest.SearchLogin, "SearchLogin is null.");
            Assert.IsNotNull(SystemUnderTest.SearchHostname, "SearchHostname was null.");
            Assert.IsNotNull(SystemUnderTest.SearchBlockedBy, "SearchBlockedBy was null.");
            Assert.IsNotNull(SystemUnderTest.SearchDatabaseName, "SearchDatabaseName was null.");
            Assert.IsNotNull(SystemUnderTest.SearchCommand, "SearchCommand was null.");
        }

        [TestMethod]
        public void WhenInitializedThenFieldPropertiesAreValid()
        {
            Assert.IsTrue(SystemUnderTest.SearchStatus.IsValid, "SearchStatus should be valid.");
            Assert.IsTrue(SystemUnderTest.SearchLogin.IsValid, "SearchLogin should be valid.");
            Assert.IsTrue(SystemUnderTest.SearchHostname.IsValid, "SearchHostname should be valid.");
            Assert.IsTrue(SystemUnderTest.SearchBlockedBy.IsValid, "SearchBlockedBy should be valid.");
            Assert.IsTrue(SystemUnderTest.SearchDatabaseName.IsValid, "SearchDatabaseName should be valid.");
        }
    }
}

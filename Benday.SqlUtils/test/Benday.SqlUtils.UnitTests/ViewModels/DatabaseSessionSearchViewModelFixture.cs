using Benday.SqlUtils.Api;
using Benday.SqlUtils.Presentation.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Benday.SqlUtils.UnitTests.ViewModels
{
    [TestClass]
    public class DatabaseSessionSearchViewModelFixture : ViewModelFixtureBase
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
            _DatabaseConnectionStringRepositoryInstance = null;
            _runner = null;
        }

        private DatabaseSessionSearchViewModel _SystemUnderTest;
        public DatabaseSessionSearchViewModel SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new DatabaseSessionSearchViewModel(
                        MessageManagerInstance,
                        Runner,
                        DatabaseConnectionStringRepositoryInstance,
                        new MockTelemetryService());
                }

                return _SystemUnderTest;
            }
        }

        private MockQueryRunner _runner;
        public MockQueryRunner Runner
        {
            get
            {
                if (_runner == null)
                {
                    _runner = new MockQueryRunner();
                }

                return _runner;
            }
        }

        private void InitalizeAllFieldsToNull()
        {
            var table = DatabaseSessionTestUtility.InitalizeAllFieldsToNull();

            Runner.RunReturnValue = table.DataSet;
        }

        private void InitalizeAllStringFieldsToWhitespace()
        {
            var table = DatabaseSessionTestUtility.InitalizeAllStringFieldsToWhitespace();

            Runner.RunReturnValue = table.DataSet;
        }

        private void InitalizeAllStringFieldsToDash()
        {
            var table = DatabaseSessionTestUtility.InitalizeAllStringFieldsToDash();

            Runner.RunReturnValue = table.DataSet;
        }

        private void InitalizeAllFieldsToValues()
        {
            var table = DatabaseSessionTestUtility.InitalizeAllFieldsToValues();

            var row = table.Rows[0];

            Runner.RunReturnValue = table.DataSet;
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
        public void WhenInitializedThenResultIsNull()
        {
            Assert.IsNull(SystemUnderTest.Result, "Result is null.");
        }

        [TestMethod]
        public void WhenInitializedThenSearchCommandIsNotNull()
        {
            Assert.IsNotNull(SystemUnderTest.SearchCommand, "SearchCommand is null.");
        }

        [TestMethod]
        public void WhenInitializedThenResetFiltersCommandIsNotNull()
        {
            Assert.IsNotNull(SystemUnderTest.ResetFiltersCommand, "ResetFiltersCommand is null.");
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

        private void AssertSearchFieldArgName(ISearchFilterable filter, string expected)
        {
            var actual = filter.ArgName;

            Assert.AreEqual<string>(expected, actual, "ArgName value was wrong.");
        }

        [TestMethod]
        public void WhenInitializedThenFieldPropertiesArgNamesAreSet()
        {
            AssertSearchFieldArgName(
                SystemUnderTest.SearchStatus, nameof(DatabaseSessionQueryResultRow.Status));
            AssertSearchFieldArgName(
                SystemUnderTest.SearchLogin, nameof(DatabaseSessionQueryResultRow.Login));
            AssertSearchFieldArgName(
                SystemUnderTest.SearchHostname, nameof(DatabaseSessionQueryResultRow.HostName));
            AssertSearchFieldArgName(
                SystemUnderTest.SearchBlockedBy, nameof(DatabaseSessionQueryResultRow.BlockedBy));
            AssertSearchFieldArgName(
                SystemUnderTest.SearchDatabaseName, nameof(DatabaseSessionQueryResultRow.DatabaseName));
            AssertSearchFieldArgName(
                SystemUnderTest.SearchCommandText, nameof(DatabaseSessionQueryResultRow.Command));
        }

        [TestMethod]
        public void WhenInitializedThenFieldPropertiesAreValid()
        {
            Assert.IsTrue(SystemUnderTest.SearchStatus.IsValid, "SearchStatus should be valid.");
            Assert.IsTrue(SystemUnderTest.SearchLogin.IsValid, "SearchLogin should be valid.");
            Assert.IsTrue(SystemUnderTest.SearchHostname.IsValid, "SearchHostname should be valid.");
            Assert.IsTrue(SystemUnderTest.SearchBlockedBy.IsValid, "SearchBlockedBy should be valid.");
            Assert.IsTrue(SystemUnderTest.SearchDatabaseName.IsValid, "SearchDatabaseName should be valid.");
            Assert.IsTrue(SystemUnderTest.SearchCommandText.IsValid, "SearchCommandText should be valid.");
        }

        [TestMethod]
        public void WhenInitializedThenFieldPropertiesAreEmptyString()
        {
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.SearchStatus.Value, "SearchStatus should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.SearchLogin.Value, "SearchLogin should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.SearchHostname.Value, "SearchHostname should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.SearchBlockedBy.Value, "SearchBlockedBy should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.SearchDatabaseName.Value, "SearchDatabaseName should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.SearchCommandText.Value, "SearchCommandText should be empty.");
        }

        [TestMethod]
        public void WhenResetFiltersIsCalledThenFieldPropertiesAreEmptyString()
        {
            SystemUnderTest.SearchStatus.Value = "asdf";
            SystemUnderTest.SearchLogin.Value = "asdf";
            SystemUnderTest.SearchHostname.Value = "asdf";
            SystemUnderTest.SearchBlockedBy.Value = "asdf";
            SystemUnderTest.SearchDatabaseName.Value = "asdf";
            SystemUnderTest.SearchCommandText.Value = "asdf";

            SystemUnderTest.ResetFiltersCommand.Execute(null);

            Assert.AreEqual<string>(String.Empty, SystemUnderTest.SearchStatus.Value, "SearchStatus should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.SearchLogin.Value, "SearchLogin should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.SearchHostname.Value, "SearchHostname should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.SearchBlockedBy.Value, "SearchBlockedBy should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.SearchDatabaseName.Value, "SearchDatabaseName should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.SearchCommandText.Value, "SearchCommandText should be empty.");
        }

        [TestMethod]
        public void Status_NotBlanks_FindsStringValues()
        {
            // arrange
            InitalizeAllFieldsToValues();
            SystemUnderTest.SearchStatus.SelectSearchType(Constants.SearchTypeNotBlankOrEmpty);

            // act
            SystemUnderTest.SearchCommand.Execute(null);

            // assert
            Assert.AreEqual<int>(1, SystemUnderTest.Result.Results.Count, "Result count was wrong.");
        }

        [TestMethod]
        public void Status_NotBlanks_ExcludesStringValues()
        {
            // arrange
            InitalizeAllFieldsToNull();
            SystemUnderTest.SearchStatus.SelectSearchType(Constants.SearchTypeNotBlankOrEmpty);

            // act
            SystemUnderTest.SearchCommand.Execute(null);

            // assert
            Assert.AreEqual<int>(0, SystemUnderTest.Result.Results.Count, "Result count was wrong.");
        }

        [TestMethod]
        public void Status_Blanks_FindsBlankValues()
        {
            // arrange
            InitalizeAllStringFieldsToDash();
            SystemUnderTest.SearchStatus.SelectSearchType(Constants.SearchTypeBlankOrEmpty);

            // act
            SystemUnderTest.SearchCommand.Execute(null);

            // assert
            Assert.AreEqual<int>(1, SystemUnderTest.Result.Results.Count, "Result count was wrong.");
        }

        [TestMethod]
        public void Status_Blanks_ExcludesStringValues()
        {
            // arrange
            InitalizeAllFieldsToValues();
            SystemUnderTest.SearchStatus.SelectSearchType(Constants.SearchTypeBlankOrEmpty);

            // act
            SystemUnderTest.SearchCommand.Execute(null);

            // assert
            Assert.AreEqual<int>(0, SystemUnderTest.Result.Results.Count, "Result count was wrong.");
        }

        [TestMethod]
        public void Status_ByValue_FindsMatchingValues()
        {
            // arrange
            InitalizeAllFieldsToValues();
            SystemUnderTest.SearchStatus.SelectSearchType(Constants.SearchTypeByValue);
            SystemUnderTest.SearchStatus.Value = "tatus";

            // act
            SystemUnderTest.SearchCommand.Execute(null);

            // assert
            Assert.AreEqual<int>(1, SystemUnderTest.Result.Results.Count, "Result count was wrong.");
        }

        [TestMethod]
        public void Status_ByValue_ExcludesNonMatchingStringValues()
        {
            // arrange
            InitalizeAllFieldsToValues();
            SystemUnderTest.SearchStatus.SelectSearchType(Constants.SearchTypeByValue);
            SystemUnderTest.SearchStatus.Value = "bogus";

            // act
            SystemUnderTest.SearchCommand.Execute(null);

            // assert
            Assert.AreEqual<int>(0, SystemUnderTest.Result.Results.Count, "Result count was wrong.");
        }
    }
}

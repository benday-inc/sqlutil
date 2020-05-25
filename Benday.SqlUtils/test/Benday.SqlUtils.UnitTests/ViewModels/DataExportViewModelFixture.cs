using Benday.SqlUtils.Api;
using Benday.SqlUtils.Api.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benday.SqlUtils.UnitTests.ViewModels
{
    [TestClass]
    public class DataExportViewModelFixture : ViewModelFixtureBase
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
            _DatabaseConnectionStringRepositoryInstance = null;
            _DatabaseUtilityInstance = null;
        }

        private DataExportViewModel _SystemUnderTest;
        public DataExportViewModel SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new DataExportViewModel(
                        DatabaseConnectionStringRepositoryInstance,
                        DatabaseUtilityInstance
                        );
                }

                return _SystemUnderTest;
            }
        }

        private MockDatabaseUtility _DatabaseUtilityInstance;
        public MockDatabaseUtility DatabaseUtilityInstance
        {
            get
            {
                if (_DatabaseUtilityInstance == null)
                {
                    _DatabaseUtilityInstance = new MockDatabaseUtility();
                }

                return _DatabaseUtilityInstance;
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
        public void WhenInitializedThenFieldPropertiesAreNotNull()
        {
            Assert.IsNotNull(SystemUnderTest.Query, "Query is null.");
            Assert.IsNotNull(SystemUnderTest.GeneratedQuery, "GeneratedQuery is null.");
            Assert.IsNotNull(SystemUnderTest.GenerateIdentityInsert, "GenerateIdentityInsert was null.");
            Assert.IsNotNull(SystemUnderTest.ExportTableName, "ExportTableName was null.");
        }

        [TestMethod]
        public void WhenInitializedThenCommandsAreNotNull()
        {
            Assert.IsNotNull(SystemUnderTest.RunQueryCommand, 
                "RunQueryCommand");
            Assert.IsNotNull(SystemUnderTest.RefreshConnectionsCommand, "RefreshConnectionsCommand");
            Assert.IsNotNull(SystemUnderTest.CreateInsertScriptCommand, "CreateInsertScriptCommand");
            Assert.IsNotNull(SystemUnderTest.CreateMergeIntoScriptCommand, "CreateMergeIntoScriptCommand");
        }

        [TestMethod]
        public void WhenInitializedThenFieldPropertiesSetToExpectedInitialValues()
        {
            Assert.IsTrue(SystemUnderTest.Query.IsVisible,
                "Query.IsVisible");
            Assert.IsTrue(SystemUnderTest.Query.IsEnabled,
                "Query.IsEnabled");
            Assert.IsTrue(SystemUnderTest.Query.IsValid,
                "Query.IsValid");

            Assert.IsTrue(SystemUnderTest.GeneratedQuery.IsVisible,
                "GeneratedQuery.IsVisible");
            Assert.IsFalse(SystemUnderTest.GeneratedQuery.IsEnabled,
                "GeneratedQuery.IsEnabled");
            Assert.IsTrue(SystemUnderTest.GeneratedQuery.IsValid,
                "GeneratedQuery.IsValid");

            Assert.IsTrue(SystemUnderTest.GenerateIdentityInsert.IsVisible,
                "GenerateIdentityInsert.IsVisible");
            Assert.IsTrue(SystemUnderTest.GenerateIdentityInsert.IsEnabled,
                "GenerateIdentityInsert.IsEnabled");
            Assert.IsTrue(SystemUnderTest.GenerateIdentityInsert.IsValid,
                "GenerateIdentityInsert.IsValid");

            Assert.IsTrue(SystemUnderTest.ExportTableName.IsVisible,
                "ExportTableName.IsVisible");
            Assert.IsFalse(SystemUnderTest.ExportTableName.IsEnabled,
                "ExportTableName.IsEnabled");
            Assert.IsTrue(SystemUnderTest.ExportTableName.IsValid,
                "ExportTableName.IsValid");
        }

        [TestMethod]
        public void ExportTableNameIsDetectedFromQuery()
        {
            ExportTableNameIsDetectedFromQuery(
                "select * from person", "person", true);
            ExportTableNameIsDetectedFromQuery(
                "select * from person", "person", true);
            ExportTableNameIsDetectedFromQuery(
                "select * from person where firstname=123", "person", true);

            ExportTableNameIsDetectedFromQuery(
                "select * From person", "person", true);
            ExportTableNameIsDetectedFromQuery(
                "select * From person", "person", true);
            ExportTableNameIsDetectedFromQuery(
                "select * From person where firstname=123", "person", true);

            ExportTableNameIsDetectedFromQuery(
                "select * FROM person", "person", true);
            ExportTableNameIsDetectedFromQuery(
                "select * FROM person", "person", true);
            ExportTableNameIsDetectedFromQuery(
                "select * FROM person where firstname=123", "person", true);
        }

        private void ExportTableNameIsDetectedFromQuery(
            string query, string expectedTableName, bool expectedIsValid)
        {
            SystemUnderTest.Query.Value = query;

            // act
            SystemUnderTest.RunQueryCommand.Execute(null);

            var actual = SystemUnderTest.ExportTableName.Value;

            // assert
            Assert.AreEqual<string>(expectedTableName, actual, "ExportTableName is wrong");
            Assert.AreEqual<bool>(expectedIsValid, SystemUnderTest.ExportTableName.IsValid, "IsValid");
        }

        [TestMethod]
        public void TableDescriptionIsPopulatedWhenQueryIsRun()
        {
            SystemUnderTest.Query.Value = "select * from person";
            this.DatabaseUtilityInstance.DescribeTableReturnValue =
                new TableDescription(UnitTestUtility.GetDescriptionDataTable());

            // act
            SystemUnderTest.RunQueryCommand.Execute(null);

            var actual = SystemUnderTest.TableDescription;

            // assert
            Assert.IsNotNull(actual, "Table description");
            Assert.AreSame(DatabaseUtilityInstance.DescribeTableReturnValue, actual, "Wrong instance");
        }
    }
}

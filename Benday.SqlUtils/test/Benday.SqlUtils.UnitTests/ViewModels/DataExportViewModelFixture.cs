using Benday.SqlUtils.Api;
using Benday.SqlUtils.Presentation.ViewModels;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
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
            _FileDialogServiceInstance = null;
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
                        DatabaseUtilityInstance, FileDialogServiceInstance
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

        private MockFileService _FileDialogServiceInstance;
        public MockFileService FileDialogServiceInstance
        {
            get
            {
                if (_FileDialogServiceInstance == null)
                {
                    _FileDialogServiceInstance = new MockFileService();
                }

                return _FileDialogServiceInstance;
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
            Assert.IsNotNull(SystemUnderTest.DatabaseConnections, "DatabaseConnections is null.");
            Assert.IsNotNull(SystemUnderTest.Query, "Query is null.");
            Assert.IsNotNull(SystemUnderTest.GeneratedQuery, "GeneratedQuery is null.");
            Assert.IsNotNull(SystemUnderTest.GenerateIdentityInsert, "GenerateIdentityInsert was null.");
            Assert.IsNotNull(SystemUnderTest.ExportTableName, "ExportTableName was null.");
            Assert.IsNotNull(SystemUnderTest.Message, "Message was null.");
            Assert.IsNotNull(SystemUnderTest.QueryResults, "QueryResults was null.");
            Assert.IsNotNull(SystemUnderTest.SaveToFileName, "SaveToFileName");
        }

        [TestMethod]
        public void WhenInitializedThenCommandsAreNotNull()
        {
            Assert.IsNotNull(SystemUnderTest.RunQueryCommand, 
                "RunQueryCommand");
            Assert.IsNotNull(SystemUnderTest.RefreshConnectionsCommand, "RefreshConnectionsCommand");
            Assert.IsNotNull(SystemUnderTest.CreateInsertScriptCommand, "CreateInsertScriptCommand");
            Assert.IsNotNull(SystemUnderTest.CreateMergeIntoScriptCommand, "CreateMergeIntoScriptCommand");
            Assert.IsNotNull(SystemUnderTest.SaveScriptCommand, "SaveScriptCommand");
        }

        [TestMethod]
        public void WhenInitializedThenFieldPropertiesSetToExpectedInitialValues()
        {
            Assert.IsFalse(SystemUnderTest.Message.IsVisible, 
                "Message should not be visible.");

            Assert.IsTrue(SystemUnderTest.QueryResults.IsVisible,
                "QueryResults.IsVisible");
            Assert.IsFalse(SystemUnderTest.QueryResults.IsEnabled,
                "QueryResults.IsEnabled");
            Assert.IsTrue(SystemUnderTest.QueryResults.IsValid,
                "QueryResults.IsValid");

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
            Assert.IsTrue(SystemUnderTest.DatabaseConnections.HasOnItemSelectedSubscriber, 
                "Database connections field should have an onitemselected subscriber.");

            SystemUnderTest.DatabaseConnections.SelectedItem =
                SystemUnderTest.DatabaseConnections.Items[0];

            Assert.IsTrue(DatabaseUtilityInstance.WasInitializeCalled, "Initialize wasn't called on db util before test.");

            SystemUnderTest.Query.Value = query;

            // act
            SystemUnderTest.RunQueryCommand.Execute(null);

            var actual = SystemUnderTest.ExportTableName.Value;

            // assert
            Assert.AreEqual<string>(expectedTableName, actual, "ExportTableName is wrong");
            Assert.AreEqual<bool>(expectedIsValid, SystemUnderTest.ExportTableName.IsValid, "IsValid");
            Assert.IsTrue(DatabaseUtilityInstance.WasInitializeCalled, "Initialize wasn't called on db util.");
        }

        [TestMethod]
        public void TableDescriptionIsPopulatedWhenQueryIsRun()
        {
            SystemUnderTest.Query.Value = "select * from recipe";
            this.DatabaseUtilityInstance.DescribeTableReturnValue =
                new TableDescription(UnitTestUtility.GetDescriptionDataTable());

            // act
            SystemUnderTest.RunQueryCommand.Execute(null);

            var actual = SystemUnderTest.TableDescription;

            // assert
            Assert.IsNotNull(actual, "Table description");
            Assert.AreSame(DatabaseUtilityInstance.DescribeTableReturnValue, actual, "Wrong instance");
        }

        [TestMethod]
        public void CreateMergeIntoScript()
        {
            SystemUnderTest.Query.Value = "select * from recipe";
            InitializeExportedData();
            this.DatabaseUtilityInstance.DescribeTableReturnValue =
                new TableDescription(UnitTestUtility.GetDescriptionDataTable());
            SystemUnderTest.RunQueryCommand.Execute(null);

            // act
            SystemUnderTest.CreateMergeIntoScriptCommand.Execute(null);

            var actual = SystemUnderTest.GeneratedQuery;

            // assert
            Assert.IsNotNull(actual, "Generated query field");

            Assert.IsFalse(SystemUnderTest.Message.IsVisible,
                "Message field should not be visible.  Message value is '{0}'",
                SystemUnderTest.Message.Value);

            Assert.IsNotNull(actual.Value, "Generated query field value");

            Console.WriteLine(actual.Value);

            Assert.IsFalse(String.IsNullOrWhiteSpace(actual.Value), "Generated query was empty");
            Assert.IsTrue(actual.Value.Contains("IDENTITY_INSERT"), "Should contain identity insert");
            Assert.IsTrue(actual.Value.Contains("MERGE INTO"), "Should contain 'merge into'");
        }

        [TestMethod]
        public void CreateInsertScript_NoIdentityInsert()
        {
            SystemUnderTest.Query.Value = "select * from recipe";
            InitializeExportedData();
            this.DatabaseUtilityInstance.DescribeTableReturnValue =
                new TableDescription(UnitTestUtility.GetDescriptionDataTable());
            SystemUnderTest.RunQueryCommand.Execute(null);

            // act
            SystemUnderTest.CreateInsertScriptCommand.Execute(null);

            var actual = SystemUnderTest.GeneratedQuery;

            // assert
            Assert.IsNotNull(actual, "Generated query field");

            Assert.IsFalse(SystemUnderTest.Message.IsVisible, 
                "Message field should not be visible.  Message value is '{0}'", 
                SystemUnderTest.Message.Value);

            Assert.IsNotNull(actual.Value, "Generated query field value");

            Console.WriteLine(actual.Value);

            Assert.IsFalse(String.IsNullOrWhiteSpace(actual.Value), "Generated query was empty");
            Assert.IsFalse(actual.Value.Contains("IDENTITY_INSERT"), "Should not contain identity insert");
        }

        [TestMethod]
        public void SaveScriptToDisk()
        {
            SystemUnderTest.Query.Value = "select * from recipe";
            InitializeExportedData();
            this.DatabaseUtilityInstance.DescribeTableReturnValue =
                new TableDescription(UnitTestUtility.GetDescriptionDataTable());
            SystemUnderTest.RunQueryCommand.Execute(null);
            SystemUnderTest.CreateInsertScriptCommand.Execute(null);
            var actual = SystemUnderTest.GeneratedQuery;

            _FileDialogServiceInstance.ShowFileDialogReturnValue = true;
            _FileDialogServiceInstance.Filename = "asdfasdfasdf.txt";

            // act
            SystemUnderTest.SaveScriptCommand.Execute(null);

            // assert

            Assert.IsTrue(SystemUnderTest.SaveToFileName.IsVisible,
                "SaveToFileName field should be visible.");

            Assert.IsTrue(_FileDialogServiceInstance.WasShowFileDialogCalled, "Show dialog was not called");
        }

        [TestMethod]
        public void CreateInsertScript_IdentityInsert()
        {
            SystemUnderTest.Query.Value = "select * from recipe";
            InitializeExportedData();
            this.DatabaseUtilityInstance.DescribeTableReturnValue =
                new TableDescription(UnitTestUtility.GetDescriptionDataTable());
            SystemUnderTest.RunQueryCommand.Execute(null);
            SystemUnderTest.GenerateIdentityInsert.Value = true;

            // act
            SystemUnderTest.CreateInsertScriptCommand.Execute(null);

            var actual = SystemUnderTest.GeneratedQuery;

            // assert
            Assert.IsNotNull(actual, "Generated query field");

            Assert.IsFalse(SystemUnderTest.Message.IsVisible,
                "Message field should not be visible.  Message value is '{0}'",
                SystemUnderTest.Message.Value);

            Assert.IsNotNull(actual.Value, "Generated query field value");

            Console.WriteLine(actual.Value);

            Assert.IsFalse(String.IsNullOrWhiteSpace(actual.Value), "Generated query was empty");
            Assert.IsTrue(actual.Value.Contains("IDENTITY_INSERT"), "Should contain identity insert");
        }

        private void InitializeExportedData()
        {
            var sampleFile = @"C:\code\repos\sql-server-utils\Benday.SqlUtils\test\Benday.SqlUtils.UnitTests\recipe-data-export-dataset.xml.txt";

            var dataset = new DataSet();

            var reader = new StringReader(File.ReadAllText(sampleFile));

            dataset.ReadXml(reader);

            this.DatabaseUtilityInstance.RunQueryReturnValue = dataset.Tables[0];
        }
    }
}

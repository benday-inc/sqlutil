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
        }

        private DataExportViewModel _SystemUnderTest;
        public DataExportViewModel SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new DataExportViewModel(
                        DatabaseConnectionStringRepositoryInstance);
                }

                return _SystemUnderTest;
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
    }
}

using Benday.Presentation;
using Benday.SqlUtils.Presentation.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Benday.SqlUtils.Api;

namespace Benday.SqlUtils.UnitTests.ViewModels
{
    [TestClass]
    public class SearchViewModelFixture : ViewModelFixtureBase
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
            _DatabaseConnectionStringRepositoryInstance = null;
        }

        private SearchViewModel _SystemUnderTest;
        public SearchViewModel SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new SearchViewModel(
                        MessageManagerInstance,
                        new DatasetQueryRunner(),
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
        public void WhenRefreshConnectionsIsCalledThenDatabaseConnectionsAreRefreshed()
        {
            Assert.IsNotNull(SystemUnderTest.DatabaseConnections, "DatabaseConnections was null.");
            Assert.AreNotEqual<int>(0, SystemUnderTest.DatabaseConnections.Count,
                "Wrong number of items.");
            var originalCount = SystemUnderTest.DatabaseConnections.Count;

            _DatabaseConnectionStringRepositoryInstance.Add(
                Guid.NewGuid().ToString(), "connection 3",
                _ValidConnectionStringUserNamePassword);

            SystemUnderTest.RefreshConnectionsCommand.Execute(null);

            var actualCount = SystemUnderTest.DatabaseConnections.Count;
        }

        [TestMethod]
        public void WhenInitializedWithDatabaseConnectionsThenTheFirstItemIsSelected()
        {
            Assert.IsNotNull(SystemUnderTest.DatabaseConnections, "DatabaseConnections was null.");
            Assert.AreNotEqual<int>(0, SystemUnderTest.DatabaseConnections.Count,
                "Wrong number of items.");

            Assert.IsTrue(SystemUnderTest.DatabaseConnections.Items[0].IsSelected, 
                "First item should be selected.");
        }

        [TestMethod]
        public void WhenInitializedTheDefaultSearchTypeIsTableName()
        {
            var expectedSearchType = "Table Name";

            AssertExpectedSearchType(expectedSearchType);
        }

        private void AssertExpectedSearchType(string expectedSearchType)
        {
            var actual = SystemUnderTest.SearchType.SelectedItem;

            Assert.IsNotNull(actual);
            Assert.AreEqual<string>(expectedSearchType, actual.Text, "Wrong search type was selected.");
        }

        [TestMethod]
        public void WhenInitializedThenFieldPropertiesAreNotNull()
        {
            Assert.IsNotNull(SystemUnderTest.SearchStringMethod, "SearchStringMethod is null.");
            Assert.IsNotNull(SystemUnderTest.SearchType, "SearchType is null.");
            Assert.IsNotNull(SystemUnderTest.SearchByColumnName, "SearchByColumnName was null.");
            Assert.IsNotNull(SystemUnderTest.SearchByTableName, "SearchByTableName was null.");
            Assert.IsNotNull(SystemUnderTest.SearchByValue, "SearchByValue was null.");
        }

        [TestMethod]
        public void WhenInitializedThenFieldPropertiesAreValid()
        {
            Assert.IsTrue(SystemUnderTest.DatabaseConnections.IsValid, "DatabaseConnections should be valid.");
            Assert.IsTrue(SystemUnderTest.SearchStringMethod.IsValid, "SearchStringMethod should be valid.");
            Assert.IsTrue(SystemUnderTest.SearchType.IsValid, "SearchType should be valid.");
            Assert.IsTrue(SystemUnderTest.SearchByColumnName.IsValid, "SearchByColumnName should be valid.");
            Assert.IsTrue(SystemUnderTest.SearchByTableName.IsValid, "SearchByTableName should be valid.");
            Assert.IsTrue(SystemUnderTest.SearchByValue.IsValid, "SearchByValue should be valid.");
        }

        [TestMethod]
        public void WhenInitializedThenSearchResultIsNull()
        {
            Assert.IsNull(SystemUnderTest.Result, "Result was null.");
        }

        [TestMethod]
        public void WhenSearchTypeIsSetToTableNameThenFieldVisibilityIsCorrect()
        {
            var expectedSearchType = "Table Name";

            SystemUnderTest.SearchType.SelectByText(expectedSearchType);

            AssertExpectedSearchType(expectedSearchType);
            
            AssertFieldVisibility(true, SystemUnderTest.SearchByTableName, "SearchByTableName");
            AssertFieldVisibility(false, SystemUnderTest.SearchByColumnName, "SearchByColumnName");
            AssertFieldVisibility(false, SystemUnderTest.SearchByColumnDataType, "SearchByColumnDataType");
            AssertFieldVisibility(false, SystemUnderTest.SearchByValue, "SearchByValue");
            AssertFieldVisibility(true, SystemUnderTest.SearchStringMethod, "SearchStringMethod");
        }

        private void AssertFieldVisibility(bool expectedVisibility,
            IVisibleField actualField, string fieldName)
        {
            Assert.AreEqual<bool>(expectedVisibility, 
                actualField.IsVisible, 
                "Visibility was wrong on field '{0}'.", 
                fieldName);
        }

        [TestMethod]
        public void WhenSearchTypeIsSetToColumnThenFieldVisibilityIsCorrect()
        {
            var expectedSearchType = Constants.SearchTypeColumn;

            SystemUnderTest.SearchType.SelectByText(expectedSearchType);

            AssertExpectedSearchType(expectedSearchType);

            AssertFieldVisibility(false, SystemUnderTest.SearchByTableName, "SearchByTableName");
            AssertFieldVisibility(true, SystemUnderTest.SearchByColumnName, "SearchByColumnName");
            AssertFieldVisibility(true, SystemUnderTest.SearchByColumnDataType, "SearchByColumnDataType");
            AssertFieldVisibility(false, SystemUnderTest.SearchByValue, "SearchByValue");
            AssertFieldVisibility(true, SystemUnderTest.SearchStringMethod, "SearchStringMethod");
        }

        [TestMethod]
        public void WhenSearchTypeIsSetToStoredProcedureNameThenFieldVisibilityIsCorrect()
        {
            var expectedSearchType = "Stored Procedure Name";

            SystemUnderTest.SearchType.SelectByText(expectedSearchType);

            AssertExpectedSearchType(expectedSearchType);

            AssertFieldVisibility(false, SystemUnderTest.SearchByTableName, "SearchByTableName");
            AssertFieldVisibility(false, SystemUnderTest.SearchByColumnName, "SearchByColumnName");
            AssertFieldVisibility(false, SystemUnderTest.SearchByColumnDataType, "SearchByColumnDataType");
            AssertFieldVisibility(true, SystemUnderTest.SearchByValue, "SearchByValue");
            AssertFieldVisibility(true, SystemUnderTest.SearchStringMethod, "SearchStringMethod");
        }

        [TestMethod]
        public void WhenSearchTypeIsSetToStoredProcedureParameterThenFieldVisibilityIsCorrect()
        {
            var expectedSearchType = "Stored Procedure Parameter Name";

            SystemUnderTest.SearchType.SelectByText(expectedSearchType);

            AssertExpectedSearchType(expectedSearchType);

            AssertFieldVisibility(false, SystemUnderTest.SearchByTableName, "SearchByTableName");
            AssertFieldVisibility(false, SystemUnderTest.SearchByColumnName, "SearchByColumnName");
            AssertFieldVisibility(false, SystemUnderTest.SearchByColumnDataType, "SearchByColumnDataType");
            AssertFieldVisibility(true, SystemUnderTest.SearchByValue, "SearchByValue");
            AssertFieldVisibility(true, SystemUnderTest.SearchStringMethod, "SearchStringMethod");
        }

        [TestMethod]
        public void WhenSearchTypeIsSetToStoredProcedureCodeThenFieldVisibilityIsCorrect()
        {
            var expectedSearchType = "Stored Procedure Source Code";

            SystemUnderTest.SearchType.SelectByText(expectedSearchType);

            AssertExpectedSearchType(expectedSearchType);

            AssertFieldVisibility(false, SystemUnderTest.SearchByTableName, "SearchByTableName");
            AssertFieldVisibility(false, SystemUnderTest.SearchByColumnName, "SearchByColumnName");
            AssertFieldVisibility(false, SystemUnderTest.SearchByColumnDataType, "SearchByColumnDataType");
            AssertFieldVisibility(true, SystemUnderTest.SearchByValue, "SearchByValue");
            AssertFieldVisibility(true, SystemUnderTest.SearchStringMethod, "SearchStringMethod");
        }

        [TestMethod]
        public void WhenSearchTypeIsSetToFindTextInAnyTableColumnThenFieldVisibilityIsCorrect()
        {
            var expectedSearchType = "Find Text In Any Table Column";

            SystemUnderTest.SearchType.SelectByText(expectedSearchType);

            AssertExpectedSearchType(expectedSearchType);

            AssertFieldVisibility(true, SystemUnderTest.SearchByTableName, "SearchByTableName");
            AssertFieldVisibility(true, SystemUnderTest.SearchByColumnName, "SearchByColumnName");
            AssertFieldVisibility(false, SystemUnderTest.SearchByColumnDataType, "SearchByColumnDataType");
            AssertFieldVisibility(true, SystemUnderTest.SearchByValue, "SearchByValue");
            AssertFieldVisibility(false, SystemUnderTest.SearchStringMethod, "SearchStringMethod");
        }

        [TestMethod]
        public void SearchStringMethodIsPopulatedWithExpectedValues()
        {
            var expected = new List<string>();
            // exact, starts with, ends with, contains

            expected.Add("Exact");
            expected.Add("Starts With");
            expected.Add("Ends With");
            expected.Add("Contains");

            var actual = SystemUnderTest.SearchStringMethod.Items;

            AssertValues(expected, actual);
        }

        private void AssertValues(List<string> expected, 
            ObservableCollection<ISelectableItem> actual)
        {
            foreach (var expectedItem in expected)
            {
                var match = (from temp in actual
                             where temp.Text == expectedItem
                             select temp).FirstOrDefault();

                Assert.IsNotNull(match, 
                    "Item '{0}' was not in the collection.", expectedItem);
            }

            Assert.AreEqual<int>(expected.Count, actual.Count, 
                "There should be the same number of items.");
        }

        [TestMethod]
        public void SearchTypeIsPopulatedWithExpectedValues()
        {
            var expected = new List<string>();
            // exact, starts with, ends with, contains

            expected.Add("Table Name");
            expected.Add("Column Name");
            expected.Add("Stored Procedure Name");
            expected.Add("Stored Procedure Parameter Name");
            expected.Add("Stored Procedure Source Code");
            expected.Add("Find Text In Any Table Column");

            var actual = SystemUnderTest.SearchType.Items;

            AssertValues(expected, actual);
        }








    }
}

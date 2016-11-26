using Benday.Presentation;
using Benday.SqlServerUtilities.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Benday.SqlServerUtilities.UnitTests.ViewModels
{
    [TestClass]
    public class SearchViewModelFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private SearchViewModel _SystemUnderTest;
        public SearchViewModel SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new SearchViewModel();
                }

                return _SystemUnderTest;
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
        public void WhenInitializedThenPropertiesAreNotNull()
        {
            Assert.IsNotNull(SystemUnderTest.SearchStringMethod, "SearchStringMethod is null.");
            Assert.IsNotNull(SystemUnderTest.SearchType, "SearchType is null.");
            Assert.IsNotNull(SystemUnderTest.SearchByColumnName, "SearchByColumnName was null.");
            Assert.IsNotNull(SystemUnderTest.SearchByTableName, "SearchByTableName was null.");
            Assert.IsNotNull(SystemUnderTest.SearchByValue, "SearchByValue was null.");
        }

        [TestMethod]
        public void WhenInitializedThenSearchResultsIsNonNullAndZeroLength()
        {
            Assert.IsNotNull(SystemUnderTest.Results, "Results was null.");
            Assert.AreEqual<int>(0, SystemUnderTest.Results.Count, "Result count should be zero.");
        }

        [TestMethod]
        public void WhenSearchTypeIsSetToTableNameThenFieldVisibilityIsCorrect()
        {
            var expectedSearchType = "Table Name";

            SystemUnderTest.SearchType.SelectByText(expectedSearchType);

            AssertExpectedSearchType(expectedSearchType);
            
            AssertFieldVisibility(true, SystemUnderTest.SearchByTableName, "SearchByTableName");
            AssertFieldVisibility(false, SystemUnderTest.SearchByColumnName, "SearchByColumnName");
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
        public void WhenSearchTypeIsSetToColumnNameThenFieldVisibilityIsCorrect()
        {
            var expectedSearchType = "Column Name";

            SystemUnderTest.SearchType.SelectByText(expectedSearchType);

            AssertExpectedSearchType(expectedSearchType);

            AssertFieldVisibility(false, SystemUnderTest.SearchByTableName, "SearchByTableName");
            AssertFieldVisibility(true, SystemUnderTest.SearchByColumnName, "SearchByColumnName");
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
            AssertFieldVisibility(true, SystemUnderTest.SearchByValue, "SearchByValue");
            AssertFieldVisibility(true, SystemUnderTest.SearchStringMethod, "SearchStringMethod");
        }

        [TestMethod]
        public void WhenSearchTypeIsSetToStoredProcedureParameterThenFieldVisibilityIsCorrect()
        {
            var expectedSearchType = "Stored Procedure Parameter";

            SystemUnderTest.SearchType.SelectByText(expectedSearchType);

            AssertExpectedSearchType(expectedSearchType);

            AssertFieldVisibility(false, SystemUnderTest.SearchByTableName, "SearchByTableName");
            AssertFieldVisibility(false, SystemUnderTest.SearchByColumnName, "SearchByColumnName");
            AssertFieldVisibility(true, SystemUnderTest.SearchByValue, "SearchByValue");
            AssertFieldVisibility(true, SystemUnderTest.SearchStringMethod, "SearchStringMethod");
        }

        [TestMethod]
        public void WhenSearchTypeIsSetToStoredProcedureCodeThenFieldVisibilityIsCorrect()
        {
            var expectedSearchType = "Stored Procedure Code";

            SystemUnderTest.SearchType.SelectByText(expectedSearchType);

            AssertExpectedSearchType(expectedSearchType);

            AssertFieldVisibility(false, SystemUnderTest.SearchByTableName, "SearchByTableName");
            AssertFieldVisibility(false, SystemUnderTest.SearchByColumnName, "SearchByColumnName");
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

using Benday.SqlUtils.Api;
using Benday.SqlUtils.Presentation.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benday.SqlUtils.UnitTests.ViewModels
{
    [TestClass]
    public class SearchFieldViewModelFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private SearchFieldViewModel _SystemUnderTest;

        private SearchFieldViewModel SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new SearchFieldViewModel();
                }

                return _SystemUnderTest;
            }
        }
                
        [TestMethod]
        public void WhenInitializedThenSearchTypesArePopulated()
        {
            Assert.IsNotNull(SystemUnderTest.SearchType, "SearchType was null.");
            Assert.AreNotEqual<int>(0, SystemUnderTest.SearchType.Count,
                "Wrong number of items.");
            Assert.AreEqual<int>(3, SystemUnderTest.SearchType.Count, "Wrong number of items");

            AssertExpectedSearchType(0, Constants.SearchTypeByValue, true);
            AssertExpectedSearchType(1, Constants.SearchTypeBlankOrEmpty, false);
            AssertExpectedSearchType(2, Constants.SearchTypeNotBlankOrEmpty, false);
        }

        private void AssertExpectedSearchType(
            int index, string expectedSearchType, bool expectedIsSelected)
        {
            var actual = SystemUnderTest.SearchType.Items[index];

            Assert.IsNotNull(actual);
            Assert.AreEqual<string>(expectedSearchType, actual.Text, 
                $"Wrong search type was selected for index '{index}'.");
            Assert.AreEqual<bool>(expectedIsSelected, actual.IsSelected, 
                $"Wrong IsSelected value for index '{index}'");

            if (expectedIsSelected == true)
            {
                Assert.AreSame(SystemUnderTest.SearchType.SelectedItem, actual,
                    $"Item at index '{index}' should be the SelectedItem");
            }
            else
            {
                Assert.AreNotSame(SystemUnderTest.SearchType.SelectedItem, actual,
                    $"Item at index '{index}' should not be the SelectedItem");
            }
        }

        
    }
}

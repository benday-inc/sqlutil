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
        }
    }
}

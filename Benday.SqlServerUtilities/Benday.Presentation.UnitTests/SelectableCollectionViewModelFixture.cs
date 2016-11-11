using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benday.Presentation;
using System.Linq;

namespace Benday.Presentation.UnitTests
{
    [TestClass]
    public class SelectableCollectionViewModelFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }
        

        private SelectableCollectionViewModel<SelectableItem> _SystemUnderTest;
        public SelectableCollectionViewModel<SelectableItem> SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new SelectableCollectionViewModel<SelectableItem>();
                }

                return _SystemUnderTest;
            }
        }

        private List<SelectableItem> CreateValues()
        {
            var values = new List<SelectableItem>();

            for (int i = 0; i < 10; i++)
            {
                var temp = new SelectableItem();

                temp.IsSelected = false;
                temp.Text = String.Format("text_{0}", i);
                temp.Value = String.Format("value_{0}", i);

                values.Add(temp);
            }

            return values;
        }

        [TestMethod]
        public void InitializeFromCollectionIsCalledThereShouldNotBeASelectedItem()
        {
            var values = CreateValues();
            SystemUnderTest.Initialize(values);

            Assert.IsNull(SystemUnderTest.SelectedItem, "SelectedItem should be null.");
        }

        [TestMethod]
        public void SelectedItemPropertyShouldBePopulatedWhenIsSelectedIsSetOnAnItem()
        {
            var values = CreateValues();
            SystemUnderTest.Initialize(values);

            // select an item
            var expected = values[3];
            expected.IsSelected = true;

            var actual = SystemUnderTest.SelectedItem;

            Assert.IsNotNull(actual, "SelectedItem should not be null.");
            Assert.AreSame(expected, actual, "Wrong selected item.");
        }

        [TestMethod]
        public void SelectedItemPropertyBeNullWhenNoSelectedItems()
        {
            var values = CreateValues();
            SystemUnderTest.Initialize(values);

            // select an item
            var expected = values[3];
            expected.IsSelected = true;

            // unselect the item
            expected.IsSelected = false;

            var actual = SystemUnderTest.SelectedItem;

            Assert.IsNull(actual, "SelectedItem should be null.");
        }

        [TestMethod]
        public void WhenSelectedItemIsSetViaAssignmentThenIsSelectedShouldBeSetToTrueOnItem()
        {
            var values = CreateValues();
            SystemUnderTest.Initialize(values);

            // select an item
            var expected = values[3];

            SystemUnderTest.SelectedItem = expected;
            
            Assert.IsTrue(expected.IsSelected, "IsSelected should be true for selected item.");
        }

        [TestMethod]
        public void WhenInMultiSelectModeSelectedItemIsSetViaAssignmentThenIsSelectedShouldBeSetToTrueOnItem()
        {
            SystemUnderTest.AllowMultipleSelections = true;

            var values = CreateValues();
            SystemUnderTest.Initialize(values);

            // select an item
            var item0 = values[3];
            var item1 = values[5];

            SystemUnderTest.SelectedItem = item0;
            SystemUnderTest.SelectedItem = item1;

            Assert.IsTrue(item0.IsSelected, "IsSelected should be true for selected item.");
            Assert.IsTrue(item1.IsSelected, "IsSelected should be true for selected item.");
        }

        [TestMethod]
        public void WhenSelectedItemIsSetViaAssignmentToNullThenIsSelectedShouldBeSetToFalseOnItem()
        {
            var values = CreateValues();
            SystemUnderTest.Initialize(values);

            // select an item
            var expected = values[3];
            SystemUnderTest.SelectedItem = expected;

            // unselect the item via assignment
            SystemUnderTest.SelectedItem = null;
            
            Assert.IsFalse(expected.IsSelected, 
                "IsSelected should be false for previously selected item.");
        }

        [TestMethod]
        public void WhenSelectedItemIsSetViaAssignmentToNullThenIsSelectedShouldBeSetToFalseOnAllItems()
        {
            var values = CreateValues();
            SystemUnderTest.Initialize(values);

            // select an item
            var item0 = values[3];
            var item1 = values[5];
            var item2 = values[7];

            item0.IsSelected = true;
            item1.IsSelected = true;
            item2.IsSelected = true;
            
            // unselect the item via assignment
            SystemUnderTest.SelectedItem = null;

            foreach (var item in SystemUnderTest.Items)
            {
                Assert.IsFalse(item.IsSelected);
            }

            Assert.IsFalse(item0.IsSelected);
            Assert.IsFalse(item1.IsSelected);
            Assert.IsFalse(item2.IsSelected);
        }

        [TestMethod]
        public void InitializedToNotAllowMultipleSelectionsByDefault()
        {
            Assert.IsFalse(SystemUnderTest.AllowMultipleSelections, 
                "AllowMultipleSelections was wrong.");
        }

        [TestMethod]
        public void WhenInMultiSelectModeThenMultipleItemsCanBeSelected()
        {
            SystemUnderTest.AllowMultipleSelections = true;

            var values = CreateValues();
            SystemUnderTest.Initialize(values);

            // select an item
            var item0 = values[3];
            var item1 = values[5];

            item0.IsSelected = true;
            item1.IsSelected = true;

            Assert.AreSame(item0, SystemUnderTest.SelectedItem);
            Assert.IsTrue(item0.IsSelected);
            Assert.IsTrue(item1.IsSelected);
        }

        [TestMethod]
        public void WhenInSingleSelectModeThenMultipleItemsCannotBeSelected()
        {
            SystemUnderTest.AllowMultipleSelections = false;

            var values = CreateValues();
            SystemUnderTest.Initialize(values);

            // select an item
            var item0 = values[3];
            var item1 = values[5];

            item0.IsSelected = true;
            item1.IsSelected = true;

            Assert.AreSame(item1, SystemUnderTest.SelectedItem);
            Assert.IsFalse(item0.IsSelected);
            Assert.IsTrue(item1.IsSelected);
        }
    }
}

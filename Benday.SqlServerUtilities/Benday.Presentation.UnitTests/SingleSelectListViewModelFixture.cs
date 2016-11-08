using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benday.Presentation.UnitTests
{
    [TestClass]
    public class SingleSelectListViewModelFixture
    {
        private List<ISelectableItem> CreateValues()
        {
            List<ISelectableItem> values = new List<ISelectableItem>();

            for (int i = 0; i < 10; i++)
            {
                ISelectableItem temp = new SelectableItem();

                temp.IsSelected = false;
                temp.Text = String.Format("text_{0}", i);
                temp.Value = String.Format("value_{0}", i);

                values.Add(temp);
            }

            return values;
        }

        [TestMethod]
        public void SingleSelectListViewModel_SelectedItem()
        {
            var values = CreateValues();

            var instance = new SingleSelectListViewModel(values);

            Assert.IsNull(instance.SelectedItem, "SelectedItem should be null.");

            ISelectableItem expectedSelectedItem;
            ISelectableItem actualSelectedItem;

            // select an item
            expectedSelectedItem = values[3];
            expectedSelectedItem.IsSelected = true;
            actualSelectedItem = instance.SelectedItem;
            Assert.IsNotNull(actualSelectedItem, "SelectedItem should not be null.");
            Assert.AreSame(expectedSelectedItem, actualSelectedItem, "Wrong selected item.");

            // unselect the item
            expectedSelectedItem.IsSelected = false;
            Assert.IsNull(instance.SelectedItem, "SelectedItem should be null after being unselected.");

            // select a different item
            expectedSelectedItem = values[2];
            expectedSelectedItem.IsSelected = true;
            actualSelectedItem = instance.SelectedItem;
            Assert.IsNotNull(actualSelectedItem, "SelectedItem should not be null.");
            Assert.AreSame(expectedSelectedItem, actualSelectedItem, "Wrong selected item.");
        }

        [TestMethod]
        public void SingleSelectListViewModel_SelectedItem_CallSetterSetsIsSelectedToTrueOnSelectedItem()
        {
            var values = CreateValues();

            var instance = new SingleSelectListViewModel(values);

            Assert.IsNull(instance.SelectedItem, "SelectedItem should be null.");

            var makeThisTheSelectedItem = values[0];

            Assert.IsFalse(makeThisTheSelectedItem.IsSelected, "IsSelected should be false.");

            instance.SelectedItem = makeThisTheSelectedItem;

            Assert.IsTrue(makeThisTheSelectedItem.IsSelected, "IsSelected should be true.");
        }

        [TestMethod]
        public void SingleSelectListViewModel_WhenSelectedItemChanges_OldSelectedItemShouldBeChangedToIsSelectedFalse()
        {
            var values = CreateValues();

            var instance = new SingleSelectListViewModel(values);

            // make sure that there is a selected item
            var originalSelectedItem = values[0];
            instance.SelectedItem = originalSelectedItem;
            Assert.IsTrue(originalSelectedItem.IsSelected, "IsSelected should be true.");

            // get the item that's going to be the new selected item
            var newSelectedItem = values[1];
            Assert.IsFalse(newSelectedItem.IsSelected, "Should not be selected.");

            // change the selected item
            instance.SelectedItem = newSelectedItem;
            Assert.IsTrue(newSelectedItem.IsSelected, "New item should be selected.");
            Assert.IsFalse(originalSelectedItem.IsSelected, "Old item should not be selected.");
        }

        [TestMethod]
        public void SingleSelectListViewModel_WhenSelectedItemChangesToNull_OldSelectedItemShouldBeChangedToIsSelectedFalse()
        {
            var values = CreateValues();

            var instance = new SingleSelectListViewModel(values);

            // make sure that there is a selected item
            var originalSelectedItem = values[0];
            instance.SelectedItem = originalSelectedItem;

            // set selecteditem to null
            instance.SelectedItem = null;
            Assert.IsNull(instance.SelectedItem, "SelectedItem should be null after being set to null.");

            Assert.IsFalse(originalSelectedItem.IsSelected, "Old item should not be selected.");
        }

        [TestMethod]
        public void SingleSelectListViewModel_SelectItemByText_OldSelectedItemShouldBeChangedToIsSelectedFalse()
        {
            var values = CreateValues();

            var instance = new SingleSelectListViewModel(values);

            // make sure that there is a selected item
            var originalSelectedItem = values[0];
            instance.SelectedItem = originalSelectedItem;
            Assert.IsTrue(originalSelectedItem.IsSelected, "IsSelected should be true.");

            // get the item that's going to be the new selected item
            var newSelectedItem = values[1];
            Assert.IsFalse(newSelectedItem.IsSelected, "Should not be selected.");

            // change the selected item
            instance.SelectByText(newSelectedItem.Text);

            Assert.IsTrue(newSelectedItem.IsSelected, "New item should be selected.");
            Assert.IsFalse(originalSelectedItem.IsSelected, "Old item should not be selected.");
            Assert.AreSame(newSelectedItem, instance.SelectedItem);
        }
    }
}

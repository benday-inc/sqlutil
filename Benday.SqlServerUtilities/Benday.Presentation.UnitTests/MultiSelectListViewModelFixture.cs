using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Benday.Presentation.UnitTests
{
    [TestClass]
    public class MultiSelectListViewModelFixture 
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
        public void MultiSelectListViewModel_SelectedItem()
        {
            var values = CreateValues();

            var instance = new MultiSelectListViewModel(values);

            Assert.IsNull(instance.SelectedItem, "SelectedItem should be null.");
            Assert.IsNotNull(instance.SelectedItems, "SelectedItems should not be null.");
            Assert.AreEqual<int>(0, instance.SelectedItems.Count, "SelectedItems count should not be null.");
            
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

            // select a second item that is in the collection after values[2]
            values[3].IsSelected = true;
            Assert.AreSame(expectedSelectedItem, instance.SelectedItem, "Wrong selected item.");
        }

        [TestMethod]
        public void MultiSelectListViewModel_SelectedItems()
        {
            var values = CreateValues();

            var instance = new MultiSelectListViewModel(values);

            Assert.IsNull(instance.SelectedItem, "SelectedItem should be null.");
            Assert.IsNotNull(instance.SelectedItems, "SelectedItems should not be null.");
            Assert.AreEqual<int>(0, instance.SelectedItems.Count, "SelectedItems count should not be null.");
            

            ISelectableItem expectedSelectedItem0;
            ISelectableItem expectedSelectedItem1;
            ISelectableItem actualSelectedItem0;
            ISelectableItem actualSelectedItem1;

            // select an item
            expectedSelectedItem0 = values[2];
            expectedSelectedItem0.IsSelected = true;

            Assert.IsNotNull(instance.SelectedItems, "SelectedItems should not be null.");
            Assert.AreEqual<int>(1, instance.SelectedItems.Count, "SelectedItems count should not be null.");


            expectedSelectedItem1 = values[3];
            expectedSelectedItem1.IsSelected = true;
            
            Assert.IsNotNull(instance.SelectedItems, "SelectedItems should not be null.");
            Assert.AreEqual<int>(2, instance.SelectedItems.Count, "SelectedItems count should not be null.");


            var actualSelectedItems = instance.SelectedItems;

            actualSelectedItem0 = actualSelectedItems[0];
            actualSelectedItem1 = actualSelectedItems[1];
            
            Assert.AreSame(expectedSelectedItem0, actualSelectedItem0, "Wrong selected item.");
            Assert.AreSame(expectedSelectedItem1, actualSelectedItem1, "Wrong selected item.");
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Benday.Presentation.UnitTests
{
    public class NotifyPropertyChangedTester
    {
        public NotifyPropertyChangedTester(INotifyPropertyChanged viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException("viewModel", "Argument cannot be null.");
            }

            this.Changes = new List<string>();

            viewModel.PropertyChanged += new PropertyChangedEventHandler(viewModel_PropertyChanged);
        }

        protected virtual void OnPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            Changes.Add(e.PropertyName);
        }

        void viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChangedEvent(sender, e);
        }

        public List<string> Changes { get; private set; }

        public void AssertChange(int changeIndex, string expectedPropertyName)
        {
            Assert.IsNotNull(Changes, "Changes collection was null.");

            Assert.IsTrue(changeIndex < Changes.Count,
                "Changes collection contains '{0}' items and does not have an element at index '{1}'.",
                Changes.Count,
                changeIndex);

            Assert.AreEqual<string>(expectedPropertyName,
                Changes[changeIndex],
                "Change at index '{0}' is '{1}' and is not equal to '{2}'.",
                changeIndex,
                Changes[changeIndex],
                expectedPropertyName);
        }

        public void AssertChange(string expectedPropertyName)
        {
            Assert.IsNotNull(Changes, "Changes collection was null.");

            Assert.IsTrue(Changes.Contains(expectedPropertyName),
                "Changes collection does not contain a change for a property named '{0}'.",
                expectedPropertyName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benday.Presentation.UnitTests
{
    public class ProgressBarViewModelTester : NotifyPropertyChangedTester
    {
        public ProgressBarViewModelTester(IProgressBarViewModel viewModel) :
            base(viewModel)
        {
            m_ViewModelInstance = viewModel;

            IsVisibleValues = new List<bool>();
            MessageValues = new List<string>();
        }

        private IProgressBarViewModel m_ViewModelInstance;
        public IProgressBarViewModel ViewModelInstance
        {
            get { return m_ViewModelInstance; }
        }

        protected override void OnPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsProgressBarVisible" || e.PropertyName == "ProgressBarMessage")
            {
                base.OnPropertyChangedEvent(sender, e);

                if (e.PropertyName == "IsProgressBarVisible")
                {
                    IsVisibleValues.Add(ViewModelInstance.IsProgressBarVisible);
                }
                else if (e.PropertyName == "ProgressBarMessage")
                {
                    MessageValues.Add(ViewModelInstance.ProgressBarMessage);
                }
            }
        }

        public List<bool> IsVisibleValues { get; private set; }
        public List<string> MessageValues { get; private set; }

        public void AssertProgressBarWasVisibleAndEndedInvisible()
        {
            Assert.IsTrue(IsVisibleValues.Count >= 2, "Didn't get at least 2 IsProgressBarVisible changes.  Only got '{0}'.", IsVisibleValues.Count);

            int lastIndex = IsVisibleValues.Count - 1;

            Assert.IsFalse(IsVisibleValues[lastIndex], "Did not end with progress bar not visible.");

            Assert.IsTrue(IsVisibleValues.Contains(true), "Progress bar was never visible.");
        }

        public void AssertMessage(string expectedMessage)
        {
            Assert.IsTrue(MessageValues.Contains(expectedMessage), "Never got the message '{0}'.", expectedMessage);
        }
    }
}

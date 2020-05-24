using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Benday.Presentation.UnitTests
{
    public class MessageBoxViewModelTester : NotifyPropertyChangedTester
    {
        public MessageBoxViewModelTester(IViewModelBase viewModel) :
            base(viewModel)
        {
            m_ViewModelInstance = viewModel;

            m_ViewModelInstance.MessageBoxRequested += 
                new MessageBoxEventHandler(_ViewModelInstance_MessageBoxRequested);
        }

        public bool WasMessageBoxRequested { get; set; }

        public MessageBoxEventArgs LastEventArgs { get; set; }

        void _ViewModelInstance_MessageBoxRequested(object sender, MessageBoxEventArgs args)
        {
            WasMessageBoxRequested = true;

            LastEventArgs = args;
        }

        private IViewModelBase m_ViewModelInstance;
        public IViewModelBase ViewModelInstance
        {
            get { return m_ViewModelInstance; }
        }

        public void AssertException<T>() where T : Exception
        {
            Assert.IsNotNull(LastEventArgs, "LastEventArgs was null.");
            Assert.IsNotNull(LastEventArgs.Exception, "Did not receive an exception.");
            Assert.IsInstanceOfType(LastEventArgs.Exception, typeof(T), "Exception was not the expected type.");
        }

        public void AssertMessage(string expectedMessage)
        {
            Assert.IsNotNull(LastEventArgs, "LastEventArgs was null.");
            Assert.IsNotNull(LastEventArgs.Message, "Did not receive a message.");
            Assert.AreEqual<string>(expectedMessage, LastEventArgs.Message, "Message was not correct.");
        }

        public void AssertMessage(string expectedMessage, bool expectedIsUnexpectedException)
        {
            AssertMessage(expectedMessage);
            Assert.AreEqual<bool>(expectedIsUnexpectedException, LastEventArgs.IsUnexpectedException, "IsUnexpectedException was wrong.");
        }
    }
}

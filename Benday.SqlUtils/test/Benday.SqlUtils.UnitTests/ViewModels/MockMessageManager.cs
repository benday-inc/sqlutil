using Benday.Presentation;
using System;

namespace Benday.SqlUtils.UnitTests.ViewModels
{
    public class MockMessageManager : IMessageManager
    {
        public MockMessageManager()
        {
        }

        public bool WasShowMessageCalled { get; set; }
        public Exception ReceivedShowMessageCallExceptionArg { get; set; }

        public void ShowMessage(Exception ex)
        {
            WasShowMessageCalled = true;
            ReceivedShowMessageCallExceptionArg = ex;
        }
    }
}

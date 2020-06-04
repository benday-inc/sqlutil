using System;
using System.Collections.Generic;

namespace Benday.Presentation
{
    public class MessageBoxEventArgs : EventArgs
    {
        public MessageBoxEventArgs(string message)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("message is null or empty.", "message");

            Message = message;
            IsUnexpectedException = false;
            Exception = null;
        }

        /// <summary>
        /// Initializes a new instance of the MessageBoxEventArgs class.
        /// </summary>
        public MessageBoxEventArgs(string message, bool isUnexpectedException, Exception exception)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentException("message is null or empty.", "message");
            if (exception == null)
                throw new ArgumentNullException("exception", "exception is null.");

            Message = message;
            IsUnexpectedException = isUnexpectedException;
            Exception = exception;
        }

        /// <summary>
        /// Initializes a new instance of the MessageBoxEventArgs class.
        /// </summary>
        public MessageBoxEventArgs(string message, Exception exception)
            : this(message, true, exception)
        {

        }

        public string Message { get; private set; }
        public bool IsUnexpectedException { get; private set; }
        public Exception Exception { get; private set; }
    }
}

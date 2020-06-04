using System;

namespace Benday.SqlUtils.ConsoleUi
{
    public class KnownException : Exception
    {
        public KnownException() { }
        public KnownException(string message) : base(message) { }
        public KnownException(string message, Exception innerException) : base(message, innerException) { }
    }
}

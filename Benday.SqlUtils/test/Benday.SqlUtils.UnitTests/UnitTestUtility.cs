using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.SqlServerUtilities.UnitTests
{
    public static class UnitTestUtility
    {
        public static void AssertIsNotNullOrWhitespace(string actual, string message)
        {
            if (String.IsNullOrWhiteSpace(actual) == true)
            {
                Assert.Fail(String.Format("{0} - {1}", nameof(AssertIsNotNullOrWhitespace), message));
            }
        }
    }
}

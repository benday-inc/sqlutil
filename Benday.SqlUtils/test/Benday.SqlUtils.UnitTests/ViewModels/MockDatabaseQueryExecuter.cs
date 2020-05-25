using Benday.SqlUtils.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Benday.SqlUtils.UnitTests.ViewModels
{
    public class MockDatabaseQueryExecuter : IDatabaseQueryExecuter
    {
        private bool _WasRunQueryCalled;
        public bool WasRunQueryCalled
        {
            get { return _WasRunQueryCalled; }
        }

        public DataTable RunQueryReturnValue { get; set; }

        public DataTable RunQuery(string query)
        {
            _WasRunQueryCalled = true;

            return RunQueryReturnValue;
        }
    }
}

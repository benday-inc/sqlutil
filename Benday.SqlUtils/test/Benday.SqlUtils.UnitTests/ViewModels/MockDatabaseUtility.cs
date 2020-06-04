using Benday.SqlUtils.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Benday.SqlUtils.UnitTests.ViewModels
{
    public class MockDatabaseUtility : IDatabaseUtility
    {
        private bool _WasRunQueryCalled;
        public bool WasRunQueryCalled
        {
            get { return _WasRunQueryCalled; }
        }

        public DataTable RunQueryReturnValue { get; set; }        

        public DataTable RunQuery(string query, Dictionary<string, string> args = null)
        {
            _WasRunQueryCalled = true;

            return RunQueryReturnValue;
        }

        private bool _WasDescribeTableCalled;
        public bool WasDescribeTableCalled
        {
            get { return _WasDescribeTableCalled; }
        }

        public TableDescription DescribeTableReturnValue { get; set; }

        public TableDescription DescribeTable(string tableName)
        {
            _WasDescribeTableCalled = true;
            return DescribeTableReturnValue;
        }

        public bool WasInitializeCalled { get; set; }
        public void Initialize(string connectionString)
        {
            WasInitializeCalled = true;
        }
    }
}

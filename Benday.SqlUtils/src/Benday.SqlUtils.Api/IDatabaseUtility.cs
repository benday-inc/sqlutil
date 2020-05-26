using System;
using System.Collections.Generic;
using System.Data;

namespace Benday.SqlUtils.Api
{
    public interface IDatabaseUtility
    {
        public DataTable RunQuery(string query, Dictionary<string, string> args = null);
        public TableDescription DescribeTable(string tableName);
        void Initialize(string connectionString);
    }
}
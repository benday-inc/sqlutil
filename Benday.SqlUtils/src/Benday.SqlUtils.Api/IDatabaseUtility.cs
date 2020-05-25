using System;
using System.Data;

namespace Benday.SqlUtils.Api
{
    public interface IDatabaseUtility
    {
        public DataTable RunQuery(string query);
        public DataTable DescribeTable(string tableName);
    }
}
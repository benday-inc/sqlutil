using System;
using System.Data;

namespace Benday.SqlUtils.Api
{
    public interface IDatabaseQueryExecuter
    {
        public DataTable RunQuery(string query);
    }
}
using Benday.SqlUtils.Presentation.ViewModels;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Benday.SqlUtils.UnitTests.ViewModels
{
    public class MockQueryRunner : IQueryRunner
    {

        public MockQueryRunner()
        {

        }

        public void Initialize(string connectionString)
        {
            throw new NotImplementedException();
        }

        public DataSet Run(SqlCommand command)
        {
            throw new NotImplementedException();
        }
    }
}

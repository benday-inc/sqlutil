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
            
        }

        public bool WasRunCalled { get; set; }

        public DataSet Run(SqlCommand command)
        {
            WasRunCalled = true;

            return RunReturnValue;
        }

        public void ExecuteNonQuery(SqlCommand command)
        {
            throw new NotImplementedException();
        }

        public DataSet RunReturnValue { get; set; }
    }
}

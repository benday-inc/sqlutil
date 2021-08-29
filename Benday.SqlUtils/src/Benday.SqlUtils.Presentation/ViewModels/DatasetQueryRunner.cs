using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class DatasetQueryRunner : IQueryRunner
    {
        private string _connectionString;

        public void Initialize(string connectionString)
        {
            _connectionString = connectionString;
        }
        

        public DataSet Run(SqlCommand command)
        {
            if (string.IsNullOrWhiteSpace(_connectionString) == true)
            {
                throw new InvalidOperationException("DatasetQueryRunner not initialized.");
            }

            DataSet results = new DataSet();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                command.Connection = connection;

                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(results);
                }
            }

            return results;
        }
    }
}

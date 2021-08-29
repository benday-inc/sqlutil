using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public interface IQueryRunner
    {
        void Initialize(string connectionString);
        DataSet Run(SqlCommand command);
    }
}

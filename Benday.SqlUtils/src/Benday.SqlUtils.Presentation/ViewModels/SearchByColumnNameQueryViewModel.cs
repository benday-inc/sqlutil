using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchByColumnNameQueryViewModel : DatabaseQueryViewModelBase
    {
        protected override string SqlQueryTemplate
        {
            get
            {
                if (HasArgumentValue("COLUMN_DATA_TYPE") == true &&
                    HasArgumentValue("COLUMN_NAME") == false)
                {
                    return @"select table_schema, table_name, column_name, data_type, character_maximum_length
from information_schema.columns where 
data_type like @COLUMN_DATA_TYPE 
ORDER BY COLUMN_NAME, TABLE_NAME";
                }
                else if (HasArgumentValue("COLUMN_DATA_TYPE") == false &&
                    HasArgumentValue("COLUMN_NAME") == true)
                {
                    return @"select table_schema, table_name, column_name, data_type, character_maximum_length
from information_schema.columns where 
column_name like @COLUMN_NAME 
ORDER BY COLUMN_NAME, TABLE_NAME";
                }
                else if (HasArgumentValue("COLUMN_DATA_TYPE") == true &&
                    HasArgumentValue("COLUMN_NAME") == true)
                {
                    return @"select table_schema, table_name, column_name, data_type, character_maximum_length
from information_schema.columns where 
data_type like @COLUMN_DATA_TYPE and
column_name like @COLUMN_NAME ORDER BY COLUMN_NAME, TABLE_NAME";
                }
                else
                {
                    throw new InvalidOperationException("SqlQueryTemplate property has an unsupported combination of arguments.");
                }
            }
        }

        protected override List<string> GetRequiredArguments()
        {
            return new List<string>();
        }

        protected override void ValidateArguments()
        {
            if (HasArgumentValue("COLUMN_NAME") == false && 
                HasArgumentValue("COLUMN_DATA_TYPE") == false)
            {
                ValidationMessage = String.Format("A value is required for 'COLUMN_NAME' or 'COLUMN_DATA_TYPE'."); 
            }
        }

        private ObservableCollection<object> ToModels(DataTable dataTable)
        {
            var returnValue = new ObservableCollection<object>();

            foreach (DataRow item in dataTable.Rows)
            {
                returnValue.Add(new SearchByNameResultRow(item));
            }

            return returnValue;
        }

        public override void Execute()
        {
            IsVisible = false;

            DataSet results = new DataSet();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (var command = GetSqlCommand())
                {
                    command.Connection = connection;

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(results);
                    }
                }
            }

            base.Results = ToModels(results.Tables[0]);

            IsVisible = true;
        }
    }
}

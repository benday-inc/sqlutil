using System;
using System.Data;
using System.Linq;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchByNameResultRow : ITableName
    {
        public SearchByNameResultRow(DataRow fromValue)
        {
            Schema = fromValue["table_schema"].ToString();
            TableName = fromValue["table_name"].ToString();
            Column = fromValue["column_name"].ToString();
            DataType = fromValue["data_type"].ToString();
            FieldLength = fromValue["character_maximum_length"].ToString();
        }

        public string Schema { get; set; }
        public string TableName { get; set; }
        public string Column { get; set; }
        public string DataType { get; set; }
        public string FieldLength { get; set; }        
    }
}

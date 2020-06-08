using System;
using System.Data;
using System.Linq;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class TextQueryResultRow : ITableName
    {
        public TextQueryResultRow(DataRow fromValue, string query, int recordCount)
        {
            Schema = fromValue["table_schema"].ToString();
            TableName = fromValue["table_name"].ToString();
            Column = fromValue["column_name"].ToString();
            Records = recordCount;
            Query = query;
        }

        public string Schema { get; set; }
        public string TableName { get; set; }
        public string Column { get; set; }
        public int Records { get; set; }
        public string Query { get; set; }
    }
}

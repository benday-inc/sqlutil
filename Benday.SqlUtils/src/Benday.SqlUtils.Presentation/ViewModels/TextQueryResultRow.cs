using System;
using System.Data;
using System.Linq;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class TextQueryResultRow
    {
        public TextQueryResultRow(DataRow fromValue, string query, int recordCount)
        {
            Schema = fromValue["table_schema"].ToString();
            Table = fromValue["table_name"].ToString();
            Column = fromValue["column_name"].ToString();
            Records = recordCount;
            Query = query;
        }

        public string Schema { get; set; }
        public string Table { get; set; }
        public string Column { get; set; }
        public int Records { get; set; }
        public string Query { get; set; }
    }
}

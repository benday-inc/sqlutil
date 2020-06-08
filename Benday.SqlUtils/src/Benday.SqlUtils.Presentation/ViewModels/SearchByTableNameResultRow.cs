using System.Data;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchByTableNameResultRow : ITableName
    {
        public SearchByTableNameResultRow(DataRow fromValue)
        {
            Schema = fromValue["table_schema"].ToString();
            TableName = fromValue["table_name"].ToString();
        }

        public string Schema { get; set; }
        public string TableName { get; set; }
    }
}

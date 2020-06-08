using System.Data;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchByTableNameResultRow
    {
        public SearchByTableNameResultRow(DataRow fromValue)
        {
            Schema = fromValue["table_schema"].ToString();
            Name = fromValue["table_name"].ToString();
        }

        public string Schema { get; set; }
        public string Name { get; set; }
    }
}

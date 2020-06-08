using System.Data;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchByStoredProcedureSourceCodeResultRow
    {
        public SearchByStoredProcedureSourceCodeResultRow(DataRow fromValue)
        {
            Schema = fromValue["schema"].ToString();
            Name = fromValue["name"].ToString();
        }

        public string Schema { get; set; }
        public string Name { get; set; }
    }
}

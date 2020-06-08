using System;
using System.Data;
using System.Linq;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchByStoredProcedureNameResultRow : IStoredProcedureName
    {
        public SearchByStoredProcedureNameResultRow(DataRow fromValue)
        {
            Schema = fromValue["schema"].ToString();
            Name = fromValue["name"].ToString();
        }

        public string Schema { get; set; }
        public string Name { get; set; }
    }
}

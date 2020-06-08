using System;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchByStoredProcedureParameterNameResultRow
    {
        public SearchByStoredProcedureParameterNameResultRow(DataRow fromValue)
        {
            Schema = fromValue["schema"].ToString();
            Name = fromValue["name"].ToString();
            ParameterName = fromValue["parameter name"].ToString();
            DataType = fromValue["data type"].ToString();
            ValueLength = fromValue["length"].ToString();
            ParameterMode = fromValue["parameter mode"].ToString();
        }

        public string Schema { get; set; }
        public string Name { get; set; }
        [DisplayName("Parameter Name")]
        public string ParameterName { get; set; }
        public string DataType { get; set; }
        public string ValueLength { get; set; }
        public string ParameterMode { get; set; }
    }
}

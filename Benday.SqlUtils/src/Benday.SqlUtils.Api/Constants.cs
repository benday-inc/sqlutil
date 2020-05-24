using System;
using System.Linq;

namespace Benday.SqlUtils.Core
{
    public static class Constants
    {
        public const string SearchTypeTableName = "Table Name";
        public const string SearchTypeColumnName = "Column Name";
        public const string SearchTypeFindTextInTableColumn = "Find Text In Any Table Column";
        public const string SearchTypeStoredProcedureName = "Stored Procedure Name";
        public const string SearchTypeStoredProcedureParameterName = "Stored Procedure Parameter Name";
        public const string SearchTypeStoredProcedureSourceCode = "Stored Procedure Source Code";

        public const string SearchStringMethodContains = "Contains";
        public const string SearchStringMethodExact = "Exact";
        public const string SearchStringMethodStartsWith = "Starts With";
        public const string SearchStringMethodEndsWith = "Ends With";
    }
}

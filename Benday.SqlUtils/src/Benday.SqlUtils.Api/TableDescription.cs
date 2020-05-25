using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Benday.SqlUtils.Api
{
    public class TableDescription
    {
        public TableDescription()
        {
            Columns = new List<ColumnDescription>();
        }

        public TableDescription(DataTable fromValue) : this()
        {
            foreach (DataRow row in fromValue.Rows)
            {
                Columns.Add(new ColumnDescription(row));
            }
        }

        public List<ColumnDescription> Columns { get; }
    }
}

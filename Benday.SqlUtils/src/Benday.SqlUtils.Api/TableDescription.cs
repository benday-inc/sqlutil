using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Reflection.PortableExecutable;

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

            PopulateTableName();
        }

        public List<ColumnDescription> Columns { get; }
        public string GetIdentityColumnName()
        {
            var match = (from temp in Columns
                         where temp.IsIdentity == true
                         select temp).FirstOrDefault();

            if (match == null)
            {
                return null;
            }
            else
            {
                return match.ColumnName;
            }
        }

        public string PrimaryKeyColumnName
        {
            get; set;
        }

        public string TableName
        {
            get; set;
        }

        private void PopulateTableName()
        {
            var firstRow = Columns.FirstOrDefault();

            if (firstRow != null)
            {
                TableName = firstRow.TableName;
            }
        }
    }
}

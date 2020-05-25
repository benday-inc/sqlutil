using Benday.SqlUtils.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace Benday.SqlUtils.UnitTests
{
    [TestClass]
    public class ColumnDescriptionFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private ColumnDescription _SystemUnderTest;

        private ColumnDescription SystemUnderTest
        {
            get
            {
         
                return _SystemUnderTest;
            }
        }

        private void AddStringColumn(DataTable table, string columnName)
        {
            var column = new DataColumn(columnName, typeof(string));

            table.Columns.Add(column);
        }

        private void AddBooleanColumn(DataTable table, string columnName)
        {
            var column = new DataColumn(columnName, typeof(string));

            table.Columns.Add(column);
        }

        private void AddInt32Column(DataTable table, string columnName)
        {
            var column = new DataColumn(columnName, typeof(int));

            table.Columns.Add(column);
        }

        private void AddDescriptionRow(DataTable table, 
            string tableName, 
            string columnName, 
            bool isNullable, 
            string dataType, 
            int fieldLength, 
            int position, 
            bool isIdentity)
        {
            var row = table.NewRow();

            row["TABLE_SCHEMA"] = "dbo";
            row["TABLE_NAME"] = tableName;
            row["COLUMN_NAME"] = columnName;
            row["IsNullable"] = isNullable;
            row["DATA_TYPE"] = dataType;

            if (fieldLength != 0)
            {
                row["CHARACTER_MAXIMUM_LENGTH"] = fieldLength;
            }
            else
            {
                row["CHARACTER_MAXIMUM_LENGTH"] = DBNull.Value;
            }

            row["ORDINAL_POSITION"] = position;
            row["IsIdentity"] = isIdentity;

            table.Rows.Add(row);
        }

        private DataTable GetDescriptionDataTable()
        {
            var table = new DataTable();

            AddStringColumn(table, "TABLE_SCHEMA");
            AddStringColumn(table, "TABLE_NAME");
            AddStringColumn(table, "COLUMN_NAME");
            AddBooleanColumn(table, "IsNullable");
            AddStringColumn(table, "DATA_TYPE");
            AddInt32Column(table, "CHARACTER_MAXIMUM_LENGTH");
            AddInt32Column(table, "ORDINAL_POSITION");
            AddBooleanColumn(table, "IsIdentity");

            string tableName = "recipe";

            AddDescriptionRow(
                table, tableName, "Id", false, "int", 0, 1, true);
            AddDescriptionRow(
                table, tableName, "Name", false, "nvarchar", -1, 2, false);
            AddDescriptionRow(
                table, tableName, "Description", false, "nvarchar", -1, 3, false);
            AddDescriptionRow(
                table, tableName, "Source", false, "nvarchar", -1, 4, false);
            AddDescriptionRow(
                table, tableName, "PhotoUrl", false, "nvarchar", -1, 5, false);
            AddDescriptionRow(
                table, tableName, "EntryDate", false, "datetime2", -1, 6, false);
            AddDescriptionRow(
                table, tableName, "Status", false, "nvarchar", -1, 7, false);

            AddDescriptionRow(
                table, tableName, "CreatedBy", false, "nvarchar", -1, 8, false);
            AddDescriptionRow(
               table, tableName, "CreatedDate", false, "datetime2", -1, 9, false);

            AddDescriptionRow(
                table, tableName, "LastModifiedBy", false, "nvarchar", -1, 10, false);
            AddDescriptionRow(
               table, tableName, "LastModifiedDate", false, "datetime2", -1, 11, false);

            AddDescriptionRow(
               table, tableName, "Timestamp", true, "timestamp", -1, 12, false);

            return table;
        }

        [TestMethod]
        public void InitializeFromDataRow_IdentityColumn()
        {
            // arrange
            var table = GetDescriptionDataTable();

            var index = 0;
            var fromValue = table.Rows[index];

            // act
            _SystemUnderTest = new ColumnDescription(fromValue);

            // assert
            AssertColumn(SystemUnderTest, "dbo", "recipe", "Id", "int", false, true);
        }

        [TestMethod]
        public void InitializeFromDataRow_StringColumn()
        {
            // arrange
            var table = GetDescriptionDataTable();

            var index = 1;
            var fromValue = table.Rows[index];

            // act
            _SystemUnderTest = new ColumnDescription(fromValue);

            // assert
            AssertColumn(SystemUnderTest, "dbo", "recipe", "Name", "nvarchar", false, false);
        }


        private void AssertColumn(
            ColumnDescription actual, 
            string expectedSchema, string expectedTableName, string expectedColumnName, string expectedDataType, bool expectedIsNullable, bool expectedIsIdentity)
        {
            Assert.AreEqual<string>(expectedSchema, actual.Schema, "Schema");
            Assert.AreEqual<string>(expectedTableName, actual.TableName, "TableName");
            Assert.AreEqual<string>(expectedColumnName, actual.ColumnName, "ColumnName");
            Assert.AreEqual<string>(expectedDataType, actual.DataType, "DataType");
            Assert.AreEqual<bool>(expectedIsNullable, actual.IsNullable, "IsNullable");
            Assert.AreEqual<bool>(expectedIsIdentity, actual.IsIdentity, "IsIdentity");
        }
    }
}

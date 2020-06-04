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

        [TestMethod]
        public void InitializeFromDataRow_IdentityColumn()
        {
            // arrange
            var table = UnitTestUtility.GetDescriptionDataTable();

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
            var table = UnitTestUtility.GetDescriptionDataTable();

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

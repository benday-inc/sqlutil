using Benday.SqlUtils.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benday.SqlUtils.UnitTests
{
    [TestClass]
    public class TableDescriptionFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private TableDescription _SystemUnderTest;

        private TableDescription SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new TableDescription();
                }

                return _SystemUnderTest;
            }
        }

        [TestMethod]
        public void InitializeFromDataTable()
        {
            // arrange
            var table = UnitTestUtility.GetDescriptionDataTable();
            var rowCount = table.Rows.Count;

            // act
            _SystemUnderTest = new TableDescription(table);

            // assert

            Assert.AreNotEqual<int>(0, rowCount, "Row count should not be zero.");
            Assert.AreEqual<int>(rowCount, SystemUnderTest.Columns.Count, "Column count");

            var expectedColumns = new string[] { 
                "Id",
                "Name",
                "Description",
                "Source",
                "PhotoUrl",
                "EntryDate",
                "Status",
                "CreatedBy",
                "CreatedDate",
                "LastModifiedBy",
                "LastModifiedDate",
                "Timestamp" };

            AssertColumns(expectedColumns, SystemUnderTest);
        }

        private void AssertColumns(string[] expectedColumns, TableDescription actual)
        {
            foreach (var expectedColumnName in expectedColumns)
            {
                var exists = actual.Columns.Exists(x => x.ColumnName == expectedColumnName);

                Assert.IsTrue(exists, $"Column '{expectedColumnName}' did not exist.");
            }
        }
    }
}

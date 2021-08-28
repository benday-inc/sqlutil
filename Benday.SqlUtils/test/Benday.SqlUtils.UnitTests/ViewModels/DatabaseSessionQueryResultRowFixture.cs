using Benday.SqlUtils.Api;
using Benday.SqlUtils.Presentation.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace Benday.SqlUtils.UnitTests.ViewModels
{
    [TestClass]
    public class DatabaseSessionQueryResultRowFixture
    {
        [TestInitialize]
        public void OnTestInitialize() { _SystemUnderTest = null; }

        private DatabaseSessionQueryResultRow _SystemUnderTest;

        private DatabaseSessionQueryResultRow SystemUnderTest
        {
            get
            {
                Assert.IsNotNull(_SystemUnderTest, "SUT not initialized");

                return _SystemUnderTest;
            }
        }

        private const int ExpectedSessionId = 1234;
        private const string ExpectedStatus = "Status Value";
        private const string ExpectedLogin = "Login Value";
        private const string ExpectedHostName = "HostName Value";
        private const string ExpectedBlockedBy = "Blocked by Value";
        private const string ExpectedDatabaseName = "Database name Value";
        private const string ExpectedCommand = "Command Value";
        private const string ExpectedProgramName = "Program Name Value";

        private DataTable GetDatabaseSessionQueryResultTable()
        {
            DataTable table = new DataTable();

            AddColumn<int>(table, Constants.ColumnName_SessionId);
            AddColumn<string>(table, Constants.ColumnName_Status);
            AddColumn<string>(table, Constants.ColumnName_Login);
            AddColumn<string>(table, Constants.ColumnName_HostName);
            AddColumn<string>(table, Constants.ColumnName_BlockedBy);
            AddColumn<string>(table, Constants.ColumnName_DatabaseName);
            AddColumn<string>(table, Constants.ColumnName_Command);
            AddColumn<string>(table, Constants.ColumnName_ProgramName);
            return table;
        }

        private void InitalizeAllFieldsToNull()
        {
            DataTable table = GetDatabaseSessionQueryResultTable();

            DataRow row = table.NewRow();

            foreach (DataColumn col in table.Columns)
            {
                row[col] = DBNull.Value;
            }

            _SystemUnderTest = new DatabaseSessionQueryResultRow(row);
        }

        private void AddColumn<T>(DataTable table, string columnName)
        {
            DataColumn column = new DataColumn(columnName, typeof(T));

            column.AllowDBNull = true;

            table.Columns.Add(column);
        }

        private void InitalizeAllStringFieldsToWhitespace()
        {
            DataTable table = GetDatabaseSessionQueryResultTable();

            DataRow row = table.NewRow();

            var valueString = "       ";

            foreach (DataColumn col in table.Columns)
            {
                if (col.DataType == typeof(int))
                {
                    row[col] = DBNull.Value;
                }
                else
                {
                    row[col] = valueString;
                }
            }

            _SystemUnderTest = new DatabaseSessionQueryResultRow(row);
        }

        private void InitalizeAllStringFieldsToDash()
        {
            DataTable table = GetDatabaseSessionQueryResultTable();

            DataRow row = table.NewRow();

            var valueString = "  .";

            foreach (DataColumn col in table.Columns)
            {
                if (col.DataType == typeof(int))
                {
                    row[col] = DBNull.Value;
                }
                else
                {
                    row[col] = valueString;
                }
            }

            _SystemUnderTest = new DatabaseSessionQueryResultRow(row);
        }

        private void InitalizeAllFieldsToValues()
        {
            DataTable table = GetDatabaseSessionQueryResultTable();

            DataRow row = table.NewRow();

            row[Constants.ColumnName_SessionId] = ExpectedSessionId;
            row[Constants.ColumnName_Status] = ExpectedStatus;
            row[Constants.ColumnName_Login] = ExpectedLogin;
            row[Constants.ColumnName_HostName] = ExpectedHostName;
            row[Constants.ColumnName_BlockedBy] = ExpectedBlockedBy;
            row[Constants.ColumnName_DatabaseName] = ExpectedDatabaseName;
            row[Constants.ColumnName_Command] = ExpectedCommand;
            row[Constants.ColumnName_ProgramName] = ExpectedProgramName;

            _SystemUnderTest = new DatabaseSessionQueryResultRow(row);
        }

        [TestMethod]
        public void Ctor_SessionId_NullBecomesNegativeOne()
        {
            // arrange


            // act
            InitalizeAllFieldsToNull();

            // assert
            Assert.AreEqual<int>(-1, SystemUnderTest.SessionId, "SessionId was wrong");
        }

        [TestMethod]
        public void Ctor_SessionId_IsPopulated()
        {
            // arrange


            // act
            InitalizeAllFieldsToValues();

            // assert
            Assert.AreEqual<int>(ExpectedSessionId, SystemUnderTest.SessionId, "SessionId was wrong");
        }

        [TestMethod]
        public void Ctor_Hostname_NullBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllFieldsToNull();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.HostName, "Hostname was wrong");
        }

        [TestMethod]
        public void Ctor_Hostname_WhitespaceBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllStringFieldsToWhitespace();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.HostName, "Hostname was wrong");
        }

        [TestMethod]
        public void Ctor_Hostname_DashBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllStringFieldsToDash();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.HostName, "Hostname was wrong");
        }

        [TestMethod]
        public void Ctor_Hostname_ExpectedValue()
        {
            // arrange


            // act
            InitalizeAllFieldsToValues();

            // assert
            Assert.AreEqual<string>(ExpectedHostName, SystemUnderTest.HostName, "Hostname was wrong");
        }

        [TestMethod]
        public void Ctor_Status_NullBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllFieldsToNull();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.Status, "Status was wrong");
        }

        [TestMethod]
        public void Ctor_Status_WhitespaceBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllStringFieldsToWhitespace();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.Status, "Status was wrong");
        }

        [TestMethod]
        public void Ctor_Status_DashBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllStringFieldsToDash();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.Status, "Status was wrong");
        }

        [TestMethod]
        public void Ctor_Status_ExpectedValue()
        {
            // arrange


            // act
            InitalizeAllFieldsToValues();

            // assert
            Assert.AreEqual<string>(ExpectedStatus, SystemUnderTest.Status, "Status was wrong");
        }

        [TestMethod]
        public void Ctor_Login_NullBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllFieldsToNull();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.Login, "Login was wrong");
        }

        [TestMethod]
        public void Ctor_Login_WhitespaceBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllStringFieldsToWhitespace();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.Login, "Login was wrong");
        }

        [TestMethod]
        public void Ctor_Login_DashBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllStringFieldsToDash();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.Login, "Login was wrong");
        }

        [TestMethod]
        public void Ctor_Login_ExpectedValue()
        {
            // arrange


            // act
            InitalizeAllFieldsToValues();

            // assert
            Assert.AreEqual<string>(ExpectedLogin, SystemUnderTest.Login, "Login was wrong");
        }

        [TestMethod]
        public void Ctor_BlockedBy_NullBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllFieldsToNull();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.BlockedBy, "BlockedBy was wrong");
        }

        [TestMethod]
        public void Ctor_BlockedBy_WhitespaceBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllStringFieldsToWhitespace();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.BlockedBy, "BlockedBy was wrong");
        }

        [TestMethod]
        public void Ctor_BlockedBy_DashBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllStringFieldsToDash();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.BlockedBy, "BlockedBy was wrong");
        }

        [TestMethod]
        public void Ctor_BlockedBy_ExpectedValue()
        {
            // arrange


            // act
            InitalizeAllFieldsToValues();

            // assert
            Assert.AreEqual<string>(ExpectedBlockedBy, SystemUnderTest.BlockedBy, "BlockedBy was wrong");
        }

        [TestMethod]
        public void Ctor_DatabaseName_NullBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllFieldsToNull();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.DatabaseName, "DatabaseName was wrong");
        }

        [TestMethod]
        public void Ctor_DatabaseName_WhitespaceBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllStringFieldsToWhitespace();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.DatabaseName, "DatabaseName was wrong");
        }

        [TestMethod]
        public void Ctor_DatabaseName_DashBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllStringFieldsToDash();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.DatabaseName, "DatabaseName was wrong");
        }

        [TestMethod]
        public void Ctor_DatabaseName_ExpectedValue()
        {
            // arrange


            // act
            InitalizeAllFieldsToValues();

            // assert
            Assert.AreEqual<string>(ExpectedDatabaseName, SystemUnderTest.DatabaseName, "DatabaseName was wrong");
        }

        [TestMethod]
        public void Ctor_Command_NullBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllFieldsToNull();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.Command, "Command was wrong");
        }

        [TestMethod]
        public void Ctor_Command_WhitespaceBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllStringFieldsToWhitespace();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.Command, "Command was wrong");
        }

        [TestMethod]
        public void Ctor_Command_DashBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllStringFieldsToDash();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.Command, "Command was wrong");
        }

        [TestMethod]
        public void Ctor_Command_ExpectedValue()
        {
            // arrange


            // act
            InitalizeAllFieldsToValues();

            // assert
            Assert.AreEqual<string>(ExpectedCommand, SystemUnderTest.Command, "Command was wrong");
        }

        [TestMethod]
        public void Ctor_ProgramName_NullBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllFieldsToNull();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.ProgramName, "ProgramName was wrong");
        }

        [TestMethod]
        public void Ctor_ProgramName_WhitespaceBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllStringFieldsToWhitespace();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.ProgramName, "ProgramName was wrong");
        }

        [TestMethod]
        public void Ctor_ProgramName_DashBecomesEmptyString()
        {
            // arrange


            // act
            InitalizeAllStringFieldsToDash();

            // assert
            Assert.AreEqual<string>(string.Empty, SystemUnderTest.ProgramName, "ProgramName was wrong");
        }

        [TestMethod]
        public void Ctor_ProgramName_ExpectedValue()
        {
            // arrange


            // act
            InitalizeAllFieldsToValues();

            // assert
            Assert.AreEqual<string>(ExpectedProgramName, SystemUnderTest.ProgramName, "ProgramName was wrong");
        }
    }
}

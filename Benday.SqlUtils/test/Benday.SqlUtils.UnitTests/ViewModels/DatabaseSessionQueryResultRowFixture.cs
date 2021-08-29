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

        private void InitalizeAllFieldsToNull()
        {
            var table = DatabaseSessionTestUtility.InitalizeAllFieldsToNull();

            var row = table.Rows[0];

            _SystemUnderTest = new DatabaseSessionQueryResultRow(row);
        }

        private void InitalizeAllStringFieldsToWhitespace()
        {
            var table = DatabaseSessionTestUtility.InitalizeAllStringFieldsToWhitespace();

            var row = table.Rows[0];

            _SystemUnderTest = new DatabaseSessionQueryResultRow(row);
        }

        private void InitalizeAllStringFieldsToDash()
        {
            var table = DatabaseSessionTestUtility.InitalizeAllStringFieldsToDash();

            var row = table.Rows[0];

            _SystemUnderTest = new DatabaseSessionQueryResultRow(row);
        }

        private void InitalizeAllFieldsToValues()
        {
            var table = DatabaseSessionTestUtility.InitalizeAllFieldsToValues();
            
            var row = table.Rows[0];

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
            Assert.AreEqual<int>(DatabaseSessionTestUtility.ExpectedSessionId, SystemUnderTest.SessionId, "SessionId was wrong");
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
            Assert.AreEqual<string>(DatabaseSessionTestUtility.ExpectedHostName, SystemUnderTest.HostName, "Hostname was wrong");
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
            Assert.AreEqual<string>(DatabaseSessionTestUtility.ExpectedStatus, SystemUnderTest.Status, "Status was wrong");
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
            Assert.AreEqual<string>(DatabaseSessionTestUtility.ExpectedLogin, SystemUnderTest.Login, "Login was wrong");
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
            Assert.AreEqual<string>(DatabaseSessionTestUtility.ExpectedBlockedBy, SystemUnderTest.BlockedBy, "BlockedBy was wrong");
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
            Assert.AreEqual<string>(DatabaseSessionTestUtility.ExpectedDatabaseName, SystemUnderTest.DatabaseName, "DatabaseName was wrong");
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
            Assert.AreEqual<string>(DatabaseSessionTestUtility.ExpectedCommand, SystemUnderTest.Command, "Command was wrong");
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
            Assert.AreEqual<string>(DatabaseSessionTestUtility.ExpectedProgramName, SystemUnderTest.ProgramName, "ProgramName was wrong");
        }
    }
}

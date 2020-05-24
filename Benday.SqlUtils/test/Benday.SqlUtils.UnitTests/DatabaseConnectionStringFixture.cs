using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benday.SqlServerUtilities.Core;

namespace Benday.SqlServerUtilities.UnitTests
{
    [TestClass]
    public class DatabaseConnectionStringFixture
    {
        private string _ValidConnectionStringUseIntegratedSecurity = "Server=the_server; Database=the_database_name; Integrated Security=True;";
        private string _ValidConnectionStringUserNamePassword = "Server=the_server; Database=the_database_name; User Id=the_username; Password=the_password;";
        private string _ExpectedUsername = "the_username";
        private string _ExpectedPassword = "the_password";
        private string _ExpectedServer = "the_server";
        private string _ExpectedDatabase = "the_database_name";

        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private DatabaseConnectionString _SystemUnderTest;
        public DatabaseConnectionString SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new DatabaseConnectionString();
                }

                return _SystemUnderTest;
            }
        }

        [TestMethod]
        public void LoadConnectionString_UseIntegratedSecurity()
        {
            var expectedConnectionString = _ValidConnectionStringUseIntegratedSecurity;

            SystemUnderTest.Load(expectedConnectionString);

            Assert.AreEqual<string>(_ExpectedDatabase, SystemUnderTest.Database, "Database was wrong.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.Password, "Password was wrong.");
            Assert.AreEqual<string>(_ExpectedServer, SystemUnderTest.Server, "Server was wrong.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.Username, "Username was wrong.");
            Assert.AreEqual<bool>(true, SystemUnderTest.UseIntegratedSecurity, "UseIntegratedSecurity was wrong.");
            Assert.AreEqual<string>(expectedConnectionString, SystemUnderTest.ConnectionString, "ConnectionString was wrong.");
        }

        [TestMethod]
        public void LoadConnectionString_UsernamePassword()
        {
            var expectedConnectionString = _ValidConnectionStringUserNamePassword;

            SystemUnderTest.Load(expectedConnectionString);

            Assert.AreEqual<string>(_ExpectedDatabase, SystemUnderTest.Database, "Database was wrong.");
            Assert.AreEqual<string>(_ExpectedPassword, SystemUnderTest.Password, "Password was wrong.");
            Assert.AreEqual<string>(_ExpectedServer, SystemUnderTest.Server, "Server was wrong.");
            Assert.AreEqual<string>(_ExpectedUsername, SystemUnderTest.Username, "Username was wrong.");
            Assert.AreEqual<bool>(false, SystemUnderTest.UseIntegratedSecurity, "UseIntegratedSecurity was wrong.");
            Assert.AreEqual<string>(expectedConnectionString, SystemUnderTest.ConnectionString, "ConnectionString was wrong.");
        }
    }
}

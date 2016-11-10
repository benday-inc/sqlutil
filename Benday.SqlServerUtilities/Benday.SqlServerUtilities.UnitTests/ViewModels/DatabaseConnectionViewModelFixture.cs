using Benday.SqlServerUtilities.Core;
using Benday.SqlServerUtilities.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.SqlServerUtilities.UnitTests.ViewModels
{
    [TestClass]
    public class DatabaseConnectionViewModelFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
            _DatabaseConnectionStringInstance = null;
        }

        private DatabaseConnectionViewModel _SystemUnderTest;
        public DatabaseConnectionViewModel SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new DatabaseConnectionViewModel();
                }

                return _SystemUnderTest;
            }
        }

        [TestMethod]
        public void FieldsAreInitializedForNewInstance()
        {
            UnitTestUtility.AssertIsNotNullOrWhitespace(SystemUnderTest.Id, "Id");

            Assert.AreEqual<string>(String.Empty, SystemUnderTest.Database, "Database should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.Name, "Name should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.Password, "Password should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.Server, "Server should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.Username, "Username should be empty.");
            Assert.IsTrue(SystemUnderTest.UseIntegratedSecurity, "UseTrustedConnection");
        }

        private DatabaseConnectionString _DatabaseConnectionStringInstance;
        public DatabaseConnectionString DatabaseConnectionStringInstance
        {
            get
            {
                if (_DatabaseConnectionStringInstance == null)
                {
                    _DatabaseConnectionStringInstance = new DatabaseConnectionString();
                }

                return _DatabaseConnectionStringInstance;
            }
        }

        private string _DatabaseConnectionId;
        public string DatabaseConnectionId
        {
            get
            {
                if (_DatabaseConnectionId == null)
                {
                    _DatabaseConnectionId = Guid.NewGuid().ToString();
                }

                return _DatabaseConnectionId;
            }
        }

        private string _OriginalName = "original name";
        public string OriginalName
        {
            get
            {
                return _OriginalName;
            }
        }

        private void InitializeUseIntegratedSecurity()
        {
            DatabaseConnectionStringInstance.Server = "the_server";
            DatabaseConnectionStringInstance.Database = "the_database";
            DatabaseConnectionStringInstance.UseIntegratedSecurity = true;

            SystemUnderTest.Initialize(
                DatabaseConnectionId, 
                OriginalName, 
                DatabaseConnectionStringInstance);
        }

        private void InitializeUseUsernameAndPassword()
        {
            DatabaseConnectionStringInstance.Server = "the_server";
            DatabaseConnectionStringInstance.Database = "the_database";
            DatabaseConnectionStringInstance.UseIntegratedSecurity = false;
            DatabaseConnectionStringInstance.Username = "the_username";
            DatabaseConnectionStringInstance.Password = "the_password";

            SystemUnderTest.Initialize(
                DatabaseConnectionId,
                OriginalName,
                DatabaseConnectionStringInstance);
        }

        [TestMethod]
        public void InitializeLoadsFieldsIntegratedSecurity()
        {
            InitializeUseIntegratedSecurity();

            Assert.AreEqual<string>(DatabaseConnectionId, SystemUnderTest.Id, "Id");
            Assert.AreEqual<string>(OriginalName, SystemUnderTest.Name, "Id");
            Assert.IsTrue(SystemUnderTest.UseIntegratedSecurity, "UseTrustedConnection");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.Username, "Username should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.Password, "Password should be empty.");
            Assert.AreEqual<string>("the_server", SystemUnderTest.Server, "Server");
            Assert.AreEqual<string>("the_database", SystemUnderTest.Database, "Database");
        }

        [TestMethod]
        public void InitializeLoadsFieldsUsernamePassword()
        {
            InitializeUseUsernameAndPassword();

            Assert.AreEqual<string>(DatabaseConnectionId, SystemUnderTest.Id, "Id");
            Assert.AreEqual<string>(OriginalName, SystemUnderTest.Name, "Id");
            Assert.IsFalse(SystemUnderTest.UseIntegratedSecurity, "UseTrustedConnection");
            Assert.AreEqual<string>("the_username", SystemUnderTest.Username, "Username should be empty.");
            Assert.AreEqual<string>("the_password", SystemUnderTest.Password, "Password should be empty.");
            Assert.AreEqual<string>("the_server", SystemUnderTest.Server, "Server");
            Assert.AreEqual<string>("the_database", SystemUnderTest.Database, "Database");
        }



    }
}

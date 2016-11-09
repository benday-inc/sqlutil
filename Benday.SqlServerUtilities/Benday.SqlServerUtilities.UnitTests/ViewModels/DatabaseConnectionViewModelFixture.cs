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
            Assert.IsTrue(SystemUnderTest.UseTrustedConnection, "UseTrustedConnection");
        }


    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benday.SqlServerUtilities.Core.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Benday.SqlServerUtilities.UnitTests.ViewModels
{
    [TestClass]
    public class DatabaseConnectionsViewModelFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
            _DatabaseConnectionStringRepositoryInstance = null;
        }

        private DatabaseConnectionsViewModel _SystemUnderTest;
        public DatabaseConnectionsViewModel SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = 
                        new DatabaseConnectionsViewModel(
                            DatabaseConnectionStringRepositoryInstance);
                }

                return _SystemUnderTest;
            }
        }

        private MockDatabaseConnectionStringRepository _DatabaseConnectionStringRepositoryInstance;
        public MockDatabaseConnectionStringRepository DatabaseConnectionStringRepositoryInstance
        {
            get
            {
                if (_DatabaseConnectionStringRepositoryInstance == null)
                {
                    _DatabaseConnectionStringRepositoryInstance = 
                        new MockDatabaseConnectionStringRepository();
                }
                return _DatabaseConnectionStringRepositoryInstance;
            }
        }
        

        [TestMethod]
        public void WhenInitializedConnectionsPropertyIsNotNull()
        {
            Assert.IsNotNull(SystemUnderTest.Connections);
        }

        [TestMethod]
        public void WhenInitializedConnectionsPropertyIsEmpty()
        {
            Assert.AreEqual<int>(0, SystemUnderTest.Connections.Count, "Count was wrong.");
        }

        [TestMethod]
        public void AddCommandAddsANewConnectionAndSelectsIt()
        {
            SystemUnderTest.AddConnectionCommand.Execute(null);

            Assert.AreEqual<int>(1, SystemUnderTest.Connections.Count, "Count was wrong.");
            Assert.IsTrue(SystemUnderTest.Connections.Items[0].IsSelected, "New item should be selected.");
            Assert.AreEqual<string>("(new connection)", SystemUnderTest.Connections.Items[0].Name, "Name was wrong.");
        }

        [TestMethod]
        public void CallingAddCommandTwiceAddsTwoConnectionsAndSelectsTheLastOne()
        {
            SystemUnderTest.AddConnectionCommand.Execute(null);
            SystemUnderTest.AddConnectionCommand.Execute(null);

            Assert.AreEqual<int>(2, SystemUnderTest.Connections.Count, "Count was wrong.");
            Assert.IsFalse(SystemUnderTest.Connections.Items[0].IsSelected, 
                "First item should not be selected.");
            Assert.IsTrue(SystemUnderTest.Connections.Items[1].IsSelected, 
                "New item should be selected.");
        }

        [TestMethod]
        public void DeleteCommandRemoveSelectedConnection()
        {
            SystemUnderTest.AddConnectionCommand.Execute(null);
            SystemUnderTest.AddConnectionCommand.Execute(null);

            var removeThis = SystemUnderTest.Connections.Items[0];

            removeThis.IsSelected = true;

            SystemUnderTest.DeleteConnectionCommand.Execute(null);

            Assert.IsFalse(SystemUnderTest.Connections.Items.Contains(removeThis), 
                "Removed item should not be in collection.");
        }

        [TestMethod]
        public void WhenSaveIsCalledOnANewConnectionThenItIsWrittenToRepository()
        {
            SystemUnderTest.AddConnectionCommand.Execute(null);

            var actual = SystemUnderTest.Connections.SelectedItem;

            actual.SaveCommand.Execute(null);

            Assert.AreEqual<int>(1, DatabaseConnectionStringRepositoryInstance.Items.Count);
        }

        [TestMethod]
        public void DeleteCommandCallsRepositoryForDelete()
        {
            SystemUnderTest.AddConnectionCommand.Execute(null);

            var actual = SystemUnderTest.Connections.SelectedItem;

            actual.SaveCommand.Execute(null);

            SystemUnderTest.DeleteConnectionCommand.Execute(null);

            Assert.AreEqual<int>(0, DatabaseConnectionStringRepositoryInstance.Items.Count);
        }

        private string _ValidConnectionStringUseIntegratedSecurity = "Server=the_server; Database=the_database_name; Integrated Security=True;";
        private string _ValidConnectionStringUserNamePassword = "Server=the_server; Database=the_database_name; User Id=the_username; Password=the_password;";

        [TestMethod]
        public void WhenInitializedFromRepositoryWithConnectionsThenConnectionsArePopulated()
        {
            DatabaseConnectionStringRepositoryInstance.Add(
                Guid.NewGuid().ToString(), "connection 1",
                _ValidConnectionStringUseIntegratedSecurity);
            DatabaseConnectionStringRepositoryInstance.Add(
                Guid.NewGuid().ToString(), "connection 2",
                _ValidConnectionStringUserNamePassword);

            // reset the system under test to make it 
            // pick up the new populated repository
            _SystemUnderTest = null;

            Assert.AreEqual<int>(2, SystemUnderTest.Connections.Count, "Connection count was wrong.");
        }
    }
}

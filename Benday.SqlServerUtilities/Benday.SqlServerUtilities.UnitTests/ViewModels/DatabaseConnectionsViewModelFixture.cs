using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Benday.SqlServerUtilities.Core.ViewModels;

namespace Benday.SqlServerUtilities.UnitTests.ViewModels
{
    [TestClass]
    public class DatabaseConnectionsViewModelFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private DatabaseConnectionsViewModel _SystemUnderTest;
        public DatabaseConnectionsViewModel SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new DatabaseConnectionsViewModel();
                }

                return _SystemUnderTest;
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
        public void ChangingSelectedConnectionIsBlockedWhenConnectionHasChanges()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void WhenConnectionThrowsSaveRequestConnectionsAreWrittenToConfig()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void WhenInitializedFromConfigConnectionsArePopulated()
        {
            Assert.Inconclusive();
        }




    }
}

using Benday.Presentation.UnitTests;
using Benday.SqlServerUtilities.Core;
using Benday.SqlServerUtilities.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Benday.Presentation;

namespace Benday.SqlServerUtilities.UnitTests.ViewModels
{
    [TestClass]
    public class DatabaseConnectionViewModelFixture : ViewModelFixtureBase
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

            AssertFieldIsInitializedToEmptyAndVisibleAndValid(SystemUnderTest.Database, "Database");
            AssertFieldIsInitializedToEmptyAndVisibleAndValid(SystemUnderTest.Name, "Name");
            AssertFieldIsInitializedToEmptyAndVisibleAndValid(SystemUnderTest.Password, "Password");
            AssertFieldIsInitializedToEmptyAndVisibleAndValid(SystemUnderTest.Server, "Server");
            AssertFieldIsInitializedToEmptyAndVisibleAndValid(SystemUnderTest.Username, "Username");
            AssertFieldIsInitializedToTrueAndVisibleAndValid(SystemUnderTest.UseIntegratedSecurity, "UseIntegratedSecurity");
        }
        private void AssertFieldIsInitializedToEmptyAndVisibleAndValid(
            ViewModelField<string> actualField, string fieldName)
        {
            Assert.IsNotNull(actualField, "Field '{0}' was null.",
                fieldName);

            Assert.AreEqual<string>(String.Empty,
                actualField.Value,
                "Value was wrong on field '{0}'.",
                fieldName);

            Assert.AreEqual<bool>(true,
                actualField.IsVisible,
                "Visibility was wrong on field '{0}'.",
                fieldName);

            Assert.IsTrue(actualField.IsValid, "IsValid was wrong on field '{0}'.",
                fieldName);
        }

        private void AssertFieldIsInitializedToTrueAndVisibleAndValid(
            ViewModelField<bool> actualField, string fieldName)
        {
            Assert.IsNotNull(actualField, "Field '{0}' was null.",
                fieldName);

            Assert.AreEqual<bool>(true,
                actualField.Value,
                "Value was wrong on field '{0}'.",
                fieldName);

            Assert.AreEqual<bool>(true,
                actualField.IsVisible,
                "Visibility was wrong on field '{0}'.",
                fieldName);

            Assert.IsTrue(actualField.IsValid, "IsValid was wrong on field '{0}'.",
                fieldName);
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
            Assert.AreEqual<string>(OriginalName, SystemUnderTest.Name.Value, "Name");
            Assert.IsTrue(SystemUnderTest.UseIntegratedSecurity.Value, "UseTrustedConnection");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.Username.Value, "Username should be empty.");
            Assert.AreEqual<string>(String.Empty, SystemUnderTest.Password.Value, "Password should be empty.");
            Assert.AreEqual<string>("the_server", SystemUnderTest.Server.Value, "Server");
            Assert.AreEqual<string>("the_database", SystemUnderTest.Database.Value, "Database");
        }

        [TestMethod]
        public void InitializeLoadsFieldsUsernamePassword()
        {
            InitializeUseUsernameAndPassword();

            Assert.AreEqual<string>(DatabaseConnectionId, SystemUnderTest.Id, "Id");
            Assert.AreEqual<string>(OriginalName, SystemUnderTest.Name.Value, "Id");
            Assert.IsFalse(SystemUnderTest.UseIntegratedSecurity.Value, "UseTrustedConnection");
            Assert.AreEqual<string>("the_username", SystemUnderTest.Username.Value, "Username should be empty.");
            Assert.AreEqual<string>("the_password", SystemUnderTest.Password.Value, "Password should be empty.");
            Assert.AreEqual<string>("the_server", SystemUnderTest.Server.Value, "Server");
            Assert.AreEqual<string>("the_database", SystemUnderTest.Database.Value, "Database");
        }

        [TestMethod]
        public void CancelCommandReloadsOriginalData()
        {
            InitializeUseUsernameAndPassword();

            SystemUnderTest.Name.Value = "asdf";
            SystemUnderTest.UseIntegratedSecurity.Value = true;
            SystemUnderTest.Username.Value = "asdf";
            SystemUnderTest.Password.Value = "asdf";
            SystemUnderTest.Server.Value = "asdf";
            SystemUnderTest.Database.Value = "asdf";

            SystemUnderTest.CancelCommand.Execute(null);

            Assert.AreEqual<string>(DatabaseConnectionId, SystemUnderTest.Id, "Id");
            Assert.AreEqual<string>(OriginalName, SystemUnderTest.Name.Value, "Name");
            Assert.IsFalse(SystemUnderTest.UseIntegratedSecurity.Value, "UseTrustedConnection");
            Assert.AreEqual<string>("the_username", SystemUnderTest.Username.Value, "Username should be empty.");
            Assert.AreEqual<string>("the_password", SystemUnderTest.Password.Value, "Password should be empty.");
            Assert.AreEqual<string>("the_server", SystemUnderTest.Server.Value, "Server");
            Assert.AreEqual<string>("the_database", SystemUnderTest.Database.Value, "Database");
        }

        [TestMethod]
        public void ConnectionStringIsPopulatedWhenInitialized()
        {
            InitializeUseUsernameAndPassword();

            var expected = _DatabaseConnectionStringInstance.ConnectionString;

            var actual = SystemUnderTest.ConnectionString;

            Assert.AreEqual<string>(expected, actual, "ConnectionString was wrong.");
        }

        [TestMethod]
        public void ConnectionStringRaisesNotifyPropertyChangedWhenUseIntegratedSecurityChanges()
        {
            var tester = new NotifyPropertyChangedTester(SystemUnderTest);

            SystemUnderTest.UseIntegratedSecurity.Value = !SystemUnderTest.UseIntegratedSecurity.Value;

            tester.AssertChange(DatabaseConnectionViewModel.ConnectionStringPropertyName);
        }

        [TestMethod]
        public void ConnectionStringRaisesNotifyPropertyChangedWhenUsernameChanges()
        {
            var tester = new NotifyPropertyChangedTester(SystemUnderTest);

            SystemUnderTest.Username.Value = "asdf";

            tester.AssertChange(DatabaseConnectionViewModel.ConnectionStringPropertyName);
        }

        [TestMethod]
        public void ConnectionStringRaisesNotifyPropertyChangedWhenPasswordChanges()
        {
            var tester = new NotifyPropertyChangedTester(SystemUnderTest);

            SystemUnderTest.Password.Value = "asdf";

            tester.AssertChange(DatabaseConnectionViewModel.ConnectionStringPropertyName);
        }

        [TestMethod]
        public void ConnectionStringRaisesNotifyPropertyChangedWhenServerChanges()
        {
            var tester = new NotifyPropertyChangedTester(SystemUnderTest);

            SystemUnderTest.Server.Value = "asdf";

            tester.AssertChange(DatabaseConnectionViewModel.ConnectionStringPropertyName);
        }

        [TestMethod]
        public void ConnectionStringRaisesNotifyPropertyChangedWhenDatabaseChanges()
        {
            var tester = new NotifyPropertyChangedTester(SystemUnderTest);

            SystemUnderTest.Database.Value = "asdf";

            tester.AssertChange(DatabaseConnectionViewModel.ConnectionStringPropertyName);
        }

        [TestMethod]
        public void SaveCommandThrowsOnSaveRequestedEvent()
        {
            bool gotOnSaveRequestedEventCall = false;
            object sender = null;

            SystemUnderTest.OnSaveRequested += (s, e) => {
                gotOnSaveRequestedEventCall = true;
                sender = s;
            };

            SystemUnderTest.SaveCommand.Execute(null);

            Assert.IsTrue(gotOnSaveRequestedEventCall, "Did not receive the call.");
            Assert.AreSame(SystemUnderTest, sender, "Sender on event was not SystemUnderTest."); 
        }
    }
}

using Benday.SqlUtils.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.SqlUtils.UnitTests
{
    [TestClass]
    public class DatabaseConnectionStringRepositoryFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
            _ConnectionStringFilePath = null;
        }

        private DatabaseConnectionStringRepository _SystemUnderTest;
        public DatabaseConnectionStringRepository SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = 
                        new DatabaseConnectionStringRepository(
                            ConnectionStringFilePath);
                }

                return _SystemUnderTest;
            }
        }

        private string _ConnectionStringFilePath;
        public string ConnectionStringFilePath
        {
            get
            {
                if (_ConnectionStringFilePath == null)
                {
                    var resultsDir = TestContext.TestResultsDirectory;

                    var path = Path.Combine(resultsDir, DateTime.Now.Ticks.ToString(),
                        "connections.xml");

                    _ConnectionStringFilePath = path;
                }

                return _ConnectionStringFilePath;
            }
        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void AddMultipleConnectionsAndSaveToDisk()
        {
            List<StoredDatabaseConnectionString> testValues = CreateTestData();

            var actualConnections = SystemUnderTest.GetAll();

            Assert.AreEqual<int>(3, actualConnections.Count, "Connection count was wrong.");

            AssertConnectionExistsAndAreEqual(testValues[0], actualConnections);
            AssertConnectionExistsAndAreEqual(testValues[1], actualConnections);
            AssertConnectionExistsAndAreEqual(testValues[2], actualConnections);
        }

        private List<StoredDatabaseConnectionString> CreateTestData()
        {
            var conn0 = new StoredDatabaseConnectionString("0", "conn0", "conn0 string");
            var conn1 = new StoredDatabaseConnectionString("1", "conn1", "conn1 string");
            var conn2 = new StoredDatabaseConnectionString("2", "conn2", "conn2 string");

            SystemUnderTest.Save(conn0);
            SystemUnderTest.Save(conn1);
            SystemUnderTest.Save(conn2);

            var testValues = new List<StoredDatabaseConnectionString>();

            testValues.Add(conn0);
            testValues.Add(conn1);
            testValues.Add(conn2);

            return testValues;
        }

        [TestMethod]
        public void UpdateExistingConnectionToDiskUpdatesValues()
        {
            var testData = CreateTestData();

            var expected = testData[1];

            expected.Name = "new name";
            expected.ConnectionString = "new conn string";

            SystemUnderTest.Save(expected);

            AssertConnectionExistsAndAreEqual(
                expected,
                SystemUnderTest.GetAll());
        }

        [TestMethod]
        public void UpdateExistingConnectionToDiskDoesNotAddDuplicate()
        {
            var testData = CreateTestData();

            var expected = testData[1];

            expected.Name = "new name";
            expected.ConnectionString = "new conn string";

            SystemUnderTest.Save(expected);

            Assert.AreEqual<int>(
                testData.Count, 
                SystemUnderTest.GetAll().Count, 
                "Should be the same count.");
        }

        [TestMethod]
        public void GetAllConnections()
        {
            Assert.AreEqual<int>(
                CreateTestData().Count, 
                SystemUnderTest.GetAll().Count, 
                "Wrong count.");
        }

        [TestMethod]
        public void WhenThereIsNoConfigFileThenGetAllConnections()
        {
            var actual = SystemUnderTest.GetAll();

            Assert.IsNotNull(actual);
            Assert.AreEqual<int>(0, actual.Count, "Connection count was wrong.");
        }

        [TestMethod]
        public void DeleteConnection()
        {
            var expected = CreateTestData()[1];

            SystemUnderTest.Delete(expected);

            var actualConnections = SystemUnderTest.GetAll();

            Assert.AreEqual<int>(2, actualConnections.Count, 
                "Wrong number of connections after delete.");

            var actual = (from temp in actualConnections
                          where temp.Id == expected.Id
                          select temp).FirstOrDefault();

            Assert.IsNull(actual, "Connection should not exist after delete.");
        }

        private void AssertConnectionExistsAndAreEqual(
            StoredDatabaseConnectionString expected,
            IList<IStoredDatabaseConnectionString> actualConnections
            )
        {
            if (expected == null)
                throw new ArgumentNullException(nameof(expected), $"{nameof(expected)} is null.");
            if (actualConnections == null || actualConnections.Count == 0)
                throw new ArgumentException($"{nameof(actualConnections)} is null or empty.", nameof(actualConnections));

            var actual = (
                from temp in actualConnections
                where temp.Id == expected.Id
                select temp).FirstOrDefault();

            Assert.IsNotNull(actual, "Could not locate a connection with id '{0}'.", expected.Id);

            Assert.AreEqual<string>(expected.Id, actual.Id, "Id");
            Assert.AreEqual<string>(expected.Name, actual.Name, "Name");
            Assert.AreEqual<string>(expected.ConnectionString, actual.ConnectionString, "ConnectionString");
        }
    }
}

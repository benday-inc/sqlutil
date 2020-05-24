using Benday.SqlUtils.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Benday.SqlUtils.UnitTests.ViewModels
{
    [TestClass]
    public class DatabaseQueryViewModelBaseFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }


        private DatabaseQueryViewModelBaseTester _SystemUnderTest;
        public DatabaseQueryViewModelBaseTester SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new DatabaseQueryViewModelBaseTester();
                }

                return _SystemUnderTest;
            }
        }

        [TestMethod]
        public void ValidateRequiredArguments_NoRequiredArgs()
        {
            SystemUnderTest.RequiredArguments.Clear();

            SystemUnderTest.Run();

            // if no exception, it worked
            Assert.IsTrue(SystemUnderTest.WasExecuteCalled, "Execute was not called.");
        }

        [TestMethod]
        public void ValidateRequiredArguments_TwoRequiredArgs_Valid()
        {
            SystemUnderTest.RequiredArguments.Clear();
            SystemUnderTest.RequiredArguments.Add("Value1");
            SystemUnderTest.RequiredArguments.Add("Value2");

            SystemUnderTest.SetArgumentValue("Value1", "val1");
            SystemUnderTest.SetArgumentValue("Value2", "val2");

            SystemUnderTest.Run();

            // if no exception, it worked
            Assert.IsTrue(SystemUnderTest.WasExecuteCalled, "Execute was not called.");

            Assert.IsTrue(SystemUnderTest.IsValid, "Should be valid.");
        }

        [TestMethod]
        public void HasArgumentValue_ReturnsTrueForValueThatExists()
        {
            string key = "asdf";
            string value = "value";

            SystemUnderTest.SetArgumentValue(key, value);

            Assert.IsTrue(SystemUnderTest.HasArgumentValue(key), "Should have the argument.");
        }

        [TestMethod]
        public void HasArgumentValue_ReturnsFalseForValueThatDoesNotExist()
        {
            string key = "asdf";

            Assert.IsFalse(SystemUnderTest.HasArgumentValue(key), "Should not have the argument.");
        }

        [TestMethod]
        public void ValidateRequiredArguments_TwoRequiredArgs_InvalidWhenOneMissing()
        {
            SystemUnderTest.RequiredArguments.Clear();
            SystemUnderTest.RequiredArguments.Add("Value1");
            SystemUnderTest.RequiredArguments.Add("Value2");

            SystemUnderTest.SetArgumentValue("Value1", "asdf");

            SystemUnderTest.Run();

            Assert.IsTrue(SystemUnderTest.WasExecuteCalled, "Execute was not called.");

            Assert.IsFalse(SystemUnderTest.IsValid, "Should not be valid.");

            string expectedMessage =
                "A value is required for 'Value2'.";

            Assert.AreEqual<string>(expectedMessage, SystemUnderTest.ValidationMessage, "Validation message is wrong.");
        }

        [TestMethod]
        public void SetArgumentValue_NewValue()
        {
            string expectedKey = "key";
            string expectedValue = "key value";

            SystemUnderTest.SetArgumentValue(expectedKey, expectedValue);

            string actualValue = SystemUnderTest.GetArgumentValue(expectedKey);

            Assert.AreEqual<string>(expectedValue, actualValue, "Value was wrong.");
        }

        [TestMethod]
        public void SetArgumentValue_OverwritesExistingValue()
        {

            string expectedKey = "key";
            string originalValue = "original";
            string expectedValue = "new value";

            SystemUnderTest.SetArgumentValue(expectedKey, originalValue);
            SystemUnderTest.SetArgumentValue(expectedKey, expectedValue);

            string actualValue = SystemUnderTest.GetArgumentValue(expectedKey);

            Assert.AreEqual<string>(expectedValue, actualValue, "Value was wrong.");
        }

        [TestMethod]
        public void GetArgumentValue_ReturnsNullWhenValueDoesNotExist()
        {
            string actual = SystemUnderTest.GetArgumentValue("key that does not exist");

            Assert.IsNull(actual, "Value for non-existent item should be null.");
        }

        [TestMethod]
        public void ValidateRequiredArguments_TwoRequiredArgs_InvalidWhenBothMissing()
        {
            SystemUnderTest.RequiredArguments.Clear();
            SystemUnderTest.RequiredArguments.Add("Value1");
            SystemUnderTest.RequiredArguments.Add("Value2");

            SystemUnderTest.Run();

            // if no exception, it worked
            Assert.IsTrue(SystemUnderTest.WasExecuteCalled, "Execute was not called.");

            Assert.IsFalse(SystemUnderTest.IsValid, "Should not be valid.");

            string expectedMessage =
                "Values are required for 'Value1' and 'Value2'.";

            Assert.AreEqual<string>(expectedMessage, 
                SystemUnderTest.ValidationMessage, 
                "Validation message is wrong.");
        }

        [TestMethod]
        public void ValidateRequiredArguments_ThreeRequiredArgs_ThreeMissing_ValidationMessageIsCorrect()
        {
            SystemUnderTest.RequiredArguments.Clear();
            SystemUnderTest.RequiredArguments.Add("Value1");
            SystemUnderTest.RequiredArguments.Add("Value2");
            SystemUnderTest.RequiredArguments.Add("Value3");

            SystemUnderTest.Run();

            // if no exception, it worked
            Assert.IsTrue(SystemUnderTest.WasExecuteCalled, "Execute was not called.");

            Assert.IsFalse(SystemUnderTest.IsValid, "Should not be valid.");

            string expectedMessage =
                "Values are required for 'Value1', 'Value2', and 'Value3'.";

            Assert.AreEqual<string>(expectedMessage,
                SystemUnderTest.ValidationMessage,
                "Validation message is wrong.");
        }

        [TestMethod]
        public void GenerateSqlCommand_TwoRequiredArgs_PopulatesArguments()
        {
            SystemUnderTest.RequiredArguments.Clear();

            SystemUnderTest.MatchMethod = Constants.SearchStringMethodExact;

            string arg0key = "id";
            string arg1key = "lastname";

            string arg0value = "id value";
            string arg1value = "%lastname%";

            SystemUnderTest.RequiredArguments.Add(arg0key);
            SystemUnderTest.RequiredArguments.Add(arg1key);

            SystemUnderTest.SetArgumentValue(arg0key, arg0value);
            SystemUnderTest.SetArgumentValue(arg1key, arg1value);

            var actual = SystemUnderTest.GetGeneratedSqlCommand();

            Assert.AreEqual<string>(SystemUnderTest.GetSqlQueryTemplate(), 
                actual.CommandText, "CommandText was wrong.");

            Assert.AreEqual<int>(2, actual.Parameters.Count, "Parameter count was wrong.");

            var parameter0 = actual.Parameters[0];
            var parameter1 = actual.Parameters[1];

            Assert.AreEqual<string>("@" + arg0key, parameter0.ParameterName, "Param 0 parameter name");
            Assert.AreEqual<string>("@" + arg1key, parameter1.ParameterName, "Param 1 parameter name");

            Assert.AreEqual<string>(arg0value, parameter0.Value.ToString(), "Param 0 parameter value");
            Assert.AreEqual<string>(arg1value, parameter1.Value.ToString(), "Param 1 parameter value");
        }

        [TestMethod]
        public void GenerateSqlCommand_OneRequiredArg_MatchMethod_Contains()
        {
            SystemUnderTest.RequiredArguments.Clear();
            
            string arg0key = "TABLE_NAME";

            string arg0value = "id value";

            string expectedArgValue = "%" + arg0value + "%";

            SystemUnderTest.MatchMethod = Constants.SearchStringMethodContains;

            SystemUnderTest.RequiredArguments.Add(arg0key);

            SystemUnderTest.SetArgumentValue(arg0key, arg0value);

            var actual = SystemUnderTest.GetGeneratedSqlCommand();

            Assert.AreEqual<string>(SystemUnderTest.GetSqlQueryTemplate(),
                actual.CommandText, "CommandText was wrong.");

            Assert.AreEqual<int>(1, actual.Parameters.Count, "Parameter count was wrong.");

            var parameter0 = actual.Parameters[0];

            Assert.AreEqual<string>("@" + arg0key, parameter0.ParameterName, "Param 0 parameter name");

            Assert.AreEqual<string>(
                expectedArgValue, parameter0.Value.ToString(), 
                "Param 0 parameter value");
        }

        [TestMethod]
        public void GenerateSqlCommand_OneRequiredArg_MatchMethod_StartsWith()
        {
            SystemUnderTest.RequiredArguments.Clear();

            string arg0key = "TABLE_NAME";

            string arg0value = "id value";

            string expectedArgValue = arg0value + "%";

            SystemUnderTest.MatchMethod = Constants.SearchStringMethodStartsWith;

            SystemUnderTest.RequiredArguments.Add(arg0key);

            SystemUnderTest.SetArgumentValue(arg0key, arg0value);

            var actual = SystemUnderTest.GetGeneratedSqlCommand();

            Assert.AreEqual<string>(SystemUnderTest.GetSqlQueryTemplate(),
                actual.CommandText, "CommandText was wrong.");

            Assert.AreEqual<int>(1, actual.Parameters.Count, "Parameter count was wrong.");

            var parameter0 = actual.Parameters[0];

            Assert.AreEqual<string>("@" + arg0key, parameter0.ParameterName, "Param 0 parameter name");

            Assert.AreEqual<string>(
                expectedArgValue, parameter0.Value.ToString(),
                "Param 0 parameter value");
        }

        [TestMethod]
        public void GenerateSqlCommand_OneRequiredArg_MatchMethod_EndsWith()
        {
            SystemUnderTest.RequiredArguments.Clear();

            string arg0key = "TABLE_NAME";

            string arg0value = "id value";

            string expectedArgValue = "%" + arg0value;

            SystemUnderTest.MatchMethod = Constants.SearchStringMethodEndsWith;

            SystemUnderTest.RequiredArguments.Add(arg0key);

            SystemUnderTest.SetArgumentValue(arg0key, arg0value);

            var actual = SystemUnderTest.GetGeneratedSqlCommand();

            Assert.AreEqual<string>(SystemUnderTest.GetSqlQueryTemplate(),
                actual.CommandText, "CommandText was wrong.");

            Assert.AreEqual<int>(1, actual.Parameters.Count, "Parameter count was wrong.");

            var parameter0 = actual.Parameters[0];

            Assert.AreEqual<string>("@" + arg0key, parameter0.ParameterName, "Param 0 parameter name");

            Assert.AreEqual<string>(
                expectedArgValue, parameter0.Value.ToString(),
                "Param 0 parameter value");
        }

        [TestMethod]
        public void GenerateSqlCommand_OneRequiredArg_MatchMethod_Exact()
        {
            SystemUnderTest.RequiredArguments.Clear();

            string arg0key = "TABLE_NAME";

            string arg0value = "id value";

            string expectedArgValue = arg0value;

            SystemUnderTest.MatchMethod = Constants.SearchStringMethodExact;

            SystemUnderTest.RequiredArguments.Add(arg0key);

            SystemUnderTest.SetArgumentValue(arg0key, arg0value);

            var actual = SystemUnderTest.GetGeneratedSqlCommand();

            Assert.AreEqual<string>(SystemUnderTest.GetSqlQueryTemplate(),
                actual.CommandText, "CommandText was wrong.");

            Assert.AreEqual<int>(1, actual.Parameters.Count, "Parameter count was wrong.");

            var parameter0 = actual.Parameters[0];

            Assert.AreEqual<string>("@" + arg0key, parameter0.ParameterName, "Param 0 parameter name");

            Assert.AreEqual<string>(
                expectedArgValue, parameter0.Value.ToString(),
                "Param 0 parameter value");
        }
    }
}

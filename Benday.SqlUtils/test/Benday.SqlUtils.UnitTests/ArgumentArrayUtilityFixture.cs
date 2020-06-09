using Benday.SqlUtils.Api;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benday.SqlUtils.UnitTests
{
    [TestClass]
    public class ArgumentArrayUtilityFixture
    {
        [TestMethod]
        public void ArgsToDictionary_ZeroArgs()
        {
            // arrange
            var expected = new Dictionary<string, string>();

            // act
            var actual = ArgumentArrayUtility.ArgsToDictionary();

            // assert
            AssertAreEqual(expected, actual);
        }

        [TestMethod]
        public void ArgsToDictionary_OneArg()
        {
            // arrange
            var expected = new Dictionary<string, string>();

            expected.Add("arg0", "arg0");

            // act
            var actual = ArgumentArrayUtility.ArgsToDictionary("arg0");

            // assert
            AssertAreEqual(expected, actual);
        }

        [TestMethod]
        public void ArgsToDictionary_TwoArgs()
        {
            // arrange
            var expected = new Dictionary<string, string>();

            expected.Add("arg0", "arg0.value");

            // act
            var actual = ArgumentArrayUtility.ArgsToDictionary("arg0", "arg0.value");

            // assert
            AssertAreEqual(expected, actual);
        }

        [TestMethod]
        public void ArgsToDictionary_ThreeArgs()
        {
            // arrange
            var expected = new Dictionary<string, string>();

            expected.Add("arg0", "arg0.value");
            expected.Add("arg1", "arg1");

            // act
            var actual = ArgumentArrayUtility.ArgsToDictionary(
                "arg0", "arg0.value",
                "arg1");

            // assert
            AssertAreEqual(expected, actual);
        }

        [TestMethod]
        public void ArgsToDictionary_FourArgs()
        {
            // arrange
            var expected = new Dictionary<string, string>();

            expected.Add("arg0", "arg0.value");
            expected.Add("arg1", "arg1.value");

            // act
            var actual = ArgumentArrayUtility.ArgsToDictionary(
                "arg0", "arg0.value",
                "arg1", "arg1.value");

            // assert
            AssertAreEqual(expected, actual);
        }

        private void AssertArgsToDictionary(int argCount)
        {
            // arrange
            var expected = GetDictionary(argCount);
            var args = GetArgs(argCount);

            // act
            var actual = ArgumentArrayUtility.ArgsToDictionary(
                args);

            // assert
            AssertAreEqual(expected, actual);
        }

        [TestMethod]
        public void ArgsToDictionary_RunMultipleTests()
        {
            AssertArgsToDictionary(0);
            AssertArgsToDictionary(1);
            AssertArgsToDictionary(2);
            AssertArgsToDictionary(3);
            AssertArgsToDictionary(4);
            AssertArgsToDictionary(5);
            AssertArgsToDictionary(6);
            AssertArgsToDictionary(7);
            AssertArgsToDictionary(8);
            AssertArgsToDictionary(9);
            AssertArgsToDictionary(10);
            AssertArgsToDictionary(11);
        }

        private Dictionary<string, string> GetDictionary(int argCount)
        {
            var returnValue = new Dictionary<string, string>();

            int iterationCount = argCount / 2;
            int iterationLeftover = argCount % 2;

            int currentIndex = -1;

            for (int i = 0; i < iterationCount; i++)
            {
                currentIndex++;
                returnValue.Add($"arg{currentIndex}", $"arg{currentIndex}.value");
            }

            currentIndex++;

            if (iterationLeftover != 0)
            {
                returnValue.Add($"arg{currentIndex}", $"arg{currentIndex}");
            }

            return returnValue;
        }

        private string[] GetArgs(int argCount)
        {
            var returnValue = new List<string>();

            var leftovers = argCount % 2;

            var currentIndex = -1;

            for (int i = 0; i < argCount / 2; i++)
            {
                currentIndex++;

                returnValue.Add($"arg{currentIndex}");
                returnValue.Add($"arg{currentIndex}.value");
            }

            if (leftovers > 0)
            {
                currentIndex++;
                returnValue.Add($"arg{currentIndex}");
            }

            return returnValue.ToArray();
        }

        [TestMethod]
        public void GetArgs_0()
        {
            // arrange
            var expectedArgCount = 0;
            var expected = new string[] { };

            // act
            var actual = GetArgs(expectedArgCount);

            // assert
            Assert.AreEqual<int>(expected.Length, actual.Length, "Array length didn't match");
            Assert.IsTrue(Enumerable.SequenceEqual(expected, actual), "Nope");
        }

        [TestMethod]
        public void GetArgs_1()
        {
            // arrange
            var expectedArgCount = 1;
            var expected = new string[] { "arg0" };

            // act
            var actual = GetArgs(expectedArgCount);

            // assert
            Assert.AreEqual<int>(expected.Length, actual.Length, "Array length didn't match");
            Assert.IsTrue(Enumerable.SequenceEqual(expected, actual), "Nope");
        }

        [TestMethod]
        public void GetArgs_2()
        {
            // arrange
            var expectedArgCount = 2;
            var expected = new string[] { "arg0", "arg0.value" };

            // act
            var actual = GetArgs(expectedArgCount);

            // assert
            Assert.AreEqual<int>(expected.Length, actual.Length, "Array length didn't match");
            Assert.IsTrue(Enumerable.SequenceEqual(expected, actual), "Nope");
        }

        [TestMethod]
        public void GetArgs_3()
        {
            // arrange
            var expectedArgCount = 3;
            var expected = new string[] { "arg0", "arg0.value", "arg1" };

            // act
            var actual = GetArgs(expectedArgCount);

            // assert
            Assert.AreEqual<int>(expected.Length, actual.Length, "Array length didn't match");
            Assert.IsTrue(Enumerable.SequenceEqual(expected, actual), "Nope");
        }

        [TestMethod]
        public void GetArgs_4()
        {
            // arrange
            var expectedArgCount = 4;
            var expected = new string[] { "arg0", "arg0.value", "arg1", "arg1.value" };

            // act
            var actual = GetArgs(expectedArgCount);

            // assert
            Assert.AreEqual<int>(expected.Length, actual.Length, "Array length didn't match");
            Assert.IsTrue(Enumerable.SequenceEqual(expected, actual), "Nope");
        }

        [TestMethod]
        public void GetDictionary_0Items()
        {
            // arrange
            var expectedArgCount = 0;
            var expected = new Dictionary<string, string>();

            // act
            var actual = GetDictionary(expectedArgCount);

            // assert
            AssertAreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDictionary_1Item()
        {
            // arrange
            var expectedArgCount = 1;
            var expected = new Dictionary<string, string>();

            expected.Add("arg0", "arg0");

            // act
            var actual = GetDictionary(expectedArgCount);

            // assert
            AssertAreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDictionary_2Items()
        {
            // arrange
            var expectedArgCount = 2;
            var expected = new Dictionary<string, string>();

            expected.Add("arg0", "arg0.value");

            // act
            var actual = GetDictionary(expectedArgCount);

            // assert
            AssertAreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDictionary_3Items()
        {
            // arrange
            var expectedArgCount = 3;
            var expected = new Dictionary<string, string>();

            expected.Add("arg0", "arg0.value");
            expected.Add("arg1", "arg1");

            // act
            var actual = GetDictionary(expectedArgCount);

            // assert
            AssertAreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDictionary_4Items()
        {
            // arrange
            var expectedArgCount = 4;
            var expected = new Dictionary<string, string>();

            expected.Add("arg0", "arg0.value");
            expected.Add("arg1", "arg1.value");

            // act
            var actual = GetDictionary(expectedArgCount);

            // assert
            AssertAreEqual(expected, actual);
        }



        [TestMethod]
        public void ArgsToDictionary_MoreArgs()
        {
            // arrange
            var argCount = 6;
            var expected = GetDictionary(argCount);
            var args = GetArgs(argCount);

            // act
            var actual = ArgumentArrayUtility.ArgsToDictionary(args);

            // assert
            AssertAreEqual(expected, actual);
        }

        private void AssertAreEqual(Dictionary<string, string> expected, Dictionary<string, string> actual)
        {
            Assert.IsNotNull(expected);
            Assert.IsNotNull(actual);

            Assert.AreEqual<int>(expected.Count, actual.Count, "Item count was wrong");

            var expectedKeys = expected.Keys.ToList();
            var actualKeys = actual.Keys.ToList();

            CollectionAssert.AreEquivalent(expectedKeys, actualKeys, "Keys are wrong.");

            foreach (string key in actualKeys)
            {
                Assert.AreEqual<string>(expected[key], actual[key], "Value for '{0}' is wrong.", key);
            }
        }



    }
}

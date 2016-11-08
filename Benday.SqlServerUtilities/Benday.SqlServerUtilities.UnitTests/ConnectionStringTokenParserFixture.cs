using Benday.SqlServerUtilities.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Benday.SqlServerUtilities.UnitTests
{
    [TestClass]
    public class ConnectionStringTokenParserFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private ConnectionStringTokenParser _SystemUnderTest;
        public ConnectionStringTokenParser SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new ConnectionStringTokenParser();
                }

                return _SystemUnderTest;
            }
        }

        private void InitializeWithSampleString()
        {
            SystemUnderTest.Initialize("Token1=token 1 value;Token2=token 2 value; Token 3 = token 3 value; Token4=token 4 value");
        }

        [TestMethod]
        public void GetTokenValue_SameCase()
        {
            InitializeWithSampleString();

            string actual = SystemUnderTest.GetValue("Token1");

            Assert.AreEqual<string>("token 1 value", actual, "errorMessage");
        }

        [TestMethod]
        public void TokensCollectionHasAllTokens()
        {
            InitializeWithSampleString();

            var actual = SystemUnderTest.Tokens;

            List<string> expected = new List<string>();

            expected.AddRange(new string[] { "Token1", "Token2", "Token 3", "Token4" });

            CollectionAssert.AreEquivalent(expected, actual);
        }



        [TestMethod]
        public void GetTokenValue_IgnoresCase()
        {
            InitializeWithSampleString();

            string actual = SystemUnderTest.GetValue("token1");

            Assert.AreEqual<string>("token 1 value", actual, "errorMessage");
        }



        [TestMethod]
        public void Contains_ReturnsTrueWhenSameCaseNoSpacesInTokenName()
        {
            InitializeWithSampleString();

            Assert.IsTrue(SystemUnderTest.Contains("Token1"));
        }

        [TestMethod]
        public void Contains_ReturnsTrueWhenSameCaseWithSpacesInTokenName()
        {
            InitializeWithSampleString();

            Assert.IsTrue(SystemUnderTest.Contains("Token 3"));
        }

        [TestMethod]
        public void Contains_ReturnsTrueForTrailingTokenWhenTheresNoDelimiter()
        {
            InitializeWithSampleString();

            Assert.IsTrue(SystemUnderTest.Contains("Token 4"));
        }

        [TestMethod]
        public void FindCaseSensitiveTokenName()
        {
            InitializeWithSampleString();

            var expected = "Token4";
            var actual = SystemUnderTest.FindCaseSensitiveTokenName("token4");

            Assert.AreEqual<string>(expected, actual, "Token didn't match");
        }



        [TestMethod]
        public void Contains_ReturnsTrueWhenExistsAndIgnoresCaseForNoSpaces()
        {
            InitializeWithSampleString();

            Assert.IsTrue(SystemUnderTest.Contains("token1"));
        }

        [TestMethod]
        public void Contains_ReturnsTrueWhenExistsAndIgnoresCaseForTokenWithSpaces()
        {
            InitializeWithSampleString();

            Assert.IsTrue(SystemUnderTest.Contains("token 3"));
        }

        [TestMethod]
        public void WhenInitializedThenTokenCountIsCorrect()
        {
            InitializeWithSampleString();

            Assert.AreEqual<int>(4, SystemUnderTest.Count, "Token count was wrong.");
        }

        [TestMethod]
        public void WhenNotInitializedThenTokenCountIsZero()
        {
            Assert.AreEqual<int>(0, SystemUnderTest.Count, "Token count was wrong.");
        }



    }
}

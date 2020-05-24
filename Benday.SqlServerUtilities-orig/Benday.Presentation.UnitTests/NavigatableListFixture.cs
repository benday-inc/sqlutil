using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Benday.Presentation.UnitTests
{
    [TestClass]
    public class NavigatableListFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;       
        }

        private NavigatableList<string> _SystemUnderTest;
        public NavigatableList<string> SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new NavigatableList<string>();

                    _SystemUnderTest.Add("Value 0");
                    _SystemUnderTest.Add("Value 1");
                    _SystemUnderTest.Add("Value 2");
                    _SystemUnderTest.Add("Value 3");
                    _SystemUnderTest.Add("Value 4");
                }

                return _SystemUnderTest;
            }
        }
        

        [TestMethod]
        public void WhenConstructedIsAtFirstIsTrue()
        {
            Assert.IsTrue(SystemUnderTest.IsAtFirst);
        }

        [TestMethod]
        public void WhenConstructedValueIsFirstItem()
        {
            Assert.AreEqual<string>("Value 0", SystemUnderTest.Value);
        }

        [TestMethod]
        public void WhenConstructedCurrentIndexIsZero()
        {
            Assert.AreEqual<int>(0, SystemUnderTest.CurrentIndex);
        }

        [TestMethod]
        public void WhenAtSecondValueIsAtFirstIsFalse()
        {
            SystemUnderTest.Next();

            Assert.IsFalse(SystemUnderTest.IsAtFirst);
        }

        [TestMethod]
        public void WhenNavigatedToFirstItemUsingPreviousIsAtFirstIsTrue()
        {
            SystemUnderTest.Next();

            SystemUnderTest.Previous();

            Assert.IsTrue(SystemUnderTest.IsAtFirst);
        }

        [TestMethod]
        public void WhenNotAtFirstIsAtFirstIsFalse()
        {
            SystemUnderTest.Next();

            Assert.IsFalse(SystemUnderTest.IsAtFirst);
        }

        [TestMethod]
        public void WhenNotAtLastIsAtLastIsFalse()
        {
            SystemUnderTest.Next();

            Assert.IsFalse(SystemUnderTest.IsAtLast);
        }

        [TestMethod]
        public void WhenNextIsCalledTheSecondValueIsCurrent()
        {
            SystemUnderTest.Next();

            Assert.AreEqual<int>(1, SystemUnderTest.CurrentIndex, "CurrentIndex value is wrong.");
            Assert.AreEqual<string>("Value 1", SystemUnderTest.Value, "Value is wrong.");
        }

        [TestMethod]
        public void WhenIsAtFirstMovePreviousDoesNothing()
        {
            Assert.IsTrue(SystemUnderTest.IsAtFirst);

            SystemUnderTest.Previous();

            Assert.IsTrue(SystemUnderTest.IsAtFirst);            
        }

        [TestMethod]
        public void WhenMoveToLastIsCalledThePositionIsSetToLast()
        {
            SystemUnderTest.MoveToLast();

            Assert.IsTrue(SystemUnderTest.IsAtLast);
            Assert.AreEqual<int>(4, SystemUnderTest.CurrentIndex);
        }

        [TestMethod]
        public void WhenMoveToFirstIsCalledThePositionIsSetToFirst()
        {
            SystemUnderTest.Next();
            SystemUnderTest.Next();
            SystemUnderTest.Next();

            SystemUnderTest.MoveToFirst();

            Assert.IsTrue(SystemUnderTest.IsAtFirst);
            Assert.AreEqual<int>(0, SystemUnderTest.CurrentIndex);
        }

        [TestMethod]
        public void WhenIsAtLastMoveNextDoesNothing()
        {
            SystemUnderTest.MoveToLast();
            
            SystemUnderTest.Next();

            Assert.IsTrue(SystemUnderTest.IsAtLast);
        }

        [TestMethod]
        public void WhenThereAreNoItemsCurrentIndexIsNegativeOne()
        {
            SystemUnderTest.Clear();

            Assert.AreEqual<int>(-1, SystemUnderTest.CurrentIndex);
        }

        [TestMethod]
        public void WhenThereAreNoItemsValueReturnsNull()
        {
            SystemUnderTest.Clear();

            Assert.IsNull(SystemUnderTest.Value);
        }
    }
}

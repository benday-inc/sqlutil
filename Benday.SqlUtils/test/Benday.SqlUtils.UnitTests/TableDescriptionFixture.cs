using Benday.SqlUtils.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benday.SqlUtils.UnitTests
{
    [TestClass]
    public class TableDescriptionFixture
    {
        [TestInitialize]
        public void OnTestInitialize()
        {
            _SystemUnderTest = null;
        }

        private TableDescription _SystemUnderTest;

        private TableDescription SystemUnderTest
        {
            get
            {
                if (_SystemUnderTest == null)
                {
                    _SystemUnderTest = new TableDescription();
                }

                return _SystemUnderTest;
            }
        }

        	
    }
}

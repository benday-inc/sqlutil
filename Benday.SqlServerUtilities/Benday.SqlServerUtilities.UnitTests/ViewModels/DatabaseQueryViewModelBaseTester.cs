using Benday.SqlServerUtilities.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.SqlServerUtilities.UnitTests.ViewModels
{
    public class DatabaseQueryViewModelBaseTester : DatabaseQueryViewModelBase
    {
        public DatabaseQueryViewModelBaseTester()
        {
            RequiredArguments = new List<string>();
        }

        public List<string> RequiredArguments 
        {
            get;
        }

        protected override List<string> GetRequiredArguments()
        {
            return RequiredArguments;
        }

        public bool WasExecuteCalled { get; set; }

        public override void Execute()
        {
            WasExecuteCalled = true;
        }
    }
}

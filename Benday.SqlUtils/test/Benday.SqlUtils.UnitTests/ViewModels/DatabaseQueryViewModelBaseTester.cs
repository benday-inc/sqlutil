using Benday.SqlUtils.Presentation.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.SqlUtils.UnitTests.ViewModels
{
    public class DatabaseQueryViewModelBaseTester : DatabaseQueryViewModelBase
    {
        public DatabaseQueryViewModelBaseTester() :
            base(new MockQueryRunner())
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

        protected override string SqlQueryTemplate
        {
            get
            {
                return "SELECT * FROM MyTable WHERE Id = @id AND LastName LIKE @LastName";
            }
        }

        public string GetSqlQueryTemplate()
        {
            return SqlQueryTemplate;    
        }

        public SqlCommand GetGeneratedSqlCommand()
        {
            return GetSqlCommand();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.SqlServerUtilities.Core
{
    public class DatabaseConnectionString
    {
        public void Load(string expectedConnectionString)
        {
            throw new NotImplementedException();
        }
        public string Database { get; set; }
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseIntegratedSecurity { get; set; }
        public string ConnectionString { get; set; }
    }
}

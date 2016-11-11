using System;
using System.Linq;

namespace Benday.SqlServerUtilities.Core
{
    public class StoredDatabaseConnectionString : IStoredDatabaseConnectionString
    {
        public StoredDatabaseConnectionString()
        {

        }

        public StoredDatabaseConnectionString(string id, string name, string connectionString)
        {
            Id = id;
            Name = name;
            ConnectionString = connectionString;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string ConnectionString { get; set; }
    }
}

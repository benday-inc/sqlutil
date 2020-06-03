using System;
using System.Linq;

namespace Benday.SqlUtils.Presentation
{
    public interface IStoredDatabaseConnectionString
    {
        string Id { get; }
        string Name { get; }
        string ConnectionString { get; }
    }
}

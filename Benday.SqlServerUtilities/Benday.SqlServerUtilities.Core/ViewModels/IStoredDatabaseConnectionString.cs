using System;
using System.Linq;

namespace Benday.SqlServerUtilities.Core.ViewModels
{
    public interface IStoredDatabaseConnectionString
    {
        string Id { get; }
        string Name { get; }
        string ConnectionString { get; }
    }
}

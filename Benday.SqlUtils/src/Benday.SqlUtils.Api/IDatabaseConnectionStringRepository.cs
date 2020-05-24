using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.SqlServerUtilities.Core
{
    public interface IDatabaseConnectionStringRepository
    {
        void Save(IStoredDatabaseConnectionString saveThis);
        void Delete(IStoredDatabaseConnectionString deleteThis);
        IList<IStoredDatabaseConnectionString> GetAll();
    }
}

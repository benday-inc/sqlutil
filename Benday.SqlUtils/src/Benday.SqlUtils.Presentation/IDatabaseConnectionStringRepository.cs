using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.SqlUtils.Presentation
{
    public interface IDatabaseConnectionStringRepository
    {
        void Save(IStoredDatabaseConnectionString saveThis);
        void Delete(IStoredDatabaseConnectionString deleteThis);
        IList<IStoredDatabaseConnectionString> GetAll();
    }
}

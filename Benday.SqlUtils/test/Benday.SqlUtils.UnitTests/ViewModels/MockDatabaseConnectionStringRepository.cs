using Benday.SqlUtils.Core;
using Benday.SqlUtils.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Benday.SqlUtils.UnitTests.ViewModels
{
    public class MockDatabaseConnectionStringRepository : IDatabaseConnectionStringRepository
    {
        private List<IStoredDatabaseConnectionString> _Items;
        public List<IStoredDatabaseConnectionString> Items
        {
            get { return _Items; }
        }

        public MockDatabaseConnectionStringRepository()
        {
            _Items = new List<IStoredDatabaseConnectionString>();
        }

        public void Delete(IStoredDatabaseConnectionString deleteThis)
        {
            var match = (from temp in Items
                         where temp.Id == deleteThis.Id
                         select temp).FirstOrDefault();

            if (match != null) Items.Remove(match);
        }

        public IList<IStoredDatabaseConnectionString> GetAll()
        {
            return Items.ToList();
        }

        public void Save(IStoredDatabaseConnectionString saveThis)
        {
            Delete(saveThis);

            var temp = new StoredDatabaseConnectionString(
                saveThis.Id, saveThis.Name, saveThis.ConnectionString);

            Items.Add(temp);
        }

        public void Add(string id, string name, string connectionString)
        {
            Save(new StoredDatabaseConnectionString(id, name, connectionString));
        }
    }
}

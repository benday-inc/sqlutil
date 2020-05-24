using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Benday.SqlServerUtilities.Core
{
    public class DatabaseConnectionStringRepository : IDatabaseConnectionStringRepository
    {
        [PreferredConstructor]
        public DatabaseConnectionStringRepository()
        {

        }

        public DatabaseConnectionStringRepository(string pathToConnectionsFile)
        {
            if (string.IsNullOrEmpty(pathToConnectionsFile))
                throw new ArgumentException($"{nameof(pathToConnectionsFile)} is null or empty.", nameof(pathToConnectionsFile));

            _ConnectionsFilePath = pathToConnectionsFile;
        }

        private string _ConnectionsFilePath;
        public string ConnectionsFilePath
        {
            get
            {
                if (_ConnectionsFilePath == null)
                {
                    var appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                    _ConnectionsFilePath = Path.Combine(appdata, 
                        "Benjamin Day Consulting, Inc.", 
                        "SQL Server Utilities", 
                        "connections.xml");
                }
                return _ConnectionsFilePath;
            }
        }

        public void Delete(IStoredDatabaseConnectionString deleteThis)
        {
            var root = Load();

            var match = (from temp in root.Elements("connection")
                         where temp.AttributeValue("id") == deleteThis.Id
                         select temp).FirstOrDefault();

            if (match != null)
            {
                match.Remove();
            }

            Save(root);
        }

        private XElement Load()
        {
            if (File.Exists(ConnectionsFilePath) == false)
            {
                var root = XElement.Parse("<connections />");

                Save(root);

                return root;
            }
            else
            {
                string contents = File.ReadAllText(ConnectionsFilePath);

                var root = XElement.Parse(contents);

                return root;
            }
        }

        private void Save(XElement root)
        {
            var directoryPath = Path.GetDirectoryName(ConnectionsFilePath);

            if (Directory.Exists(directoryPath) == false)
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(ConnectionsFilePath, root.ToString());
        }

        public IList<IStoredDatabaseConnectionString> GetAll()
        {
            XElement root = Load();

            var connections = root.Elements("connection");

            var returnValues = new List<IStoredDatabaseConnectionString>();

            foreach (var fromValue in connections)
            {
                var toValue = new StoredDatabaseConnectionString();

                AdaptXElementToStoredDatabaseConnectionString(
                        fromValue, toValue);

                returnValues.Add(toValue);
            }

            return returnValues;
        }

        private void AdaptXElementToStoredDatabaseConnectionString(
            XElement fromValue, 
            StoredDatabaseConnectionString toValue)
        {
            toValue.ConnectionString = fromValue.AttributeValue("connectionstring");
            toValue.Id = fromValue.AttributeValue("id");
            toValue.Name = fromValue.AttributeValue("name");
        }

        public void Save(IStoredDatabaseConnectionString saveThis)
        {
            var root = Load();

            var connectionElement = (from temp in root.Elements("connection")
                         where temp.AttributeValue("id") == saveThis.Id
                         select temp).FirstOrDefault();

            if (connectionElement != null)
            {
                // update existing

                connectionElement.SetAttributeValue("name", saveThis.Name);
                connectionElement.SetAttributeValue("connectionstring", saveThis.ConnectionString);
            }
            else
            {
                // save new

                connectionElement = new XElement("connection");

                connectionElement.SetAttributeValue("id", saveThis.Id);
                connectionElement.SetAttributeValue("name", saveThis.Name);
                connectionElement.SetAttributeValue("connectionstring", saveThis.ConnectionString);

                root.Add(connectionElement);
            }

            Save(root);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benday.SqlServerUtilities.Core
{
    public class DatabaseConnectionString
    {
        private ConnectionStringTokenParser _Parser = new ConnectionStringTokenParser();

        public void Load(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"{nameof(value)} is null or empty.", nameof(value));

            _Parser = null;
            _Parser = new ConnectionStringTokenParser();
            _Parser.Initialize(value);
        }

        private string _Database;
        public string Database
        {
            get
            {
                if (_Database == null)
                {
                    _Database = NullToEmptyString(_Parser.GetValue("Database"));
                }

                return _Database;
            }
            set
            {
                _Database = value;
            }
        }

        private string _Server;
        public string Server
        {
            get
            {
                if (_Server == null)
                {
                    _Server = NullToEmptyString(_Parser.GetValue("Server"));
                }

                return _Server;
            }
            set
            {
                _Server = value;
            }
        }
        private string _Username;
        public string Username
        {
            get
            {
                if (_Username == null)
                {
                    _Username = NullToEmptyString(_Parser.GetValue("User Id"));
                }

                return _Username;
            }
            set
            {
                _Username = value;
            }
        }

        private string _Password;
        public string Password
        {
            get
            {
                if (_Password == null)
                {
                    _Password = NullToEmptyString(_Parser.GetValue("Password"));
                }

                return _Password;
            }
            set
            {
                _Password = value;
            }
        }

        private bool? _UseIntegratedSecurity;
        public bool UseIntegratedSecurity
        {
            get
            {
                if (_UseIntegratedSecurity == null)
                {
                    var temp = _Parser.GetValue("Integrated Security");

                    if (temp == null)
                    {
                        _UseIntegratedSecurity = false;
                    }
                    else
                    {
                        if (temp.ToLower() == "true")
                        {
                            _UseIntegratedSecurity = true;
                        }
                        else
                        {
                            _UseIntegratedSecurity = false;
                        }
                    }
                }

                return _UseIntegratedSecurity.Value;
            }
            set
            {
                _UseIntegratedSecurity = value;
            }
        }
        public string ConnectionString { get; set; }
        private string NullToEmptyString(string value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            else
            {
                return value;
            }
        }
    }
}

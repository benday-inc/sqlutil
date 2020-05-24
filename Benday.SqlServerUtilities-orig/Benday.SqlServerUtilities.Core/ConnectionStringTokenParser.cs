using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Benday.SqlServerUtilities.Core
{
    public class ConnectionStringTokenParser
    {

        public ConnectionStringTokenParser()
        {

        }
        public void Initialize(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"{nameof(value)} is null or empty.", nameof(value));

            var nameValuePairs = value.Split(';');

            foreach (var pair in nameValuePairs)
            {
                var splitToken = pair.Split('=');

                if (splitToken.Length == 0)
                {
                    continue;
                }
                else
                {
                    string tokenValue = String.Empty;

                    string tokenName = splitToken[0].Trim();

                    if (splitToken.Length > 1)
                    {
                        tokenValue = splitToken[1].Trim();

                        if (tokenValue.Length > 1 && 
                            tokenValue.EndsWith(";") == true)
                        {
                            tokenValue = tokenValue.Substring(
                                0, tokenValue.Length - 1);
                        }
                    }

                    Add(tokenName, tokenValue);
                }
            }
        }
        public int Count
        {
            get
            {
                return _Values.Count;
            }
        }

        public bool Contains(string tokenName)
        {
            if (string.IsNullOrEmpty(tokenName))
                throw new ArgumentException($"{nameof(tokenName)} is null or empty.", nameof(tokenName));

            var actualTokenName = FindCaseSensitiveTokenName(tokenName);

            if (actualTokenName == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string GetValue(string tokenName)
        {
            if (string.IsNullOrEmpty(tokenName))
                throw new ArgumentException($"{nameof(tokenName)} is null or empty.", 
                    nameof(tokenName));

            var actualTokenName = FindCaseSensitiveTokenName(tokenName);

            if (actualTokenName == null)
            {
                return null;
            }
            else
            {
                return _Values[actualTokenName];
            }
        }

        public List<string> Tokens
        {
            get
            {
                return _Values.Keys.ToList();
            }
        }

        private Dictionary<string, string> _Values = new Dictionary<string, string>();
        private Dictionary<string, string> _ToLowerValues = new Dictionary<string, string>();


        private void Add(string key, string value)
        {
            if (_Values.ContainsKey(key) == true)
            {
                _Values.Remove(key);
                _ToLowerValues.Remove(key.ToLower());
            }
            else
            {
                _Values.Add(key, value);
                _ToLowerValues.Add(key.ToLower(), key);
            }
        }
        public string FindCaseSensitiveTokenName(string caseInsensitiveName)
        {
            var toLower = caseInsensitiveName.ToLower();

            if (_ToLowerValues.ContainsKey(toLower) == true)
            {
                return _ToLowerValues[toLower];
            }
            else
            {
                return null;
            }
        }
    }
}

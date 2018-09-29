using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Benday.SqlServerUtilities.Core.ViewModels
{
    public abstract class DatabaseQueryViewModelBase : ViewModelBase
    {
        private const string IsVisiblePropertyName = "IsVisible";

        private bool _IsVisible;
        public bool IsVisible
        {
            get
            {
                return _IsVisible;
            }
            set
            {
                _IsVisible = value;
                RaisePropertyChanged(IsVisiblePropertyName);
            }
        }

        private const string ResultsPropertyName = "Results";

        private DataTable _Results;

        public DataTable Results
        {
            get
            {
                return _Results;
            }
            set
            {
                _Results = value;
                RaisePropertyChanged(ResultsPropertyName);
            }
        }

        private Dictionary<string, string> _ArgumentValues;

        private Dictionary<string, string> ArgumentValues
        {
            get
            {
                if (_ArgumentValues == null)
                {
                    _ArgumentValues = new Dictionary<string, string>();
                }

                return _ArgumentValues;
            }
        }

        public void SetArgumentValue(string key, string value)
        {
            if (ArgumentValues.ContainsKey(key) == true)
            {
                ArgumentValues.Remove(key);
            }

            ArgumentValues.Add(key, value);
        }

        public string GetArgumentValue(string key)
        {
            if (HasArgumentValue(key) == false)
            {
                return null;
            }
            else
            {
                return ArgumentValues[key];
            }
        }

        public void Run()
        {
            ValidateArguments();

            Execute();
        }

        public abstract void Execute();

        public string ConnectionString { get; set; }

        protected abstract List<string> GetRequiredArguments();

        protected abstract string SqlQueryTemplate { get; }

        protected SqlCommand GetSqlCommand()
        {
            var command = new SqlCommand(SqlQueryTemplate);

            foreach (var key in _ArgumentValues.Keys)
            {
                var parameter = new SqlParameter();

                parameter.ParameterName = String.Format("@{0}", key);
                parameter.SqlDbType = SqlDbType.NVarChar;
                parameter.Value = _ArgumentValues[key];

                command.Parameters.Add(parameter);
            }

            return command;
        }

        protected virtual void ValidateArguments()
        {
            var requiredArguments = GetRequiredArguments();

            if (requiredArguments == null || requiredArguments.Count == 0)
            {
                // nothing to do
            }
            else
            {
                List<string> missingArgs = new List<string>();

                foreach (var requiredArg in requiredArguments)
                {
                    if (HasArgumentValue(requiredArg) == false)
                    {
                        missingArgs.Add(requiredArg);
                    }
                }

                if (missingArgs.Count > 0)
                {
                    ValidationMessage = GetValidationFailureMessageForMissingFields(
                        missingArgs);
                    IsValid = false;
                }
                else
                {
                    ValidationMessage = String.Empty;
                    IsValid = true;
                }
            }
        }
        private string GetValidationFailureMessageForMissingFields(List<string> missingArgs)
        {
            // "A value is required for ''.

            if (missingArgs.Count == 0)
            {
                return string.Empty;
            }
            else if (missingArgs.Count == 1)
            {
                return String.Format("A value is required for '{0}'.", missingArgs[0]);
            }
            else if (missingArgs.Count == 2)
            {
                return String.Format("Values are required for '{0}' and '{1}'.", 
                    missingArgs[0], missingArgs[1]);
            }
            else
            {
                return String.Format("Values are required for '{0}', '{1}', and '{2}'.",
                    missingArgs[0], missingArgs[1], missingArgs[2]);
            }
        }

        private const string IsValidPropertyName = "IsValid";

        private bool m_IsValid;
        public bool IsValid
        {
            get
            {
                return m_IsValid;
            }
            set
            {
                m_IsValid = value;
                RaisePropertyChanged(IsValidPropertyName);
            }
        }

        private const string ValidationMessagePropertyName = "ValidationMessage";

        private string m_ValidationMessage;
        public string ValidationMessage
        {
            get
            {
                return m_ValidationMessage;
            }
            set
            {
                m_ValidationMessage = value;
                RaisePropertyChanged(ValidationMessagePropertyName);
            }
        }

        public bool HasArgumentValue(string key)
        {
            return ArgumentValues.ContainsKey(key);
        }
    }
}

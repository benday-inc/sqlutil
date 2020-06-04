using Benday.SqlUtils.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace Benday.SqlUtils.ConsoleUi
{
    public class ExportDataCommand : CommandBase
    {
        public ExportDataCommand(string[] args, ITextOutputProvider outputProvider)
            : base(args, outputProvider)
        {

        }

        public ExportDataCommand(Dictionary<string, string> args, ITextOutputProvider outputProvider)
            : base(args, outputProvider)
        {

        }

        protected override List<string> GetRequiredArguments()
        {
            var argumentNames = new List<string>();

            argumentNames.Add(Constants.ArgumentNameServerName);
            argumentNames.Add(Constants.ArgumentNameDatabaseName);
            argumentNames.Add(Constants.ArgumentNameScriptType);
            argumentNames.Add(Constants.ArgumentNameQuery);

            return argumentNames;
        }

        protected override void DisplayUsage(StringBuilder builder)
        {
            base.DisplayUsage(builder);

            string usageString =
                String.Format("{0} {1} /{0}:server /{1}:database /{2}:query /{3}:script-type [/{4}:username] [/{5}:password] [/{6}:filename]",
                Constants.ExeName,
                CommandArgumentName,
                Constants.ArgumentNameServerName,
                Constants.ArgumentNameDatabaseName,
                Constants.ArgumentNameQuery,
                Constants.ArgumentNameScriptType,
                Constants.ArgumentNameUserName,
                Constants.ArgumentNamePassword,
                Constants.ArgumentNameFilename);

            builder.AppendLine(usageString);

            builder.AppendFormat("Valid values for /{0}: {1}, {2}, {3}",
                Constants.ArgumentNameScriptType, Constants.ArgumentValueScriptType_Insert,
                Constants.ArgumentValueScriptType_IdentityInsert, Constants.ArgumentValueScriptType_MergeInto);

            builder.AppendLine();
        }

        protected override string CommandArgumentName
        {
            get { return Constants.CommandArgumentNameExportData; }
        }

        protected override void AfterValidateArguments()
        {
            VerifyScriptTypeArguments();
            VerifyLoginArguments();
        }

        private void VerifyLoginArguments()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
            {
                if (ArgNameExists(Constants.ArgumentNameUserName) == false ||
                    ArgNameExists(Constants.ArgumentNamePassword) == false)
                {
                    // bad
                    var message = String.Format(
                        "Arguments for '{0}' and '{1}' is required when not running on Windows.",
                        Constants.ArgumentNameUserName, Constants.ArgumentNamePassword);

                    WriteLine(message);
                    throw new MissingArgumentException(message);
                }
            }
            else if (ArgNameExists(Constants.ArgumentNameUserName) == false &&
                ArgNameExists(Constants.ArgumentNamePassword) == false)
            {
                // good
            }
            else if (ArgNameExists(Constants.ArgumentNameUserName) == true &&
                ArgNameExists(Constants.ArgumentNamePassword) == true)
            {
                // good
            }
            else if (ArgNameExists(Constants.ArgumentNameUserName) == false)
            {
                // bad
                var message = String.Format(
                    "Argument for '{0}' is required if '{1}' argument is supplied.",
                    Constants.ArgumentNameUserName, Constants.ArgumentNamePassword);

                WriteLine(message);
                throw new MissingArgumentException(message);
            }
            else if (ArgNameExists(Constants.ArgumentNamePassword) == false)
            {
                // bad
                var message = String.Format(
                    "Argument for '{0}' is required if '{1}' argument is supplied.",
                    Constants.ArgumentNamePassword, Constants.ArgumentNameUserName);

                WriteLine(message);
                throw new MissingArgumentException(message);
            }
        }

        private void VerifyScriptTypeArguments()
        {
            var scriptType = Arguments[Constants.ArgumentNameScriptType];

            List<string> validValues = new List<string>();

            validValues.Add(Constants.ArgumentValueScriptType_IdentityInsert);
            validValues.Add(Constants.ArgumentValueScriptType_Insert);
            validValues.Add(Constants.ArgumentValueScriptType_MergeInto);

            if (validValues.Contains(scriptType) == false)
            {
                var message = String.Format("ERROR: Argument '{0}' value invalid.  Valid values are '{1}', '{2}', or '{3}'.",
                    Constants.ArgumentNameScriptType,
                    Constants.ArgumentValueScriptType_Insert,
                    Constants.ArgumentValueScriptType_IdentityInsert,
                    Constants.ArgumentValueScriptType_MergeInto);

                WriteLine(message);
                throw new MissingArgumentException(message);
            }
        }

        public string GetResult()
        {
            var connString = new DatabaseConnectionString();

            connString.Database = Arguments[Constants.ArgumentNameDatabaseName];
            connString.Server = Arguments[Constants.ArgumentNameServerName];

            if (Arguments.ContainsKey(Constants.ArgumentNameUserName) == false)
            {
                connString.UseIntegratedSecurity = true;
            }
            else
            {
                connString.UseIntegratedSecurity = false;

                connString.Username = Arguments[Constants.ArgumentNameUserName];
                connString.Password = Arguments[Constants.ArgumentNamePassword];
            }

            var databaseUtil = new SqlServerDatabaseUtility();

            databaseUtil.Initialize(connString.ConnectionString);

            var exporter = new SqlDataExport(databaseUtil, Arguments[Constants.ArgumentNameQuery]);

            if (Arguments[Constants.ArgumentNameScriptType] == Constants.ArgumentValueScriptType_IdentityInsert)
            {
                return exporter.GetInsertScript(true);
            }
            else if (Arguments[Constants.ArgumentNameScriptType] == Constants.ArgumentValueScriptType_Insert)
            {
                return exporter.GetInsertScript(false);
            }
            else if (Arguments[Constants.ArgumentNameScriptType] == Constants.ArgumentValueScriptType_MergeInto)
            {
                return exporter.GetMergeIntoScript();
            }
            else
            {
                throw new InvalidOperationException("Unsuppored script type.");
            }
        }

        public override void Run()
        {
            if (ArgNameExists(Constants.ArgumentNameFilename) == false)
            {
                WriteLine();
                WriteLine(GetResult());
            }
            else
            {
                WriteLine();

                string filenameArgValue = Arguments[Constants.ArgumentNameFilename];

                var filename = Path.Combine(Environment.CurrentDirectory, filenameArgValue);

                FileInfo info = new FileInfo(filename);

                var result = GetResult();

                if (String.IsNullOrWhiteSpace(result) == true)
                {
                    WriteLine("Problem generating the script.");
                }
                else
                {
                    File.WriteAllText(filename, result);

                    if (ArgNameExists(Constants.ArgumentNameQuiet) == false)
                    {
                        WriteLine(String.Format("Script written to '{0}'.", info.FullName));
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
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
            return null;
        }

        public override void Run()
        {
            /*
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
                    WriteLine("Could not locate the build definition.");
                }
                else
                {
                    File.WriteAllText(filename, result);

                    if (ArgNameExists(Constants.ArgumentNameQuiet) == false)
                    {
                        WriteLine(String.Format("Build definition written to '{0}'.", info.FullName));
                    }
                }
            }
            */
        }
    }
}

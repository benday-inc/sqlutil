using System;
using System.Collections.Generic;
using System.Text;

namespace Benday.SqlUtils.ConsoleUi
{
    public abstract class CommandBase
    {
        public CommandBase(string[] args, ITextOutputProvider outputProvider)
        {
            if (args == null)
            {
                throw new ArgumentException($"{nameof(args)} is null.", nameof(args));
            }

            if (outputProvider == null)
            {
                throw new ArgumentNullException(nameof(outputProvider), $"{nameof(outputProvider)} is null.");
            }

            InitializeAndValidate(ArgumentUtility.GetArgsAsDictionary(args), outputProvider);
        }

        public CommandBase(Dictionary<string, string> args, ITextOutputProvider outputProvider)
        {
            if (args == null || args.Count == 0)
            {
                throw new ArgumentException($"{nameof(args)} is null or empty.", nameof(args));
            }

            if (outputProvider == null)
            {
                throw new ArgumentNullException(nameof(outputProvider), $"{nameof(outputProvider)} is null.");
            }

            InitializeAndValidate(args, outputProvider);
        }

        private void InitializeAndValidate(
            Dictionary<string, string> args,
            ITextOutputProvider outputProvider)
        {
            _Arguments = args;
            _OutputProvider = outputProvider;

            ValidateArguments();
            AfterValidateArguments();
        }

        protected ITextOutputProvider _OutputProvider;
        public ITextOutputProvider OutputProvider
        {
            get
            {
                return _OutputProvider;
            }
        }

        protected abstract string CommandArgumentName { get; }

        protected void WriteLine(string line)
        {
            _OutputProvider.WriteLine(line);
        }

        protected void WriteLine()
        {
            _OutputProvider.WriteLine();
        }

        protected void WriteLine(string template, params object[] args)
        {
            _OutputProvider.WriteLine(String.Format(template, args));
        }

        protected virtual void RaiseExceptionAndDisplayError(bool displayUsage, string message)
        {
            StringBuilder builder = new StringBuilder();

            DisplayUsage(builder);

            builder.AppendLine(message);

            WriteLine(builder.ToString());

            throw new InvalidOperationException(message);
        }

        private void RaiseExceptionAndDisplayErrorForMissingArguments(List<string> missingArgs)
        {
            StringBuilder builder = new StringBuilder();

            DisplayUsage(builder);

            builder.AppendLine();

            builder.AppendLine("Invalid or missing arguments.");

            missingArgs.ForEach(x => builder.AppendLine(String.Format("- '{0}' is required.'", x)));

            WriteLine(builder.ToString());

            throw new MissingArgumentException(builder.ToString());
        }

        protected abstract List<string> GetRequiredArguments();

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
                    if (ArgNameExists(requiredArg) == false)
                    {
                        missingArgs.Add(requiredArg);
                    }
                }

                if (missingArgs.Count > 0)
                {
                    RaiseExceptionAndDisplayErrorForMissingArguments(missingArgs);
                }
            }
        }

        protected virtual void AfterValidateArguments()
        {

        }

        protected virtual void DisplayUsage(StringBuilder builder)
        {
            builder.AppendLine();
            builder.AppendLine("Team Foundation Server Utility");
            builder.AppendLine("Benjamin Day Consulting, Inc.");
            builder.AppendLine("www.benday.com");
            builder.AppendLine();
        }

        private Dictionary<string, string> _Arguments;
        public Dictionary<string, string> Arguments
        {
            get
            {
                return _Arguments;
            }
        }

        protected void AddArgument(string argName, string argValue)
        {
            AddArgument(Arguments, argName, argValue);
        }

        protected void AddArgument(Dictionary<string, string> args, string argName, string argValue)
        {
            if (ArgNameExists(args, argName) == true)
            {
                args.Remove(argName);
            }

            args.Add(argName, argValue);
        }

        protected void RemoveArgument(Dictionary<string, string> args, string argName)
        {
            if (ArgNameExists(args, argName) == true)
            {
                args.Remove(argName);
            }
        }

        protected void RenameArgument(Dictionary<string, string> args,
            string fromArgName, string toArgName)
        {
            if (ArgNameExists(args, fromArgName) == true)
            {
                var argValue = args[fromArgName];

                AddArgument(args, toArgName, argValue);

                args.Remove(fromArgName);
            }
            else
            {
                throw new InvalidOperationException(
                    String.Format("Cannot rename arg because no arg named '{0}' exists.",
                    fromArgName));
            }
        }

        protected bool ArgNameExists(string argName)
        {
            return ArgNameExists(Arguments, argName);
        }

        private bool ArgNameExists(Dictionary<string, string> args, string argName)
        {
            if (args.ContainsKey(argName) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void DebugArgDictionary(Dictionary<string, string> argsAsDictionary)
        {
            WriteLine("Valid arg count: {0}", argsAsDictionary.Keys.Count);

            foreach (var key in argsAsDictionary.Keys)
            {
                WriteLine("Arg: '{0}'; Value: '{1}';", key, argsAsDictionary[key]);
            }
        }

        public abstract void Run();
    }
}

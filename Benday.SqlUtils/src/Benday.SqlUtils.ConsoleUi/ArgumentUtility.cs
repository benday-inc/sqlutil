using System;
using System.Collections.Generic;

namespace Benday.SqlUtils.ConsoleUi
{
    public static class ArgumentUtility
    {
        public static string[] CreateArgsArray(params string[] args)
        {
            return args;
        }

        public static string GetArgEntry(string argumentName, string value)
        {
            return String.Format("/{0}:{1}", argumentName, value);
        }

        public static Dictionary<string, string> GetArgsAsDictionary(string[] args)
        {
            var returnValue = new Dictionary<string, string>();

            foreach (var arg in args)
            {
                if (String.IsNullOrWhiteSpace(arg) == false &&
                    arg.StartsWith("/") == true &&
                    arg.Contains(":") == true)
                {
                    CleanArgAndAddToDictionary(arg, returnValue);
                }
                else if (String.IsNullOrWhiteSpace(arg) == false &&
                    arg.StartsWith("/") == true &&
                    arg.Contains(":") == false)
                {
                    CleanArgWithoutColonAndAddToDictionary(arg, returnValue);
                }
            }

            return returnValue;
        }

        private static void CleanArgWithoutColonAndAddToDictionary(string arg, Dictionary<string, string> args)
        {
            var argWithoutSlash = arg.Substring(1).ToLower();

            if (args.ContainsKey(argWithoutSlash) == false)
            {
                args.Add(argWithoutSlash, String.Empty);
            }
        }

        private static void CleanArgAndAddToDictionary(string arg, Dictionary<string, string> args)
        {
            var argWithoutSlash = arg.Substring(1);

            int locationOfColon = argWithoutSlash.IndexOf(":");

            var argName = argWithoutSlash.Substring(0, locationOfColon);

            var argValue = argWithoutSlash.Substring(locationOfColon + 1).Trim();

            argValue = RemoveLeadingQuote(argValue);
            argValue = RemoveTrailingQuote(argValue);

            if (args.ContainsKey(argName) == false)
            {
                args.Add(argName, argValue);
            }
        }

        private static string RemoveLeadingQuote(string argValue)
        {
            if (argValue.StartsWith("\"") == true)
            {
                return argValue.Substring(1);
            }
            else
            {
                return argValue;
            }
        }

        private static string RemoveTrailingQuote(string argValue)
        {
            if (argValue.EndsWith("\"") == true)
            {
                return argValue.Substring(0, argValue.Length - 1);
            }
            else
            {
                return argValue;
            }
        }
    }
}

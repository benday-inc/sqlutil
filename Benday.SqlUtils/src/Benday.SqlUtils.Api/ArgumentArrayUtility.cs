using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benday.SqlUtils.Api
{
    public static class ArgumentArrayUtility
    {
        public static Dictionary<string, string> ArgsToDictionary(params string[] args)
        {
            Dictionary<string, string> returnValue = new Dictionary<string, string>();

            if (args == null || args.Length == 0)
            {
                // do nothing
            }
            else if (args.Length == 1)
            {
                returnValue.Add(args[0], args[0]);
            }
            else if (args.Length == 2)
            {
                returnValue.Add(args[0], args[1]);
            }
            else if (args.Length == 3)
            {
                returnValue.Add(args[0], args[1]);
                returnValue.Add(args[2], args[2]);
            }
            else if (args.Length == 4)
            {
                returnValue.Add(args[0], args[1]);
                returnValue.Add(args[2], args[3]);
            }
            else
            {
                int iterationCount = args.Length / 2;
                int leftovers = args.Length % 2;

                if (iterationCount > 0)
                {
                    int index = 0;

                    for (int i = 0; i < iterationCount; i++)
                    {
                        returnValue.Add(args[index], args[index + 1]);

                        index+=2;
                    }
                }

                if (leftovers > 0 && args.Length != 0)
                {
                    var lastItem = args.Last();

                    returnValue.Add(lastItem, lastItem);
                }
            }

            return returnValue;
        }
    }
}

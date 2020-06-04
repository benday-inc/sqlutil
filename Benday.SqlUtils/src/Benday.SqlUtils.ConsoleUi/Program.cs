using System;
using System.Text;

namespace Benday.SqlUtils.ConsoleUi
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                DisplayUsage();
            }
            else
            {
                try
                {
                    string commandName = args[0];

                    var outputProvider = new ConsoleTextOutputProvider();

                    if (commandName == Constants.CommandArgumentNameExportData)
                    {
                        new ExportDataCommand(args, outputProvider).Run();
                    }
                    else
                    {
                        DisplayUsage();
                    }
                }
                catch (MissingArgumentException)
                {

                }
                catch (KnownException ex)
                {
                    Console.Error.WriteLine();
                    Console.Error.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    var parsedArgs = ArgumentUtility.GetArgsAsDictionary(args);

                    if (parsedArgs.ContainsKey(Constants.ArgumentNameWaitBeforeExit) == true)
                    {
                        Console.WriteLine("Press ENTER to exit...");
                        Console.ReadLine();
                    }
                }
            }
        }

        private static void DisplayUsage()
        {
            string indent = "\t";

            StringBuilder builder = new StringBuilder();

            builder.AppendLine();
            builder.AppendLine("SQL Server Utilities");
            builder.AppendLine("Benjamin Day Consulting, Inc.");
            builder.AppendLine("www.benday.com");
            builder.AppendLine();
            builder.AppendLine("Available commands:");
            builder.AppendLine(indent + Constants.CommandArgumentNameExportData);
            builder.AppendLine();
            
            Console.WriteLine(builder.ToString());
        }
    }
}

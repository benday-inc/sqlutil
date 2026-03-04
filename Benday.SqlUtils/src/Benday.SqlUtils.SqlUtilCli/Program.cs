using Benday.CommandsFramework;

namespace Benday.SqlUtils.ShovelCli;

class Program
{
    static void Main(string[] args)
    {
        var options = new DefaultProgramOptions()
        {
            ApplicationName = "sqlutil",
            Website = "https://github.com/benday-inc/sql-server-utils",
            Version = "1.0.0",
            UsesConfiguration = true
        };

        options.DisplayUsageOptions.ShowCategories = true;

        var program = new DefaultProgram(options, typeof(Program).Assembly);
        program.Run(args);
    }
}

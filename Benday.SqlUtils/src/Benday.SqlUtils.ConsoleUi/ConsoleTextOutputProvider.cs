using System;

namespace Benday.SqlUtils.ConsoleUi
{
    public class ConsoleTextOutputProvider : ITextOutputProvider
    {
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }
        public void WriteLine()
        {
            Console.WriteLine();
        }
    }
}

using System;

namespace Benday.SqlUtils.ConsoleUi
{
    public interface ITextOutputProvider
    {
        void WriteLine(string line);
        void WriteLine();
    }
}

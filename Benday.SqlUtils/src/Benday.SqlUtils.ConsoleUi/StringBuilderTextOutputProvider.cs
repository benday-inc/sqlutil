using System;
using System.Text;

namespace Benday.SqlUtils.ConsoleUi
{
    public class StringBuilderTextOutputProvider : ITextOutputProvider
    {
        public StringBuilderTextOutputProvider()
        {
            _Instance = new StringBuilder();
        }

        private StringBuilder _Instance;

        public void WriteLine(string line)
        {
            _Instance.AppendLine(line);
        }

        public string GetOutput()
        {
            return _Instance.ToString();
        }
        public void WriteLine()
        {
            _Instance.AppendLine();
        }
    }
}

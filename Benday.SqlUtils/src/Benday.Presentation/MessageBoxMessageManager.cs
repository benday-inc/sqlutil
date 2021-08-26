using System;
using System.Diagnostics;
using System.Windows;

namespace Benday.Presentation
{
    public class MessageBoxMessageManager : IMessageManager
    {
        public void ShowMessage(Exception ex)
        {
            Trace.TraceError(ex.ToString());

            MessageBox.Show(ex.Message);
        }
    }
}

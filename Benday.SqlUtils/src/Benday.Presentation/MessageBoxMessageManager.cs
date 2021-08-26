using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows;

namespace Benday.Presentation
{
    public class MessageBoxMessageManager : IMessageManager
    {
        public void ShowMessage(Exception ex)
        {
            if (ex is null)
            {
                Trace.TraceError("MessageBoxMessageManager got a call to ShowMesage(ex) with a null exception.");
            }
            else
            {
                Trace.TraceError(ex.ToString());

                if (ex.InnerException != null && ex.InnerException is SqlException)
                {
                    MessageBox.Show(ex.InnerException.Message, 
                        "Error", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }            
        }
    }
}

using System;
using System.Windows;

namespace Benday.Presentation
{
    public interface IMessageManager
    {
        void ShowMessage(Exception ex);        
    }
}

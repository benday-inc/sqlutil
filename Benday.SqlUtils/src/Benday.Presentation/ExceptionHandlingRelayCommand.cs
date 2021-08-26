using GalaSoft.MvvmLight.Command;
using System;
using System.Linq;

namespace Benday.Presentation
{
    public class ExceptionHandlingRelayCommand : RelayCommand
    {
        private IMessageManager _msgManager;

        public ExceptionHandlingRelayCommand(IMessageManager msgManager, Action execute)
            : base(execute)
        {
            _msgManager = msgManager ?? throw new ArgumentNullException(nameof(msgManager), $"{nameof(msgManager)} is null.");
        }

        public override void Execute(object parameter)
        {
            try
            {
                base.Execute(parameter);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HandleException(Exception ex)
        {
            _msgManager.ShowMessage(ex);
        }
    }
}
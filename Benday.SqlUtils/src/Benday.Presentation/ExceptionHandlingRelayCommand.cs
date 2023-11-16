using CommunityToolkit.Mvvm.Input;
using System;
using System.Linq;

namespace Benday.Presentation
{
    public class ExceptionHandlingRelayCommand : IRelayCommand
    {
        private IMessageManager _MsgManager;
        private readonly Action _Action;

        public ExceptionHandlingRelayCommand(IMessageManager msgManager, Action execute)
        {
            _Action = execute ?? throw new ArgumentNullException(nameof(execute), $"{nameof(execute)} is null.");
            _MsgManager = msgManager ?? throw new ArgumentNullException(nameof(msgManager), $"{nameof(msgManager)} is null.");
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            try
            {
                _Action.Invoke();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void NotifyCanExecuteChanged()
        {
            throw new NotImplementedException();
        }

        private void HandleException(Exception ex)
        {
            _MsgManager.ShowMessage(ex);
        }
    }
}
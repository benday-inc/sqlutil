using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Benday.Presentation;

namespace Benday.MvvmTips.ViewModels
{
    public class MessageBoxSampleViewModel : MessageBoxRequestViewModelBase, IMessageBoxSampleViewModel
    {
        public MessageBoxSampleViewModel()
        {
            ThrowExceptionCommand = new RelayCommand(DoSomethingThatThrowsAnException);
            MessageBoxCommand = new RelayCommand(DoSomethingThatNeedsAMessageBox);
        }

        public ICommand ThrowExceptionCommand { get; set; }
        public ICommand MessageBoxCommand { get; set; }

        public void DoSomethingThatNeedsAMessageBox()
        {
            RequestMessageBox("Hi.  I'd like a message box.");
        }

        public void DoSomethingThatThrowsAnException()
        {
            try
            {
                int one = 1;
                int zero = 0;
                var result = one / zero;
                RequestMessageBox(String.Format("The result is {0}.", result));
            }
            catch (Exception ex)
            {
                RequestMessageBox("Something went horribly wrong.", ex);
            }
        }
    }
}

using Benday.Presentation;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class CancellableProgressBarDemoViewModel : MessagingViewModelBase
    {
        public CancellableProgressBarDemoViewModel(IMessageManager msgManager) :
            base(msgManager)
        {
            Progress = new ProgressBarViewModel();
            Progress.IsCancelable = true;
            Progress.OnCancelRequested += ProgressBar_OnCancelRequested;
        }

        private bool _IsCancelRequested = false;

        private void ProgressBar_OnCancelRequested(object sender, EventArgs e)
        {
            _IsCancelRequested = true;
        }

        private const string ProgressBarPropertyName = "Progress";

        private ProgressBarViewModel _ProgressBar;
        public ProgressBarViewModel Progress
        {
            get
            {
                return _ProgressBar;
            }
            set
            {
                _ProgressBar = value;
                RaisePropertyChanged(ProgressBarPropertyName);
            }
        }

        private ICommand _RunCommand;
        public ICommand RunCommand
        {
            get
            {
                if (_RunCommand == null)
                {
                    _RunCommand = new ExceptionHandlingRelayCommand(Messages, Run);
                }

                return _RunCommand;
            }
        }


        private void Run()
        {
            Task.Run(RunInBackground);
        }

        private void RunInBackground()
        {
            try
            {
                Console.WriteLine("RunInBackground() starting...");

                _IsCancelRequested = false;

                Progress.IsProgressBarVisible = true;

                int counter = 0;

                while (_IsCancelRequested == false)
                {
                    counter++;
                    Progress.ProgressBarMessage = $"progress update {counter}";
                    Thread.Sleep(1000);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("RunInBackground(): exception" + ex.ToString());
            }
            finally
            {
                Console.WriteLine("RunInBackground() exiting...");

                Progress.IsProgressBarVisible = false;
            }
        }
    }
}

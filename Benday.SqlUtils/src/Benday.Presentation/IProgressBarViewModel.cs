using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace Benday.Presentation
{
    public interface IProgressBarViewModel : INotifyPropertyChanged
    {
        bool IsProgressBarVisible { get; set; }
        string ProgressBarMessage { get; set; }
        bool IsCancelable { get; set; }

        ICommand CancelOperationCommand { get; }

        event EventHandler OnCancelRequested;
    }
}

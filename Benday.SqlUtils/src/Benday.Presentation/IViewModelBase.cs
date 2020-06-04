using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace Benday.Presentation
{
    public interface IViewModelBase : INotifyPropertyChanged
    {
        event MessageBoxEventHandler MessageBoxRequested;
    }
}

using System;
using System.Collections.Generic;

namespace Benday.Presentation
{
    public interface IProgressBarViewModel : IViewModelBase
    {
        bool IsProgressBarVisible { get; set; }
        string ProgressBarMessage { get; set; }
    }
}

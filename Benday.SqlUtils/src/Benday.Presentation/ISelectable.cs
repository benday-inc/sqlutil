using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace Benday.Presentation
{
    public interface ISelectable : INotifyPropertyChanged
    {
        bool IsSelected { get; set; }
    }
}

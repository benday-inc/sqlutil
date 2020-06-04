using System;
using System.ComponentModel;

namespace Benday.Presentation
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                var temp = PropertyChanged;

                if (temp != null)
                {
                    temp(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

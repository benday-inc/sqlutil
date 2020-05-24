using System;
using System.Windows;
using System.Windows.Input;

using System.Threading;


using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Generic;

namespace Benday.Presentation
{
    public class ViewModelField<T> : ViewModelBase, INotifyPropertyChanged, IVisibleField
    {
        public ViewModelField()
        {
            IsValid = true;
            IsVisible = true;
            ValidationMessage = String.Empty;
        }

        public ViewModelField(T initialValue)
            : this()
        {
            m_Value = initialValue;
        }

        private T m_Value;
        public T Value
        {
            get { return m_Value; }
            set
            {
                if (EqualityComparer<T>.Default.Equals(
                    m_Value, value) == false)
                {
                    m_Value = value;
                    RaisePropertyChanged("Value");
                    RaiseOnValueChanged();
                }
            }
        }

        public event EventHandler OnValueChanged;
        public virtual void RaiseOnValueChanged()
        {
            EventHandler handler = OnValueChanged;

            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        private const string IsVisiblePropertyName = "IsVisible";

        private bool m_IsVisible;
        public bool IsVisible
        {
            get
            {
                return m_IsVisible;
            }
            set
            {
                m_IsVisible = value;
                RaisePropertyChanged(IsVisiblePropertyName);
            }
        }

        private const string IsValidPropertyName = "IsValid";

        private bool m_IsValid;
        public bool IsValid
        {
            get
            {
                return m_IsValid;
            }
            set
            {
                m_IsValid = value;
                RaisePropertyChanged(IsValidPropertyName);
            }
        }

        private const string ValidationMessagePropertyName = "ValidationMessage";

        private string m_ValidationMessage;
        public string ValidationMessage
        {
            get
            {
                return m_ValidationMessage;
            }
            set
            {
                m_ValidationMessage = value;
                RaisePropertyChanged(ValidationMessagePropertyName);
            }
        }

        public override string ToString()
        {
            if (Value == null)
            {
                return String.Empty;
            }
            else
            {
                return Value.ToString();
            }
        }
    }
}

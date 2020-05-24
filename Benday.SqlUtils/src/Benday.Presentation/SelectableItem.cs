using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Benday.Presentation
{
    public class SelectableItem : ViewModelBase, ISelectableItem
    {
        public SelectableItem()
        {

        }

        /// <summary>
        /// Initializes a new instance of the SelectableItem class.
        /// </summary>
        /// <param name="isSelected"></param>
        /// <param name="text"></param>
        public SelectableItem(bool isSelected, string text)
            : this(isSelected, text, text)
        {

        }

        /// <summary>
        /// Initializes a new instance of the SelectableItemViewModel class.
        /// </summary>
        /// <param name="isSelected"></param>
        /// <param name="text"></param>
        /// <param name="value"></param>
        public SelectableItem(bool isSelected, string text, string value)
        {
            _IsSelected = isSelected;
            _Text = text;
            _Value = value;
        }

        public SelectableItem(bool isSelected, string text, int value)
        {
            _IsSelected = isSelected;
            _Text = text;
            _Id = value;
            _Value = value.ToString();
        }

        private const string IsSelectedPropertyName = "IsSelected";

        private bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    RaisePropertyChanged(IsSelectedPropertyName);    
                }                
            }
        }

        private const string TextPropertyName = "Text";

        private string _Text;
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                RaisePropertyChanged(TextPropertyName);
            }
        }

        private const string ValuePropertyName = "Value";

        private string _Value;
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                RaisePropertyChanged(ValuePropertyName);
            }
        }

        public override string ToString()
        {
            return Text;
        }
                
        private const string IdPropertyName = "Id";

        private int _Id;
        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
                RaisePropertyChanged(IdPropertyName);
            }
        }
    }
}

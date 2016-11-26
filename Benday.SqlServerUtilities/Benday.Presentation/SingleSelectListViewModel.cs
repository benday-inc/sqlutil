using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Benday.Presentation
{
    public class SingleSelectListViewModel : SelectableCollectionViewModel<ISelectableItem>,
        IVisibleField
    {        
        protected SingleSelectListViewModel() : base()
        {
            IsVisible = true;
        }

        public SingleSelectListViewModel(IEnumerable<ISelectableItem> values)
            : this(new ObservableCollection<ISelectableItem>(values))
        {
            if (values == null)
                throw new ArgumentNullException("values", "values is null.");

            IsVisible = true;
        }

        public SingleSelectListViewModel(IEnumerable<ISelectableItem> values, ISelectableItem selectedItem)
            : this(new ObservableCollection<ISelectableItem>(values), selectedItem)
        {
            if (values == null)
                throw new ArgumentNullException("values", "values is null.");
            if (selectedItem == null)
                throw new ArgumentNullException("selectedItem", "selectedItem is null.");

            IsVisible = true;
        }

        public SingleSelectListViewModel(ObservableCollection<ISelectableItem> values) : base(values)
        {
            IsVisible = true;
        }

        public SingleSelectListViewModel(ObservableCollection<ISelectableItem> values, ISelectableItem selectedItem)
            : base(values, selectedItem)
        {
            IsVisible = true;
        }

        public override void Initialize(IEnumerable<ISelectableItem> values)
        {
            Items = new ObservableCollection<ISelectableItem>(values);

            RefreshSelectedItem();

            IsVisible = true;
        }

        public void SelectByText(string text)
        {
            SelectedItem = GetByText(Items, text);
        }

        private ISelectableItem GetByText(ObservableCollection<ISelectableItem> values, string text)
        {
            var selected = (from temp in values
                            where temp.Text == text
                            select temp).FirstOrDefault();

            return selected;
        }

        protected ISelectableItem GetByValue(ObservableCollection<ISelectableItem> values, string value)
        {
            var selected = (from temp in values
                            where temp.Value == value
                            select temp).FirstOrDefault();

            return selected;
        }

        public virtual void SelectByValue(string value)
        {
            SelectedItem = GetByValue(Items, value);
        }

        public void SelectByValue(int value)
        {
            SelectedItem = GetByValue(Items, value.ToString());
        }

        private const string IsVisiblePropertyName = "IsVisible";

        private bool _IsVisible;
        public bool IsVisible
        {
            get
            {
                return _IsVisible;
            }
            set
            {
                _IsVisible = value;
                RaisePropertyChanged(IsVisiblePropertyName);
            }
        }

        private const string IsValidPropertyName = "IsValid";

        private bool _IsValid;
        public bool IsValid
        {
            get
            {
                return _IsValid;
            }
            set
            {
                _IsValid = value;
                RaisePropertyChanged(IsValidPropertyName);
            }
        }

        private const string ValidationMessagePropertyName = "ValidationMessage";

        private string _ValidationMessage;
        public string ValidationMessage
        {
            get
            {
                return _ValidationMessage;
            }
            set
            {
                _ValidationMessage = value;
                RaisePropertyChanged(ValidationMessagePropertyName);
            }
        }

    }
}
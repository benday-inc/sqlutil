using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace Benday.Presentation
{
    public class MultiSelectListViewModel : SingleSelectListViewModel
    {
        protected MultiSelectListViewModel() : base()
        {
            
        }

        public MultiSelectListViewModel(IList<ISelectableItem> values) : base(values)
        {
            
        }

        private const string SelectedItemsPropertyName = "SelectedItems";
        private IList<ISelectableItem> _SelectedItems;
        public IList<ISelectableItem> SelectedItems
        {
            get
            {
                _SelectedItems = GetSelectedItems(Items);

                return _SelectedItems;
            }
            set
            {
                _SelectedItems = value;
                RaisePropertyChanged(SelectedItemsPropertyName);
                RaisePropertyChanged(HasSelectedItemsPropertyName);
            }
        }

        protected const string HasSelectedItemsPropertyName = "HasSelectedItems";

        public virtual bool HasSelectedItems
        {
            get
            {
                var items = SelectedItems;

                if (items != null && items.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }            
        }

        private IList<ISelectableItem> GetSelectedItems(ObservableCollection<ISelectableItem> values)
        {
            var selected = (from temp in values
                            where temp.IsSelected == true
                            select temp).ToList();

            return selected;
        }

        public void SelectByValue(IList<int> values)
        {
            var valuesAsStrings = new List<string>();

            foreach (var value in values)
            {
                valuesAsStrings.Add(value.ToString());
            }

            SelectByValue(valuesAsStrings);
        }

        public void SelectByValue(IList<string> values)
        {
            foreach (var item in Items)
            {
                item.IsSelected = false;
            }

            if (values != null)
            {
                foreach (var item in values)
                {
                    SelectByValue(item);
                }
            }
        }

        public override void SelectByValue(string value)
        {
            var item = GetByValue(Items, value);

            if (item != null)
            {
                item.IsSelected = true;
            }
        }

        protected override void OnItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
            if (sender is ISelectableItem && e.PropertyName == "IsSelected")
            {
                // don't do the refresh logic that SingleSelectListViewModel does when an item changes it's IsSelected property
                // only notify that there's been a change

                RaisePropertyChanged(HasSelectedItemsPropertyName);
            }
        }
    }
}

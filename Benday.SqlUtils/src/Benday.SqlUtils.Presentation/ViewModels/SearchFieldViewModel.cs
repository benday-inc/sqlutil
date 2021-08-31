using Benday.Presentation;
using Benday.SqlUtils.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchFieldViewModel : ViewModelField<string>, ISearchFilterable
    {
        public SearchFieldViewModel()
        {
            SearchType = new SingleSelectListViewModel(new List<SelectableItem>());
            PopulateSearchTypes();
            SearchType.OnItemSelected += SearchType_OnItemSelected;
            IsValid = true;
            SearchType.IsValid = true;
        }

        public SearchFieldViewModel(string argName) : this()
        {
            if (argName == null)
            {
                throw new ArgumentNullException("argName", "Argument cannot be null.");
            }

            ArgName = argName;
        }

        private void SearchType_OnItemSelected(object sender, EventArgs e)
        {
            HandleSearchTypeSelected();
        }

        public void SelectSearchType(string searchTypeText)
        {
            var selectThis = (from temp in SearchType.Items
                              where temp.Text == searchTypeText
                              select temp).FirstOrDefault();

            if (selectThis == null)
            {
                throw new InvalidOperationException($"Search type '{searchTypeText}' not found.");
            }
            else
            {
                selectThis.IsSelected = true;
            }            
        }

        private void HandleSearchTypeSelected()
        {
            if (SearchType.Items.Count == 0 ||
                SearchType.SelectedItem == null)
            {
                IsEnabled = true;
            }
            else
            {
                if (SearchType.SelectedItem.Text == Constants.SearchTypeByValue)
                {
                    IsEnabled = true;
                }
                else
                {
                    IsEnabled = false;
                }
            }
        }

        private const string SearchTypePropertyName = "SearchType";

        private SingleSelectListViewModel _searchType;
        public SingleSelectListViewModel SearchType
        {
            get
            {
                return _searchType;
            }
            set
            {
                _searchType = value;
                RaisePropertyChanged(SearchTypePropertyName);
            }
        }

        public SearchFilter GetSearchFilter()
        {
            if (HasSearchFilter == false)
            {
                return null;
            }
            else
            {
                return new SearchFilter(ArgName, SearchType.SelectedItem.Text, Value);
            }
        }

        public string ArgName
        {
            get;
            set;
        }

        public bool HasSearchFilter
        {
            get
            {
                if (SearchType.SelectedItem == null)
                {
                    return false;
                }
                else if (SearchType.SelectedItem.Text == Constants.SearchTypeBlankOrEmpty)
                {
                    return true;
                }
                else if (SearchType.SelectedItem.Text == Constants.SearchTypeNotBlankOrEmpty)
                {
                    return true;
                }
                else if (SearchType.SelectedItem.Text == Constants.SearchTypeByValue)
                {
                    return !string.IsNullOrWhiteSpace(Value);
                }
                else
                {
                    return false;
                }
            }
        }

        private void PopulateSearchTypes()
        {
            SearchType.Add(new SelectableItem(true, Constants.SearchTypeByValue));
            SearchType.Add(new SelectableItem(false, Constants.SearchTypeBlankOrEmpty));
            SearchType.Add(new SelectableItem(false, Constants.SearchTypeNotBlankOrEmpty));
        }
    }
}

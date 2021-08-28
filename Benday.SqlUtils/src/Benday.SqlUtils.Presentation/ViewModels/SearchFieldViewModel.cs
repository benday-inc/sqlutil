using Benday.Presentation;
using Benday.SqlUtils.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchFieldViewModel : ViewModelField<string>
    {
        public SearchFieldViewModel()
        {
            SearchType = new SelectableCollectionViewModel<SelectableItem>();
            PopulateSearchTypes();
            SearchType.OnItemSelected += SearchType_OnItemSelected;
        }

        private void SearchType_OnItemSelected(object sender, EventArgs e)
        {
            HandleSearchTypeSelected();
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

        private SelectableCollectionViewModel<SelectableItem> _searchType;
        public SelectableCollectionViewModel<SelectableItem> SearchType
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

        private void PopulateSearchTypes()
        {
            SearchType.Add(new SelectableItem(true, Constants.SearchTypeByValue));
            SearchType.Add(new SelectableItem(false, Constants.SearchTypeBlankOrEmpty));
            SearchType.Add(new SelectableItem(false, Constants.SearchTypeNotBlankOrEmpty));
        }
    }
}

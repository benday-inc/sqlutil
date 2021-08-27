using Benday.Presentation;
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
            SearchType.Add(new SelectableItem(true, "By Value"));
            SearchType.Add(new SelectableItem(false, "Blank / Empty"));
            SearchType.Add(new SelectableItem(true, "Not Blank / Empty"));
        }
    }
}

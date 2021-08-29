namespace Benday.SqlUtils.Presentation.ViewModels
{
    public class SearchFilter
    {
        public SearchFilter()
        {
        }

        public SearchFilter(string argName, string searchType, string value)
        {
            ArgName = argName;
            SearchType = searchType;
            Value = value;
        }

        public string ArgName { get; }
        public string SearchType { get; }
        public string Value { get; }
    }
}

using Benday.Presentation;
using System;

namespace Benday.SqlUtils.Presentation.ViewModels
{
    public interface ISearchFilterable
    {
        SearchFilter GetSearchFilter();

        string ArgName { get; set; }
        bool HasSearchFilter { get; }
    }
}

using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Sort;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DnDNavigator.Runtime.Model.Search
{
    public interface ISearchService
    {
        List<Group<T>> OrderItems<T>(List<T> entries, Func<T, string> groupFunc);
        Task<List<IEntry>> PerformSearch(string userQuery, IEnumerable<string> categories);
    }
}

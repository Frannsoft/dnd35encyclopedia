using DnDNavigator.Runtime.Model.Entry;
using DnDNavigator.Runtime.Sort;
using System.Collections.Generic;

namespace DnDNavigator.Runtime.DataAccess
{
    public interface IHistoryService
    {
        void AddToHistory(BaseEntry entry);
        bool IsInHistory(BaseEntry item);
        void ClearHistory();
        List<Group<BaseEntry>> GetGroupedHistoryItems();
    }
}

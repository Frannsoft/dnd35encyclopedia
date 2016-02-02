using DnDNavigator.Runtime.Model.Entry;
using DnDNavigator.Runtime.Sort;
using System.Collections.Generic;

namespace DnDNavigator.Runtime.DataAccess
{
    public interface IFavoriteService
    {
        void AddToFavorites(BaseEntry entry);
        void RemoveFromFavorites(BaseEntry entry);
        bool IsInFavorites(BaseEntry entry);
        void DeleteFavorites(List<BaseEntry> favorites);
        List<Group<BaseEntry>> GetGroupedFavoriteItems();
    }
}

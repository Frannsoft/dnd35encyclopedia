using System;
using DnDNavigator.Runtime.Constants;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.DataAccess.EntityObjects;
using DnDEntry = DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Sort;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DnDNavigator.Runtime.Model.Browse
{
    public class ItemBrowseListService : IBrowseListService<DnDEntry.Item>
    {
        public async Task<List<Group<DnDEntry.Item>>> GetEntries()
        {
            EntryDataService eds = new EntryDataService();
            List<DnDEntry.Item> entries = await eds.GetAllEntries<ItemEntity, DnDEntry.Item>(DatabaseQueries.QUERY_ITEM);

            return PropertyKeyGroup<DnDEntry.Item>.GetItemGroupsBySpecialOrder(entries, e => e.Name);
        }
    }
}
using DnDNavigator.Runtime.Constants;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.DataAccess.EntityObjects;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Sort;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DnDNavigator.Runtime.Model.Browse
{
    public class PowerBrowseListService : IBrowseListService<Power>
    {
        public async Task<List<Group<Power>>> GetEntries()
        {
            EntryDataService eds = new EntryDataService();
            List<Power> entries = await eds.GetAllEntries<PowerEntity, Power>(DatabaseQueries.QUERY_POWER);

            return PropertyKeyGroup<Power>.GetItemGroupsBySpecialOrder(entries, e => e.Name);
        }
    }
}
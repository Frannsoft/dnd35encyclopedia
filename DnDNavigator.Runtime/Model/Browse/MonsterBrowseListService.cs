
using DnDNavigator.Runtime.Constants;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.DataAccess.EntityObjects;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Sort;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DnDNavigator.Runtime.Model.Browse
{
    public class MonsterBrowseListService : IBrowseListService<Monster>
    {
        public async Task<List<Group<Monster>>> GetEntries()
        {
            EntryDataService eds = new EntryDataService();
            List<Monster> entries = await eds.GetAllEntries<MonsterEntity, Monster>(DatabaseQueries.QUERY_MONSTER);

            return PropertyKeyGroup<Monster>.GetItemGroupsBySpecialOrder(entries, e => e.Name);
        }
    }
}

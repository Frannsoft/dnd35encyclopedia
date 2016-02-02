using DnDNavigator.Runtime.Constants;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.DataAccess.EntityObjects;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Sort;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DnDNavigator.Runtime.Model.Browse
{
    public class EquipmentBrowseListService : IBrowseListService<Equipment>
    {
        public async Task<List<Group<Equipment>>> GetEntries()
        {
            EntryDataService eds = new EntryDataService();
            List<Equipment> entries = await eds.GetAllEntries<EquipmentEntity, Equipment>(DatabaseQueries.QUERY_EQUIPMENT);

            return PropertyKeyGroup<Equipment>.GetItemGroupsBySpecialOrder(entries, e => e.Name);
        }
    }
}

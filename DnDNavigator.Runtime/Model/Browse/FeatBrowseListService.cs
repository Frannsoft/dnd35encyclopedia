using DnDNavigator.Runtime.Constants;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.DataAccess.EntityObjects;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Sort;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DnDNavigator.Runtime.Model.Browse
{
    public class FeatBrowseListService : IBrowseListService<Feat>
    {
        public async Task<List<Group<Feat>>> GetEntries()
        {
            EntryDataService eds = new EntryDataService();
            List<Feat> entries = await eds.GetAllEntries<FeatEntity, Feat>(DatabaseQueries.QUERY_FEAT);

            return PropertyKeyGroup<Feat>.GetItemGroupsBySpecialOrder(entries, e => e.Name);
        }
    }
}
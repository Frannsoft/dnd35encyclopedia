using DnDNavigator.Runtime.Constants;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.DataAccess.EntityObjects;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Sort;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DnDNavigator.Runtime.Model.Browse
{
    public class DomainBrowseListService : IBrowseListService<Domain>
    {
        public async Task<List<Group<Domain>>> GetEntries()
        {
            EntryDataService eds = new EntryDataService();
            List<Domain> entries = await eds.GetAllEntries<DomainEntity, Domain>(DatabaseQueries.QUERY_DOMAIN);

            return PropertyKeyGroup<Domain>.GetItemGroupsBySpecialOrder(entries, e => e.Name);
        }
    }
}

using DnDNavigator.Runtime.Constants;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.DataAccess.EntityObjects;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Sort;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DnDNavigator.Runtime.Model.Browse
{
    public class ClassBrowseListService : IBrowseListService<Class>
    {
        public async Task<List<Group<Class>>> GetEntries()
        {
            EntryDataService eds = new EntryDataService();
            List<Class> entries = await eds.GetAllEntries<ClassEntity, Class>(DatabaseQueries.QUERY_CLASS);

            return PropertyKeyGroup<Class>.GetItemGroupsBySpecialOrder(entries, e => e.Name);
        }
    }
}

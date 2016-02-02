using DnDNavigator.Runtime.Constants;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.DataAccess.EntityObjects;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Sort;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace DnDNavigator.Runtime.Model.Browse
{
    public class SpellBrowseListService : IBrowseListService<Spell>
    {
        public async Task<List<Group<Spell>>> GetEntries()
        {
            EntryDataService eds = new EntryDataService();
            List<Spell> entries = await eds.GetAllEntries<SpellEntity, Spell>(DatabaseQueries.QUERY_SPELL);

            return PropertyKeyGroup<Spell>.GetItemGroupsByAlpha(entries, CultureInfo.CurrentUICulture, e => e.Name);
        }

    }
}

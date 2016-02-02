using DnDNavigator.Runtime.Constants;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.DataAccess.EntityObjects;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Sort;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DnDNavigator.Runtime.Model.Browse
{
    public class SkillBrowseListService : IBrowseListService<Skill>
    {
        public async Task<List<Group<Skill>>> GetEntries()
        {
            EntryDataService eds = new EntryDataService();
            List<Skill> entries = await eds.GetAllEntries<SkillEntity, Skill>(DatabaseQueries.QUERY_SKILL);

            return PropertyKeyGroup<Skill>.GetItemGroupsBySpecialOrder(entries, e => e.Name);
        }
    }
}
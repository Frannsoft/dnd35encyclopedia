
using DnDNavigator.Runtime.Model;
using DnDNavigator.Runtime.Model.Entry;
using System.Collections.Generic;
namespace DnD35.SRD.Navigator8.Design
{
    public class DesignItemsService : IEntryService
    {
        public IEnumerable<Entry> Refresh()
        {
            List<Entry> result = new List<Entry>();

            for(int i = 0; i < 10; i++)
            {
                result.Add(
                    new Entry("Entry" + i, i));
            }

            return result;
        }
    }
}

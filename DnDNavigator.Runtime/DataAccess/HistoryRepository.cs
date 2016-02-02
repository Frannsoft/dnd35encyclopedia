using DnDNavigator.Runtime.Model.Entry;
using System.Runtime.Serialization;

namespace DnDNavigator.Runtime.DataAccess
{
    [CollectionDataContract(Name = "HistoryRepository")]
    public class HistoryRepository : Repository<BaseEntry>
    {
        public HistoryRepository()
        { }
    }
}

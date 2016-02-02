using DnDNavigator.Runtime.Model.Entry;
using System.Runtime.Serialization;

namespace DnDNavigator.Runtime.DataAccess
{
    [CollectionDataContract(Name = "FavoritesRepository")]
    public class FavoritesRepository : Repository<BaseEntry>
    {
        public FavoritesRepository()
        { }
    }
}

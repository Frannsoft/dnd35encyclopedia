using System.Collections.Generic;
using System.Threading.Tasks;

namespace DnDNavigator.Runtime.DataAccess
{
    public interface IDataService
    {
        Task<List<TResult>> GetAllEntries<T, TResult>(string queryStatement) where T : new();
    }
}

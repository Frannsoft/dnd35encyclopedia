
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DnDNavigator.Runtime.Model.Menu
{
    public interface IMenuOptionsService
    {
        Task<IEnumerable<MenuOption>> RefreshHomeMenu();
    }
}

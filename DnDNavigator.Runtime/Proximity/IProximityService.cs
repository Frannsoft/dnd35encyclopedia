
using DnDNavigator.Runtime.Model.DnDEntry;
using System.Threading.Tasks;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
namespace DnDNavigator.Runtime.Proximity
{
    public interface IProximityService
    {
        Task RetryConnection(StreamSocket socket);
        void MessageReceived(ProximityDevice sender, ProximityMessage message);
        void SendData(IEntry entry);
        void Share();
        void InitializeProximity();
    }
}

using DnDNavigator.Runtime.Model.Playlists;
using System.Runtime.Serialization;

namespace DnDNavigator.Runtime.DataAccess
{
    [CollectionDataContract(Name = "PlaylistRepository")]
    public class PlaylistRepository : Repository<Playlist>
    {
        public PlaylistRepository()
        { }
    }
}

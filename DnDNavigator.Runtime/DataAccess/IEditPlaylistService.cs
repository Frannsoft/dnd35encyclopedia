using DnDNavigator.Runtime.Model.Playlists;

namespace DnDNavigator.Runtime.DataAccess
{
    public interface IEditPlaylistService
    {
        void SavePlaylist(Playlist playlist);
    }
}

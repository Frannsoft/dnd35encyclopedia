using DnDNavigator.Runtime.Model.Entry;
using DnDNavigator.Runtime.Model.Playlists;
using DnDNavigator.Runtime.Sort;
using System.Collections.Generic;

namespace DnDNavigator.Runtime.DataAccess
{
    public interface IPlaylistsVMService
    {
        List<Group<Playlist>> LoadAllPlaylists();
        void SaveAllPlaylists(List<Playlist> playlists);
        void DeletePlaylists(List<Playlist> playlists);
        void SaveEntryToPlaylists(BaseEntry entry, List<Playlist> selectedPlaylists);
    }
}

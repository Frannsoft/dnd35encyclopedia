using DnDNavigator.Runtime.Model.Playlists;

namespace DnDNavigator.Runtime.DataAccess
{
    public class EditPlaylistService : IEditPlaylistService
    {
        public void SavePlaylist(Playlist playlist)
        {
            if (IsolatedStorage.Playlists.Contains(playlist))
            {
                for (int i = 0; i < IsolatedStorage.Playlists.Count; i++)
                {
                    if (playlist.Equals(IsolatedStorage.Playlists[i]))
                    {
                        IsolatedStorage.Playlists[i] = playlist;
                    }
                }
            }
            else
            {
                IsolatedStorage.Playlists.Add(playlist);
            }
        }
    }
}

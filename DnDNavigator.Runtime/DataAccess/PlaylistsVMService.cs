using DnDNavigator.Runtime.Model.Entry;
using DnDNavigator.Runtime.Model.Playlists;
using DnDNavigator.Runtime.Sort;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DnDNavigator.Runtime.DataAccess
{
    public class PlaylistsVMService : IPlaylistsVMService
    {
        public List<Group<Playlist>> LoadAllPlaylists()
        {
            List<Playlist> playlists = IsolatedStorage.Playlists.ToList();

            var groupedPlaylists = PropertyKeyGroup<Playlist>.GetItemGroupsByAlpha(playlists,
                                        CultureInfo.CurrentUICulture, p => p.Name);

            return groupedPlaylists;
        }

        public void SaveAllPlaylists(List<Playlist> playlists)
        {
            IsolatedStorage.Playlists = playlists;
        }


        public void DeletePlaylists(List<Playlist> playlists)
        {
            var existingPlaylists = IsolatedStorage.Playlists;

            foreach (Playlist p in playlists)
            {
                if (existingPlaylists.Contains(p))
                {
                    existingPlaylists.Remove(p);
                }
            }

            this.SaveAllPlaylists(existingPlaylists);
        }

        public void SaveEntryToPlaylists(BaseEntry entry, List<Playlist> selectedPlaylists)
        {
            if (entry != null && selectedPlaylists != null && selectedPlaylists.Count > 0)
            {
                List<Playlist> existingPlaylists = IsolatedStorage.Playlists;

                //assume the playlist exists in the list

                for (int i = 0; i < selectedPlaylists.Count; i++)
                {
                    Playlist p = selectedPlaylists[i];

                    Playlist existingPlaylist = (from playlist in existingPlaylists
                                                 where playlist.Name.Equals(p.Name)
                                                 select playlist).First();

                    if (!existingPlaylist.Items.Contains(entry))
                    {
                        existingPlaylist.Items.Add(entry);
                    }
                }
                 SaveAllPlaylists(existingPlaylists);
            }
        }
    }
}

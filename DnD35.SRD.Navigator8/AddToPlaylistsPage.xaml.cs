using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using DnD35.SRD.Navigator8.ViewModel;
using DnDNavigator.Runtime.Model.Entry;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.Model.Playlists;

namespace DnD35.SRD.Navigator8
{
    public partial class AddToPlaylistsPage : PhoneApplicationPage
    {
        public AddToPlaylistsPage()
        {
            InitializeComponent();
        }

        private BaseEntry GetStoredEntry()
        {
            return IsolatedStorage.TempEntry;
        }

        private void MainPivot_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = (PlaylistsViewModel)DataContext;
            vm.GetPlaylistsCommand.Execute(null);
            vm.ActiveEntry = GetStoredEntry();
        }


        private void OKAppBarButton_Click(object sender, EventArgs e)
        {
            if (PlaylistItemsList.SelectedItems.Count > 0)
            {
                List<Playlist> playlists = PlaylistItemsList.SelectedItems.Cast<Playlist>().ToList<Playlist>();
                var vm = (PlaylistsViewModel)DataContext;

                if (vm.ActiveEntry != null)
                {
                    vm.SaveEntryToPlaylistsCommand.Execute(playlists);
                }
            }
            NavigationService.GoBack();
        }

        private void CancelAppBarButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

    }
}
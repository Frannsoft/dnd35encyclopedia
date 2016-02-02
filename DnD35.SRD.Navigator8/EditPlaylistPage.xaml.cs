using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using DnD35.SRD.Navigator8.ViewModel;
using DnDNavigator.Runtime.Model.Playlists;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.Model.Entry;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Phone.Shell;

namespace DnD35.SRD.Navigator8
{
    public partial class EditPlaylistPage : PhoneApplicationPage
    {
        public EditPlaylistPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var queryString = NavigationContext.QueryString;
            string isNewQueryValue = string.Empty;
            bool isNew = false;

            if (queryString.TryGetValue("IsNew", out isNewQueryValue))
            {
                isNew = Convert.ToBoolean(isNewQueryValue);
            }

            if (isNew)
            {
                this.DataContext = new EditPlaylistViewModel();
            }
            else
            {
                //load up the existing playlist data from isostorage and shove it into a new playlistviewmodel
                Playlist playlist = IsolatedStorage.TempPlaylist;
                this.DataContext = new EditPlaylistViewModel(playlist);
            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (PlaylistItemsList.IsSelectionEnabled)
            {
                PlaylistItemsList.IsSelectionEnabled = false;
                e.Cancel = true;
            }
            else
            {
                PlaylistItemsList.SelectedItems.Clear();
                var vm = (EditPlaylistViewModel)DataContext;
                vm.CancelEditPlaylistCommand.Execute(null);
            }
            
        }

        private void OKAppBarButton_Click(object sender, EventArgs e)
        {
            string input = PlaylistNamePhoneTextBox.Text;

            if (!string.IsNullOrEmpty(input))
            {
                var vm = (EditPlaylistViewModel)DataContext;

                PlaylistItemsList.SelectedItems.Clear();
                PlaylistItemsList.IsSelectionEnabled = false;
                vm.CreateNewPlaylistCommand.Execute(input);
            }
            else
            {
                MessageBox.Show("Please enter a Name for the new Playlist.");
            }
        }

        private void CancelAppBarButton_Click(object sender, EventArgs e)
        {
            PlaylistItemsList.SelectedItems.Clear();
            PlaylistItemsList.IsSelectionEnabled = false;
            var vm = (EditPlaylistViewModel)DataContext;
            vm.CancelEditPlaylistCommand.Execute(null);
        }

        private void DeleteSelectedPlaylistItemsButton_Click(object sender, EventArgs e)
        {
            if (PlaylistItemsList.SelectedItems.Count > 0)
            {
                List<BaseEntry> selectedEntries = new List<BaseEntry>();
                var vm = (EditPlaylistViewModel)DataContext;
                foreach (BaseEntry entry in PlaylistItemsList.SelectedItems.Cast<BaseEntry>().ToList<BaseEntry>())
                {
                    selectedEntries.Add(entry);
                }

                vm.ModifyPlaylistItemsCommand.Execute(selectedEntries);
            }
            else
            {
                MessageBox.Show("Please select at least one entry to delete.");
            }

            PlaylistItemsList.SelectedItems.Clear();
            PlaylistItemsList.IsSelectionEnabled = false;
        }

        private void SelectionAppBarButton_Click(object sender, EventArgs e)
        {
            PlaylistItemsList.IsSelectionEnabled = !PlaylistItemsList.IsSelectionEnabled;
        }

        private void PlaylistItemsList_IsSelectionEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (PlaylistItemsList != null)
            {
                if (PlaylistItemsList.IsSelectionEnabled)
                {
                    ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).IsEnabled = true;
                }
                else
                {
                    ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).IsEnabled = false;
                }
            }
        }
    }
}
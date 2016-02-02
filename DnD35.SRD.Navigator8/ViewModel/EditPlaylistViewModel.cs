using DnD35.SRD.Navigator8.MVVMLight;
using DnD35.SRD.Navigator8.Navigation;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.Model.Entry;
using DnDNavigator.Runtime.Model.Playlists;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace DnD35.SRD.Navigator8.ViewModel
{
    public class EditPlaylistViewModel
    {
        private readonly IEditPlaylistService editPlaylistService;
        private readonly INavigationService navigationService;

        private Playlist playlist;
        public Playlist Playlist
        {
            get { return playlist; }
            private set
            {
                playlist = value;
                RaisePropertyChanged("Playlist");
            }
        }

        private List<BaseEntry> itemsDeletedHistory;
        private bool undo;

        private RelayCommand<string> createNewPlaylistCommand;
        public RelayCommand<string> CreateNewPlaylistCommand
        {
            get
            {
                return createNewPlaylistCommand
                    ?? (createNewPlaylistCommand = new RelayCommand<string>((name) =>
                    {
                        CreateNewPlaylist(name);
                    }));
            }
        }

        private RelayCommand cancelEditPlaylistCommand;
        public RelayCommand CancelEditPlaylistCommand
        {
            get
            {
                return cancelEditPlaylistCommand
                    ?? (cancelEditPlaylistCommand = new RelayCommand(() =>
                    {
                        undo = true;
                        CancelEditPlaylist();
                    }));
            }
        }

       

        private RelayCommand<List<BaseEntry>> modifyPlaylistItemsCommand;
        public RelayCommand<List<BaseEntry>> ModifyPlaylistItemsCommand
        {
            get
            {
                return modifyPlaylistItemsCommand
                    ?? (modifyPlaylistItemsCommand = new RelayCommand<List<BaseEntry>>((entries) =>
                    {
                        EditPlaylist(entries);
                    }));
            }
        }

        

        [PreferredConstructor]
        public EditPlaylistViewModel(IEditPlaylistService editPlaylistService, INavigationService navigationService)
        {
            this.editPlaylistService = editPlaylistService;
            this.navigationService = navigationService;
            Playlist = new Playlist(); //default name for a temp playlist on vm creation
        }

        public EditPlaylistViewModel()
            : this(new EditPlaylistService(), new NavigationService())
        { }

        public EditPlaylistViewModel(Playlist existingPlaylist)
            : this(new EditPlaylistService(), new NavigationService())
        {
            this.Playlist = existingPlaylist;
            itemsDeletedHistory = new List<BaseEntry>();
        }

        private void EditPlaylist(List<BaseEntry> entries)
        {
            if (entries.Count > 0)
            {
                foreach (BaseEntry entry in entries)
                {
                    if (Playlist.Items.Contains(entry))
                    {
                        itemsDeletedHistory.Add(entry);
                        Playlist.Items.Remove(entry);
                    }
                }
            }
        }

        private void CancelEditPlaylist()
        {
            if(undo && itemsDeletedHistory != null && itemsDeletedHistory.Count > 0)
            {
                foreach(BaseEntry entry in itemsDeletedHistory.ToList())
                {
                    if(!Playlist.Items.Contains(entry))
                    {
                        Playlist.Items.Add(entry);
                        itemsDeletedHistory.Remove(entry);
                    }
                }
            }

            navigationService.GoBack();
        }

        private void CreateNewPlaylist(string name)
        {
            this.Playlist.Name = name;
            editPlaylistService.SavePlaylist(this.Playlist);
            navigationService.NavigateTo(new Uri("/MainPage.xaml?GoTo=PlaylistPivot", UriKind.Relative));
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}

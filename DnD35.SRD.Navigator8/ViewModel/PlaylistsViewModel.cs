using DnD35.SRD.Navigator8.MVVMLight;
using DnD35.SRD.Navigator8.Navigation;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.Model.Entry;
using DnDNavigator.Runtime.Model.Playlists;
using DnDNavigator.Runtime.Sort;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DnD35.SRD.Navigator8.ViewModel
{
    public class PlaylistsViewModel : INotifyPropertyChanged
    {
        private readonly IPlaylistsVMService playlistVMService;
        private readonly INavigationService navigationService;

        /// <summary>
        /// Uses the LicenseService's IsLicensed bool as the authority on whether or not the device is licensed.
        /// </summary>
        public bool IsLicensed
        {
            get { return LicenseService.IsLicensed; }
        }

        /// <summary>
        /// Used for adding an entry to Playlists (only in AddToPlaylistsPage.xaml.cs for now)
        /// </summary>
        public BaseEntry ActiveEntry { get; set; }

        private bool isBusy;
        /// <summary>
        /// Views bind to this to display an indeterminate loading bar while stuff is being done.  
        /// I know this is in BaseListViewModel, but this view model doesn't need anything else from
        /// BaseListViewModel so I'm reimplementing it for now.
        /// </summary>
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public ObservableCollection<Group<Playlist>> PlaylistItems { get; private set; }

        private RelayCommand getPlaylistsCommand;
        public RelayCommand GetPlaylistsCommand
        {
            get
            {
                return getPlaylistsCommand
                    ?? (getPlaylistsCommand = new RelayCommand(() =>
                    {
                        IsBusy = true;
                        GetPlaylists();
                        IsBusy = false;
                    }));
            }
        }

        private RelayCommand buyPremiumCommand;
        public RelayCommand BuyPremiumCommand
        {
            get
            {
                return buyPremiumCommand
                    ?? (buyPremiumCommand = new RelayCommand(async () =>
                    {
                        await BuyPremium();
                    }));
            }
        }

        private RelayCommand createNewPlaylistCommand;
        public RelayCommand CreateNewPlaylistCommand
        {
            get
            {
                return createNewPlaylistCommand
                    ?? (createNewPlaylistCommand = new RelayCommand(() =>
                    {
                        CreateNewPlaylist();
                    }));
            }
        }

        private RelayCommand<List<Playlist>> deletePlaylistsCommand;
        public RelayCommand<List<Playlist>> DeletePlaylistsCommand
        {
            get
            {
                return deletePlaylistsCommand
                    ?? (deletePlaylistsCommand = new RelayCommand<List<Playlist>>((playlists) =>
                    {
                        DeleteSelectedPlaylists(playlists);
                    }));
            }
        }

        private RelayCommand<Playlist> goToPlaylistCommand;
        public RelayCommand<Playlist> GoToPlaylistCommand
        {
            get
            {
                return goToPlaylistCommand
                    ?? (goToPlaylistCommand = new RelayCommand<Playlist>((playlist) =>
                    {
                        NavigateToPlaylist(playlist);
                    }));
            }
        }

        private RelayCommand<List<Playlist>> saveEntryToPlaylistsCommand;
        public RelayCommand<List<Playlist>> SaveEntryToPlaylistsCommand
        {
            get
            {
                return saveEntryToPlaylistsCommand
                    ?? (saveEntryToPlaylistsCommand = new RelayCommand<List<Playlist>>((playlists) =>
                    {
                        SaveEntryToPlaylists(playlists);
                    }));
            }
        }



        [PreferredConstructor]
        public PlaylistsViewModel(IPlaylistsVMService playlistService, INavigationService navigationService)
        {
            this.playlistVMService = playlistService;
            this.navigationService = navigationService;
            this.PlaylistItems = new ObservableCollection<Group<Playlist>>();
        }

        public PlaylistsViewModel()
            : this(new PlaylistsVMService(), new NavigationService())
        { }

        private void SaveEntryToPlaylists(List<Playlist> playlists)
        {
            if (ActiveEntry != null)
            {
                playlistVMService.SaveEntryToPlaylists(ActiveEntry, playlists);
            }
        }

        private void DeleteSelectedPlaylists(List<Playlist> playlists)
        {
            playlistVMService.DeletePlaylists(playlists);
            RefreshPlaylists();
        }

        private void RefreshPlaylists()
        {
            if (PlaylistItems.Count > 0)
            {
                PlaylistItems.Clear();
            }

            GetPlaylists();
        }

        /// <summary>
        /// Kickoff purchase of premium features.
        /// </summary>
        private async Task BuyPremium()
        {
            await LicenseService.BeginPurchaseProduct();
            RaisePropertyChanged("IsLicensed");
        }

        /// <summary>
        /// Checks if user is licensed for Playlists.  If yes, loads up the playlists.  Otherwise,
        /// does nothing and the 'upgrade' button will be shown.
        /// </summary>
        private void GetPlaylists()
        {
            var items = playlistVMService.LoadAllPlaylists();

            //reset so we don't get duplicates reported
            if (PlaylistItems.Count > 0)
            { PlaylistItems.Clear(); }

            foreach (Group<Playlist> group in items)
            {
                PlaylistItems.Add(group);
            }

            RaisePropertyChanged("PlaylistItems");
        }

        private void NavigateToPlaylist(Playlist playlist)
        {
            IsolatedStorage.TempPlaylist = playlist;
            navigationService.NavigateTo(new Uri("/EditPlaylistPage.xaml?IsNew=false", UriKind.Relative));
        }


        /// <summary>
        /// Takes the user to the new Playlist Creation Page? (or maybe custom dialog box since they are just naming it?)
        /// </summary>
        private void CreateNewPlaylist()
        {
            navigationService.NavigateTo(new Uri("/EditPlaylistPage.xaml?IsNew=true", UriKind.Relative));
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

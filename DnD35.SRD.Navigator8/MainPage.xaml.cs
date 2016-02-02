using System.Windows;
using Microsoft.Phone.Controls;
using DnD35.SRD.Navigator8.ViewModel;
using System.Windows.Navigation;
using System;
using DnDNavigator.Runtime.Constants;
using System.Windows.Controls;
using Microsoft.Phone.Shell;
using DnDNavigator.Runtime.Feedback;
using DnDNavigator.Runtime.DataAccess;
using System.Windows.Media.Imaging;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using DnDNavigator.Runtime.Model.Playlists;
using System.Threading;
using DnDNavigator.Runtime.Model.Entry;

namespace DnD35.SRD.Navigator8
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            SetPivotBackgroungImage();
        }

        private void SetPivotBackgroungImage()
        {
            Visibility v = (Visibility)Resources["PhoneLightThemeVisibility"];

            if (v == Visibility.Visible)
            {
                //light theme used
                PivotBackgroundImageBrush.ImageSource = new BitmapImage(new Uri(@"\Assets\ApplicationIconLightTheme.png", UriKind.Relative));
            }
            else
            {
                //dark theme used
                PivotBackgroundImageBrush.ImageSource = new BitmapImage(new Uri(@"\Assets\ApplicationIconDarkTheme.png", UriKind.Relative));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (IsFromNFC())
            {
                this.NavigationService.Navigate(new Uri("/SharePage.xaml?IsHost=false", UriKind.Relative));
            }

            ShouldGoToSpecificPivot();
        }

        /// <summary>
        /// Check if GoTo url parameter is present
        /// </summary>
        /// <returns></returns>
        private bool ShouldGoToSpecificPivot()
        {
            bool retValue = false;
            string key = "GoTo";
            var queryString = this.NavigationContext.QueryString;

            if (queryString.ContainsKey(key))
            {
                GoToSpecificPivot(queryString[key]);
            }

            return retValue;
        }

        private void GoToSpecificPivot(string pivotName)
        {
            switch (pivotName)
            {
                case "PlaylistPivot":
                    {
                        MasterPivot.SelectedIndex = 3;
                        break;
                    }
                case "FavoritePivot":
                    {
                        MasterPivot.SelectedIndex = 2;
                        break;
                    }
                case "HistoryPivot":
                    {
                        MasterPivot.SelectedIndex = 1;
                        break;
                    }
                case "HomePivot":
                    {
                        MasterPivot.SelectedIndex = 0;
                        break;
                    }
            }
        }

        /// <summary>
        /// Returns true if this page was launched from an NFC event from another device (nfc argument passed in launch string for app).
        /// </summary>
        /// <returns></returns>
        private bool IsFromNFC()
        {
            const string NFC_TYPE_KEY = "Windows.Networking.Proximity.PeerFinder:StreamSocket";
            string queryStringValue = string.Empty;
            bool retValue = false;

            var queryString = this.NavigationContext.QueryString;

            if (queryString.TryGetValue("ms_nfp_launchargs", out queryStringValue))
            {
                if (queryStringValue.Equals(NFC_TYPE_KEY))
                {
                    retValue = true;
                }
            }
            return retValue;
        }

        private void HomePivot_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = (HomeViewModel)HomePivot.DataContext;
            vm.RefreshHomeMenuCommand.Execute(null);
        }

        private void ReleaseNotesMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ReleaseNotes.RELEASE_NOTES);
        }

        private void MasterPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((Pivot)sender).SelectedIndex)
            {
                case 0:
                    {
                        ApplicationBar = ((ApplicationBar)this.Resources["HomeAppBar"]);
                        break;
                    }
                case 1:
                    {
                        ApplicationBar = ((ApplicationBar)this.Resources["HistoryAppBar"]);
                        HistoryPivot.DataContext = new HistoryViewModel();
                        var vm = (HistoryViewModel)HistoryPivot.DataContext;
                        vm.GetHistoryItemsCommand.Execute(null);
                        break;
                    }
                case 2:
                    {
                        ApplicationBar = ((ApplicationBar)this.Resources["FavoritesAppBar"]);
                        FavoritePivot.DataContext = new FavoriteViewModel();
                        var vm = (FavoriteViewModel)FavoritePivot.DataContext;
                        vm.GetFavoriteItemsCommand.Execute(null);
                        break;
                    }
                case 3:
                    {
                        MaybeDisplayPlaylists();
                        break;
                    }
            }
        }

        private void MaybeDisplayPlaylists()
        {
            PlaylistPivot.DataContext = new PlaylistsViewModel();
            var vm = (PlaylistsViewModel)PlaylistPivot.DataContext;

            if (vm != null)
            {
                if (vm.IsLicensed)
                {
                    ApplicationBar = ((ApplicationBar)this.Resources["PlaylistsAppBar"]);
                    ApplicationBar.Mode = ApplicationBarMode.Default;
                    vm.GetPlaylistsCommand.Execute(null);
                }
                else
                {
                    ApplicationBar = null;
                }
            }
        }

        /// <summary>
        /// If selection mode is on, just turn it off when the user presses back
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (FavoriteItemsList.IsSelectionEnabled)
            {
                FavoriteItemsList.IsSelectionEnabled = false;
                ApplicationBar = ((ApplicationBar)this.Resources["FavoritesAppBar"]);
                ApplicationBar.Mode = ApplicationBarMode.Default;
                e.Cancel = true;
            }

            if (PlaylistItemsList.IsSelectionEnabled)
            {
                PlaylistItemsList.IsSelectionEnabled = false;
                ApplicationBar = ((ApplicationBar)this.Resources["PlaylistsAppBar"]);
                ApplicationBar.Mode = ApplicationBarMode.Default;
                e.Cancel = true;
            }
            else
            {
                base.OnBackKeyPress(e);
            }
        }

        private void PlaylistOnSelect_Click(object sender, EventArgs e)
        {
            PlaylistItemsList.IsSelectionEnabled = true;

            //ApplicationBar = ((ApplicationBar)this.Resources["DeletePlaylistsAppBar"]);
            //ApplicationBar.Mode = ApplicationBarMode.Default;
        }

        private void ClearHistoryAppBarButton_Click(object sender, EventArgs e)
        {
            var vm = (HistoryViewModel)HistoryPivot.DataContext;
            vm.ClearHistoryCommand.Execute(null);
        }

        private void FeedbackMenuItem_Click(object sender, EventArgs e)
        {
            new FeedbackEmail();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            IsolatedStorage.ShowExceptionMessageIfPresent();
        }

        private void CreateNewPlaylistButton_Click(object sender, EventArgs e)
        {
            var vm = (PlaylistsViewModel)PlaylistPivot.DataContext;
            vm.CreateNewPlaylistCommand.Execute(null);
        }

        private void DeletePlaylistsButton_Click(object sender, EventArgs e)
        {
            List<Playlist> items = PlaylistItemsList.SelectedItems.Cast<Playlist>().ToList<Playlist>();

            if (items.Count > 0)
            {
                var vm = (PlaylistsViewModel)PlaylistPivot.DataContext;
                vm.DeletePlaylistsCommand.Execute(items);
            }

            PlaylistItemsList.IsSelectionEnabled = false;
        }

        private void BuyPremiumButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = (PlaylistsViewModel)PlaylistPivot.DataContext;
            vm.BuyPremiumCommand.Execute(null);

            MaybeDisplayPlaylists();
        }

        private void PlaylistItemsList_IsSelectionEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (PlaylistItemsList.IsSelectionEnabled)
            {
                ApplicationBar = ((ApplicationBar)this.Resources["DeletePlaylistsAppBar"]);
                ApplicationBar.Mode = ApplicationBarMode.Default;
            }
            else
            {
                ApplicationBar = ((ApplicationBar)this.Resources["PlaylistsAppBar"]);
                ApplicationBar.Mode = ApplicationBarMode.Default;
            }
        }

        private void FavoriteItemsList_IsSelectionEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (FavoriteItemsList.IsSelectionEnabled)
            {
                ApplicationBar = ((ApplicationBar)this.Resources["DeleteFavoritesAppBar"]);
                ApplicationBar.Mode = ApplicationBarMode.Default;
            }
            else
            {
                ApplicationBar = ((ApplicationBar)this.Resources["FavoritesAppBar"]);
                ApplicationBar.Mode = ApplicationBarMode.Default;
            }
        }

        private void FavoritesEnableListSelectButton_Click(object sender, EventArgs e)
        {
            FavoriteItemsList.IsSelectionEnabled = true;
        }

        private void DeleteFavoritesButton_Click(object sender, EventArgs e)
        {
            List<BaseEntry> selectedFavoriteItems = FavoriteItemsList.SelectedItems.Cast<BaseEntry>().ToList<BaseEntry>();
            if (selectedFavoriteItems.Count > 0)
            {
                var vm = (FavoriteViewModel)FavoritePivot.DataContext;
                vm.DeleteFavoritesCommand.Execute(selectedFavoriteItems);
            }
        }

        private void ReleaseNotesTextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            MessageBox.Show(ReleaseNotes.RELEASE_NOTES);
        }
    }
}
using System.Windows;
using Microsoft.Phone.Controls;
using DnD35.SRD.Navigator8.ViewModel;
using Microsoft.Phone.Shell;
using System;
using System.Windows.Media;
using DnDNavigator.Runtime.DataAccess;

namespace DnD35.SRD.Navigator8
{
    public partial class EntryPage : PhoneApplicationPage
    {
        public EntryPage()
        {
            InitializeComponent();
        }

        private void EntryPageLoaded(object sender, RoutedEventArgs e)
        {
            var vm = (EntryViewModel)DataContext;
            vm.LoadEntryDataCommand.Execute(null);

            //load up the app bar - awful that wp8 doesn't have binding support for appbars
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            var phoneAccentBrush = new SolidColorBrush((App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush).Color);
            ApplicationBar.BackgroundColor = phoneAccentBrush.Color;

            //Add to favorites button
            ApplicationBarIconButton AddFavoritesAppBarButton = new ApplicationBarIconButton();
            AddFavoritesAppBarButton.IconUri = new Uri("/Assets/AppBar/favs.addto.png", UriKind.Relative);
            AddFavoritesAppBarButton.Text = "Add fave";
            AddFavoritesAppBarButton.IsEnabled = !IsFavorite();
            ApplicationBar.Buttons.Add(AddFavoritesAppBarButton);
            AddFavoritesAppBarButton.Click += new EventHandler(AddFavoritesAppBarButton_Clicked);

            //remove from favorites button
            ApplicationBarIconButton RemoveFavoritesAppBarButton = new ApplicationBarIconButton();
            RemoveFavoritesAppBarButton.IconUri = new Uri("/Assets/AppBar/delete.png", UriKind.Relative);
            RemoveFavoritesAppBarButton.Text = "Remove fave";
            RemoveFavoritesAppBarButton.IsEnabled = IsFavorite();
            ApplicationBar.Buttons.Add(RemoveFavoritesAppBarButton);
            RemoveFavoritesAppBarButton.Click += new EventHandler(RemoveFavoritesAppBarButton_Clicked);

            //share button
            ApplicationBarIconButton ShareAppBarButton = new ApplicationBarIconButton();
            ShareAppBarButton.IconUri = new Uri("/Assets/AppBar/share.png", UriKind.Relative);
            ShareAppBarButton.Text = "Share";
            ApplicationBar.Buttons.Add(ShareAppBarButton);
            ShareAppBarButton.Click += new EventHandler(ShareAppBarButton_Click);

            //view full text button
            ApplicationBarIconButton FullTextAppBarButton = new ApplicationBarIconButton();
            FullTextAppBarButton.IconUri = new Uri("/Assets/AppBar/edit.png", UriKind.Relative);
            FullTextAppBarButton.Text = "Full Text...";
            ApplicationBar.Buttons.Add(FullTextAppBarButton);
            FullTextAppBarButton.Click += new EventHandler(FullTextAppBarButton_Click);

            MaybeAddPlaylistAppBarButton();
        }

        private bool IsFavorite()
        {
            var vm = (EntryViewModel)DataContext;
            return vm.Entry.IsFavorite;
        }

        private void RefreshAppBar()
        {
            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = !IsFavorite();
            ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = IsFavorite();
        }

        private void MaybeAddPlaylistAppBarButton()
        {
            var vm = (EntryViewModel)DataContext;
            if (vm.IsLicensed)
            {
                ApplicationBarMenuItem PlaylistAppBarItem = new ApplicationBarMenuItem();
                PlaylistAppBarItem.Text = "Add Entry to Playlists";
                ApplicationBar.MenuItems.Add(PlaylistAppBarItem);
                PlaylistAppBarItem.Click += PlaylistAppBarItem_Click;
            }
        }

        private void PlaylistAppBarItem_Click(object sender, EventArgs e)
        {
            var vm = (EntryViewModel)DataContext;
            vm.AddEntryToPlaylistsCommand.Execute(vm.Entry);
        }

        private void FullTextAppBarButton_Click(object sender, EventArgs e)
        {
            var vm = (EntryViewModel)DataContext;
            vm.ViewFullTextCommand.Execute(vm.Entry);
        }

        private void ShareAppBarButton_Click(object sender, EventArgs e)
        {
            var vm = (EntryViewModel)DataContext;
            vm.ShareEntryCommand.Execute(vm.Entry);
        }

        private void AddFavoritesAppBarButton_Clicked(object sender, EventArgs args)
        {
            var vm = (EntryViewModel)DataContext;
            vm.AddEntryToFavoritesCommand.Execute(vm.Entry);
            RefreshAppBar();
        }

        private void RemoveFavoritesAppBarButton_Clicked(object sender, EventArgs args)
        {
            var vm = (EntryViewModel)DataContext;
            vm.RemoveEntryFromFavoritesCommand.Execute(vm.Entry);
            RefreshAppBar();
        }
    }
}
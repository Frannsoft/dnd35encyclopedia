using DnD35.SRD.Navigator8.MVVMLight;
using DnD35.SRD.Navigator8.Navigation;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.Model.Entry;
using DnDNavigator.Runtime.Sort;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace DnD35.SRD.Navigator8.ViewModel
{
    public abstract class BaseListViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService navigationService;
        private readonly IFavoriteService favoriteService;

        private bool isBusy;
        /// <summary>
        /// Views bind to this to display an indeterminate loading bar while stuff is being done
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

        private bool isFilterable;
        /// <summary>
        /// True if the current ViewModel is in a mode that supports Filtering
        /// results along with standard sorting of them.
        /// </summary>
        public bool IsFilterable
        {
            get { return isFilterable; }
            set
            {
                isFilterable = value;
                RaisePropertyChanged("IsFilterable");
            }
        }

        private RelayCommand<BaseEntry> addEntryToFavoritesCommand;
        public RelayCommand<BaseEntry> AddEntryToFavoritesCommand
        {
            get
            {
                return addEntryToFavoritesCommand
                    ?? (addEntryToFavoritesCommand = new RelayCommand<BaseEntry>(
                                                    (entry) =>
                                                    {
                                                        AddEntryToFavorites(entry);
                                                    }));
            }
        }

        private RelayCommand<BaseEntry> addEntryToPlaylistsCommand;
        public RelayCommand<BaseEntry> AddEntryToPlaylistsCommand
        {
            get
            {
                return addEntryToPlaylistsCommand
                    ?? (addEntryToPlaylistsCommand = new RelayCommand<BaseEntry>(
                                                    (entry) =>
                                                    {
                                                        AddEntryToPlaylists(entry);
                                                    }));
            }
        }

        private void AddEntryToPlaylists(BaseEntry entry)
        {
            if (LicenseService.IsLicensed)
            {
                NavigateToPlaylistsList(entry);
            }
        }

        private RelayCommand<BaseEntry> removeEntryFromFavoritesCommand;
        public RelayCommand<BaseEntry> RemoveEntryFromFavoritesCommand
        {
            get
            {
                return removeEntryFromFavoritesCommand
                    ?? (removeEntryFromFavoritesCommand = new RelayCommand<BaseEntry>(
                                                    (entry) =>
                                                    {
                                                        RemoveEntryFromFavorites(entry);
                                                    }));
            }
        }

        private RelayCommand<BaseEntry> navigateToEntryCommand;
        public RelayCommand<BaseEntry> NavigateToEntryCommand
        {
            get
            {
                return navigateToEntryCommand
                   ?? (navigateToEntryCommand = new RelayCommand<BaseEntry>(
                                                (entry) =>
                                                {
                                                    NavigateToEntry(entry);
                                                }));
            }
        }

        private RelayCommand<BaseEntry> shareEntryCommand;
        public RelayCommand<BaseEntry> ShareEntryCommand
        {
            get
            {
                return shareEntryCommand
                    ?? (shareEntryCommand = new RelayCommand<BaseEntry>(
                                            (entry) =>
                                            {
                                                ShareEntry(entry);
                                            }));
            }
        }

        private RelayCommand<BaseEntry> viewFullTextCommand;
        public RelayCommand<BaseEntry> ViewFullTextCommand
        {
            get
            {
                return viewFullTextCommand
                    ?? (viewFullTextCommand = new RelayCommand<BaseEntry>(
                                                (entry) =>
                                                {
                                                    ViewEntryFullText(entry);
                                                }));
            }
        }

        protected BaseListViewModel(INavigationService navigationService, IFavoriteService favoriteService)
        {
            this.navigationService = navigationService;
            this.favoriteService = favoriteService;
        }

        protected void AddEntryToFavorites(BaseEntry entry)
        {
            favoriteService.AddToFavorites(entry);
        }

        protected void RemoveEntryFromFavorites(BaseEntry entry)
        {
            favoriteService.RemoveFromFavorites(entry);
        }

        protected IList<Group<T>> GroupItemsByProperty<T>(IList<Group<T>> existingEntries, Func<T, string> groupFunc)
        {
            List<T> tempList = AddExistingEntriesToList<T>(existingEntries);
            ClearExistingEntries<T>(existingEntries);

            var resultEntries = PropertyKeyGroup<T>.GetItemGroupsBySpecialOrder(tempList, groupFunc);

            return resultEntries;
        }

        protected IList<Group<T>> GroupItemsByPropertyAndFilter<T>(IList<Group<T>> existingEntries, Func<T, string> groupFunc)
        {
            List<T> tempList = AddExistingEntriesToList<T>(existingEntries);
            ClearExistingEntries<T>(existingEntries);

            var resultEntries = PropertyKeyGroup<T>.GetItemGroupsBySpecialOrderAndFilter(tempList, groupFunc);

            return resultEntries;
        }

        protected IList<Group<T>> GroupItemsByAlphaName<T>(IList<Group<T>> existingEntries, Func<T, string> groupFunc)
        {
            List<T> tempList = AddExistingEntriesToList<T>(existingEntries);
            ClearExistingEntries<T>(existingEntries);

            var resultEntries = PropertyKeyGroup<T>.GetItemGroupsByAlpha(tempList, CultureInfo.CurrentUICulture, groupFunc);

            return resultEntries;
        }

        protected void NavigateToEntry(BaseEntry entry)
        {
            //store entry in isostorage
            IsolatedStorage.TempEntry = entry;

            navigationService.NavigateTo(new Uri("/EntryPage.xaml", UriKind.Relative));
        }

        protected void NavigateToPlaylistsList(BaseEntry entry)
        {
            IsolatedStorage.TempEntry = entry;
            navigationService.NavigateTo(new Uri("/AddToPlaylistsPage.xaml", UriKind.Relative));
        }

        protected void ViewEntryFullText(BaseEntry entry)
        {
            IsolatedStorage.TempEntry = entry;
            navigationService.NavigateTo(new Uri("/FullTextViewerPage.xaml", UriKind.Relative));
        }

        private void ShareEntry(BaseEntry entry)
        {
            IsolatedStorage.TempEntry = entry;
            navigationService.NavigateTo(new Uri("/SharePage.xaml?IsHost=true", UriKind.Relative));
        }

        private void ClearExistingEntries<T>(IList<Group<T>> existingEntries)
        {
            existingEntries.Clear();
        }

        private List<T> AddExistingEntriesToList<T>(IList<Group<T>> existingEntries)
        {
            List<T> tempList = new List<T>();

            foreach (Group<T> group in existingEntries)
            {
                tempList.AddRange(group);
            }

            return tempList;
        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName)
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

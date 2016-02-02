using DnD35.SRD.Navigator8.MVVMLight;
using DnD35.SRD.Navigator8.Navigation;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.Model.Entry;
using DnDNavigator.Runtime.Model.Playlists;
using DnDNavigator.Runtime.Sort;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DnD35.SRD.Navigator8.ViewModel
{
    public class FavoriteViewModel : BaseListViewModel
    {
        private readonly IFavoriteService favoriteService;

        public ObservableCollection<Group<BaseEntry>> FavoriteItems { get; private set; }

        private RelayCommand getFavoriteItemsCommand;
        public RelayCommand GetFavoriteItemsCommand
        {
            get
            {
                return getFavoriteItemsCommand ?? (getFavoriteItemsCommand =
                    new RelayCommand(() =>
                    {
                        IsBusy = true;
                        GetFavoriteItems();
                        IsBusy = false;
                    }));
            }
        }

        private RelayCommand<BaseEntry> navigateToFavoriteEntryCommand;
        public RelayCommand<BaseEntry> NavigateToFavoriteEntryCommand
        {
            get
            {
                return navigateToFavoriteEntryCommand ?? (navigateToFavoriteEntryCommand =
                    new RelayCommand<BaseEntry>((entry) =>
                    {
                        NavigateToFavoriteEntry(entry);
                    }));
            }
        }

        private RelayCommand<List<BaseEntry>> deleteFavoritesCommand;
        public RelayCommand<List<BaseEntry>> DeleteFavoritesCommand
        {
            get
            {
                return deleteFavoritesCommand ?? (deleteFavoritesCommand =
                    new RelayCommand<List<BaseEntry>>((favorites) =>
                    {
                        DeleteFavorites(favorites);
                    }));
            }
        }

        [PreferredConstructor]
        public FavoriteViewModel(IFavoriteService favoriteService, INavigationService navigationService)
            : base(navigationService, favoriteService)
        {
            this.favoriteService = favoriteService;
            FavoriteItems = new ObservableCollection<Group<BaseEntry>>();
        }

        public FavoriteViewModel()
            : this(new FavoriteService(), new NavigationService())
        { }

        private void DeleteFavorites(List<BaseEntry> favorites)
        {
            favoriteService.DeleteFavorites(favorites);
            GetFavoriteItems();
        }

        private void GetFavoriteItems()
        {
            ClearFavoriteItems();

            var favoriteItems = favoriteService.GetGroupedFavoriteItems();

            //returns entries grouped by alpha by default
            foreach (Group<BaseEntry> entry in favoriteItems)
            {
                FavoriteItems.Add(entry);
            }

            RaisePropertyChanged("FavoriteItems");
        }

        private void ClearFavoriteItems()
        {
            if(FavoriteItems.Count > 0)
            {
                FavoriteItems.Clear();
                RaisePropertyChanged("FavoriteItems");
            }
        }

        private void NavigateToFavoriteEntry(BaseEntry entry)
        {
            NavigateToEntry(entry);
        }
    }
}

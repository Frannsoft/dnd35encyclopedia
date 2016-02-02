using DnD35.SRD.Navigator8.MVVMLight;
using DnD35.SRD.Navigator8.Navigation;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.Model.Browse;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Sort;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DnD35.SRD.Navigator8.ViewModel
{
    public class DomainBrowseListViewModel : BaseListViewModel, IBrowseListViewModel<Domain>
    {
        private readonly IBrowseListService<Domain> browseListService;

        public ObservableCollection<Group<Domain>> Entries { get; private set; }
        public EntryType.Types EntryType { get; private set; }

        public enum SortType
        {
            [Description("Name")]
            Name          
        }

        public IEnumerable<SortType> SortTypes
        {
            get
            {
                return Enum.GetValues(typeof(SortType)).Cast<SortType>();
            }
        }

        private RelayCommand getEntriesCommand;
        public RelayCommand GetEntriesCommand
        {
            get
            {
                return getEntriesCommand
                    ?? (getEntriesCommand = new RelayCommand(
                                                async () =>
                                                {
                                                    IsBusy = true;
                                                    await GetEntries();
                                                    ChangeSort(SortType.Name);
                                                    IsBusy = false;

                                                }));
            }
        }

        private RelayCommand<string> reorderEntriesCommand;
        public RelayCommand<string> ReorderEntriesCommand
        {
            get
            {
                return reorderEntriesCommand
                    ?? (reorderEntriesCommand = new RelayCommand<string>(
                                                (sortType) =>
                                                {
                                                    ChangeSort((SortType)Enum.Parse(typeof(SortType), sortType.ToString()));
                                                }));
            }
        }

        [PreferredConstructor]
        public DomainBrowseListViewModel(IBrowseListService<Domain> browseListService, INavigationService navigationService, IFavoriteService favoriteService)
            : base(navigationService, favoriteService)
        {
            this.browseListService = browseListService;
            Entries = new ObservableCollection<Group<Domain>>();
            this.EntryType = DnDNavigator.Runtime.Model.DnDEntry.EntryType.Types.Domain;
        }

        public DomainBrowseListViewModel()
            : this(new DomainBrowseListService(), new NavigationService(), new FavoriteService())
        { }

        public async Task GetEntries()
        {
            if (Entries.Count > 0)
            {
                Entries.Clear();
            }

            var entries = await browseListService.GetEntries();

            foreach (Group<Domain> entry in entries)
            {
                Entries.Add(entry);
            }
        }

        /// <summary>
        /// Modifies the displayed grouping to the passed in SortType.
        /// </summary>
        /// <param name="sortType"></param>
        public void ChangeSort(SortType sortType)
        {
            switch (sortType)
            {
                case SortType.Name:
                    {
                        Entries = new ObservableCollection<Group<Domain>>(GroupItemsByAlphaName<Domain>(Entries, e => e.Name));
                        break;
                    }
                default:
                    {
                        Entries = new ObservableCollection<Group<Domain>>(GroupItemsByAlphaName<Domain>(Entries, e => e.Name));
                        break;
                    }
            }

            RaisePropertyChanged("Entries");
        }
    }
}

using DnD35.SRD.Navigator8.MVVMLight;
using DnD35.SRD.Navigator8.Navigation;
using DnDNavigator.Runtime.Model.Browse;
using DnDEntry = DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Sort;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DnDNavigator.Runtime.DataAccess;

namespace DnD35.SRD.Navigator8.ViewModel
{
    public class ItemBrowseListViewModel : BaseListViewModel, IBrowseListViewModel<DnDEntry.Item>
    {
        private readonly IBrowseListService<DnDEntry.Item> browseListService;

        public ObservableCollection<Group<DnDEntry.Item>> Entries { get; private set; }
        public DnDEntry.EntryType.Types EntryType { get; private set; }

        public enum SortType
        {
            [Description("Name")]
            Name,

            [Description("Category")]
            Category,

            [Description("Subcategory")]
            Subcategory
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
        public ItemBrowseListViewModel(IBrowseListService<DnDEntry.Item> browseListService, INavigationService navigationService, IFavoriteService favoriteService)
            : base(navigationService, favoriteService)
        {
            this.browseListService = browseListService;
            Entries = new ObservableCollection<Group<DnDEntry.Item>>();
            this.EntryType = DnDNavigator.Runtime.Model.DnDEntry.EntryType.Types.Item;
        }

        public ItemBrowseListViewModel()
            : this(new ItemBrowseListService(), new NavigationService(), new FavoriteService())
        { }

        public async Task GetEntries()
        {
            if (Entries.Count > 0)
            {
                Entries.Clear();
            }

            var entries = await browseListService.GetEntries();

            foreach (Group<DnDEntry.Item> entry in entries)
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
                        Entries = new ObservableCollection<Group<DnDEntry.Item>>(GroupItemsByAlphaName<DnDEntry.Item>(Entries, e => e.Name));
                        break;
                    }
                case SortType.Category:
                    {
                        Entries = new ObservableCollection<Group<DnDEntry.Item>>(GroupItemsByProperty<DnDEntry.Item>(Entries, e => e.Category));
                        break;
                    }
                case SortType.Subcategory:
                    {
                        Entries = new ObservableCollection<Group<DnDEntry.Item>>(GroupItemsByProperty<DnDEntry.Item>(Entries, e => e.Subcategory));
                        break;
                   }
                default:
                    {
                        Entries = new ObservableCollection<Group<DnDEntry.Item>>(GroupItemsByAlphaName<DnDEntry.Item>(Entries, e => e.Name));
                        break;
                    }
            }

            RaisePropertyChanged("Entries");
        }
    }
}

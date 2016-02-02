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
    public class PowerBrowseListViewModel : BaseListViewModel, IBrowseListViewModel<Power>
    {
        private readonly IBrowseListService<Power> browseListService;

        public ObservableCollection<Group<Power>> Entries { get; private set; }
        public EntryType.Types EntryType { get; private set; }

        public enum SortType
        {
            [Description("Name")]
            Name,

            [Description("Discipline")]
            Discipline,

            [Description("Display")]
            Display,

            [Description("Duration")]
            Duration,

            [Description("Level")]
            Level,

            [Description("Manifesting Time")]
            Manifesting_Time,

            [Description("Power Points")]
            Power_Points,

            [Description("Power Resistance")]
            Power_Resistance,

            [Description("Range")]
            Range
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
        public PowerBrowseListViewModel(IBrowseListService<Power> browseListService, INavigationService navigationService, IFavoriteService favoriteService)
            : base(navigationService, favoriteService)
        {
            this.browseListService = browseListService;
            Entries = new ObservableCollection<Group<Power>>();
            this.EntryType = DnDNavigator.Runtime.Model.DnDEntry.EntryType.Types.Power;
        }

        public PowerBrowseListViewModel()
            : this(new PowerBrowseListService(), new NavigationService(), new FavoriteService())
        { }

        public async Task GetEntries()
        {
            if (Entries.Count > 0)
            {
                Entries.Clear();
            }

            var entries = await browseListService.GetEntries();

            foreach (Group<Power> entry in entries)
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
                case SortType.Discipline:
                    {
                        Entries = new ObservableCollection<Group<Power>>(GroupItemsByProperty<Power>(Entries, e => e.Discipline));
                        break;
                    }
                case SortType.Display:
                    {
                        Entries = new ObservableCollection<Group<Power>>(GroupItemsByProperty<Power>(Entries, e => e.Display));
                        break;
                    }
                case SortType.Duration:
                    {
                        Entries = new ObservableCollection<Group<Power>>(GroupItemsByProperty<Power>(Entries, e => e.Duration));
                        break;
                    }
                case SortType.Level:
                    {
                        Entries = new ObservableCollection<Group<Power>>(GroupItemsByProperty<Power>(Entries, e => e.Level));
                        break;
                    }
                case SortType.Manifesting_Time:
                    {
                        Entries = new ObservableCollection<Group<Power>>(GroupItemsByProperty<Power>(Entries, e => e.Manifesting_Time));
                        break;
                    }
                case SortType.Name:
                    {
                        Entries = new ObservableCollection<Group<Power>>(GroupItemsByAlphaName<Power>(Entries, e => e.Name));
                        break;
                    }
                case SortType.Power_Points:
                    {
                        Entries = new ObservableCollection<Group<Power>>(GroupItemsByProperty<Power>(Entries, e => e.Power_Points));
                        break;
                    }
                case SortType.Power_Resistance:
                    {
                        Entries = new ObservableCollection<Group<Power>>(GroupItemsByProperty<Power>(Entries, e => e.Power_Resistance));
                        break;
                    }
                case SortType.Range:
                    {
                        Entries = new ObservableCollection<Group<Power>>(GroupItemsByProperty<Power>(Entries, e => e.Range));
                        break;
                    }
                default:
                    {
                        Entries = new ObservableCollection<Group<Power>>(GroupItemsByAlphaName<Power>(Entries, e => e.Name));
                        break;
                    }
            }

            RaisePropertyChanged("Entries");
        }
    }
}

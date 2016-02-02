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
    public class SkillBrowseListViewModel : BaseListViewModel, IBrowseListViewModel<Skill>
    {
        private readonly IBrowseListService<Skill> browseListService;

        public ObservableCollection<Group<Skill>> Entries { get; private set; }
        public EntryType.Types EntryType { get; private set; }

        public enum SortType
        {
            [Description("Name")]
            Name,

            [Description("Key Ability")]
            Key_Ability
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
        public SkillBrowseListViewModel(IBrowseListService<Skill> browseListService, INavigationService navigationService, IFavoriteService favoriteService)
            : base(navigationService, favoriteService)
        {
            this.browseListService = browseListService;
            Entries = new ObservableCollection<Group<Skill>>();
            this.EntryType = DnDNavigator.Runtime.Model.DnDEntry.EntryType.Types.Skill;
        }

        public SkillBrowseListViewModel()
            : this(new SkillBrowseListService(), new NavigationService(), new FavoriteService())
        { }

        public async Task GetEntries()
        {
            if (Entries.Count > 0)
            {
                Entries.Clear();
            }

            var entries = await browseListService.GetEntries();

            foreach (Group<Skill> entry in entries)
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
                        Entries = new ObservableCollection<Group<Skill>>(GroupItemsByAlphaName<Skill>(Entries, e => e.Name));
                        break;
                    }
                case SortType.Key_Ability:
                    {
                        Entries = new ObservableCollection<Group<Skill>>(GroupItemsByProperty<Skill>(Entries, e => e.Key_Ability));
                        break;
                    }
                default:
                    {
                        Entries = new ObservableCollection<Group<Skill>>(GroupItemsByAlphaName<Skill>(Entries, e => e.Name));
                        break;
                    }
            }

            RaisePropertyChanged("Entries");
        }
    }
}

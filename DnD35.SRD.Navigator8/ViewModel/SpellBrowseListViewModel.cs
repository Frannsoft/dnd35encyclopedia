using DnD35.SRD.Navigator8.Design;
using DnD35.SRD.Navigator8.MVVMLight;
using DnDNavigator.Runtime.Model.Browse;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Sort;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using DnD35.SRD.Navigator8.Navigation;
using DnDNavigator.Runtime.DataAccess;
using System.Text.RegularExpressions;

namespace DnD35.SRD.Navigator8.ViewModel
{
    public class SpellBrowseListViewModel : BaseListViewModel, IBrowseListViewModel<Spell>
    {
        private readonly IBrowseListService<Spell> browseListService;
        private bool isFiltered = false;

        public ObservableCollection<Group<Spell>> Entries { get; private set; }
        public EntryType.Types EntryType { get; private set; }
        public enum SortType
        {
            [Description("Name")]
            Name,

            [Description("School")]
            School,

            [Description("Casting Time")]
            Casting_Time,

            [Description("Duration")]
            Duration,

            [Description("Level")]
            Level,

            [Description("Saving Throw")]
            Saving_Throw,

            [Description("Spell Resistance")]
            Spell_Resistance,

            [Description("Subschool")]
            Subschool
        }

        public IEnumerable<SortType> SortTypes
        {
            get
            {
                return Enum.GetValues(typeof(SortType)).Cast<SortType>();
            }
        }

        private SortType activeSort;

        public enum FilterType
        {
            [Description("All")]
            All,

            [Description("Air")]
            Air,

            [Description("Animal")]
            Animal,

            [Description("Artifice")]
            Artifice,

            [Description("Bard")]
            Bard,

            [Description("Blackguard")]
            Blackguard,

            [Description("Cleric")]
            Cleric,

            [Description("Creation")]
            Creation,

            [Description("Darkness")]
            Darkness,

            [Description("Death")]
            Death,

            [Description("Destruction")]
            Destruction,

            [Description("Druid")]
            Druid,

            [Description("Earth")]
            Earth,

            [Description("Evil")]
            Evil,

            [Description("Fire")]
            Fire,

            [Description("Glory")]
            Glory,

            [Description("Good")]
            Good,

            [Description("Healing")]
            Healing,

            [Description("Knowledge")]
            Knowledge,

            [Description("Law")]
            Law,

            [Description("Luck")]
            Luck,

            [Description("Madness")]
            Madness,

            [Description("Magic")]
            Magic,

            [Description("Mind")]
            Mind,

            [Description("Paladin")]
            Paladin,

            [Description("Plant")]
            Plant,

            [Description("Protection")]
            Protection,

            [Description("Ranger")]
            Ranger,

            [Description("Repose")]
            Repose,

            [Description("Sorcerer/Wizard")]
            SorcererWizard,

            [Description("Strength")]
            Strength,

            [Description("Sun")]
            Sun,

            [Description("Travel")]
            Travel,

            [Description("Trickery")]
            Trickery,

            [Description("War")]
            War,

            [Description("Water")]
            Water,

            [Description("Wizard")]
            Wizard

        }

        public IEnumerable<FilterType> FilterTypes
        {
            get
            {
                return Enum.GetValues(typeof(FilterType)).Cast<FilterType>();
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
                                                    //ChangeSort(SortType.Name);
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

        private RelayCommand<string> filterEntriesCommand;
        public RelayCommand<string> FilterEntriesCommand
        {
            get
            {
                return filterEntriesCommand
                    ?? (filterEntriesCommand = new RelayCommand<string>(
                                                async (filterType) =>
                                                {
                                                    IsBusy = true;
                                                    await ChangeFilter((FilterType)Enum.Parse(typeof(FilterType), filterType.ToString()));
                                                    IsBusy = false;
                                                }));
            }
        }

        [PreferredConstructor]
        public SpellBrowseListViewModel(IBrowseListService<Spell> browseListService, INavigationService navigationService, IFavoriteService favoriteService)
            : base(navigationService, favoriteService)
        {
            this.browseListService = browseListService;
            Entries = new ObservableCollection<Group<Spell>>();
            this.EntryType = DnDNavigator.Runtime.Model.DnDEntry.EntryType.Types.Spell;
        }

        public SpellBrowseListViewModel()
            : this(new SpellBrowseListService(), new NavigationService(), new FavoriteService())
        {

        }

        public async Task GetEntries()
        {
            var entries = await browseListService.GetEntries();
            if (Entries.Count > 0)
            {
                Entries.Clear();
            }

            foreach (Group<Spell> entry in entries)
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
            if (isFiltered  && sortType != SortType.Level)
            {
                isFiltered = false;
            }

            //don't bother redoing the filter if it isn't different from the current one
            if (activeSort == sortType)
            { return; }


            switch (sortType)
            {
                case SortType.Duration:
                    {
                        IsFilterable = false;
                        Entries = new ObservableCollection<Group<Spell>>(GroupItemsByProperty<Spell>(Entries, e => e.Duration));
                        break;
                    }
                case SortType.Level:
                    {
                        IsFilterable = true;
                        Entries = new ObservableCollection<Group<Spell>>(GroupItemsByProperty<Spell>(Entries, e => e.Level));
                        break;
                    }
                case SortType.Casting_Time:
                    {
                        IsFilterable = false;
                        Entries = new ObservableCollection<Group<Spell>>(GroupItemsByProperty<Spell>(Entries, e => e.Casting_Time));
                        break;
                    }
                case SortType.Name:
                    {
                        IsFilterable = false;
                        Entries = new ObservableCollection<Group<Spell>>(GroupItemsByAlphaName<Spell>(Entries, e => e.Name));
                        break;
                    }
                case SortType.Saving_Throw:
                    {
                        IsFilterable = false;
                        Entries = new ObservableCollection<Group<Spell>>(GroupItemsByProperty<Spell>(Entries, e => e.Saving_Throw));
                        break;
                    }
                case SortType.Spell_Resistance:
                    {
                        IsFilterable = false;
                        Entries = new ObservableCollection<Group<Spell>>(GroupItemsByProperty<Spell>(Entries, e => e.Spell_Resistance));
                        break;
                    }
                case SortType.Subschool:
                    {
                        IsFilterable = false;
                        Entries = new ObservableCollection<Group<Spell>>(GroupItemsByProperty<Spell>(Entries, e => e.SubSchool));
                        break;
                    }

                case SortType.School:
                    {
                        IsFilterable = false;
                        Entries = new ObservableCollection<Group<Spell>>(GroupItemsByProperty<Spell>(Entries, e => e.School));
                        break;
                    }
                default:
                    {
                        IsFilterable = false;
                        Entries = new ObservableCollection<Group<Spell>>(GroupItemsByProperty<Spell>(Entries, e => e.School));
                        break;
                    }
            }

            RaisePropertyChanged("Entries");

            activeSort = sortType;
        }

        /// <summary>
        /// Filter the Entries to the specific class passed in as FilterType.  Refreshes all
        /// Entries if IsFiltered is true to ensure it starts with all Entries.
        /// </summary>
        /// <param name="filterType"></param>
        private async Task ChangeFilter(FilterType filterType)
        {
            List<Group<Spell>> tempEntries = new List<Group<Spell>>();
            IList<Group<Spell>> items = null;
            string filter = string.Empty;

            //Reset entries so that all are present in the collection
            if (isFiltered)
            {
                await GetEntries();
                isFiltered = false;
            }

            switch (filterType)
            {
                case FilterType.All:
                    {
                        //do nothing we want all entries
                        ChangeSort(SortType.Level);
                        break;
                    }
                case FilterType.SorcererWizard: //exception since a '/' is included in the db column val
                    {
                        filter = "Sorcerer/Wizard";
                        break;
                    }
                default: //just get the string of the passed in FilterType
                    {
                        filter = filterType.ToString();
                        break;
                    }
            }

            if (!string.IsNullOrEmpty(filter))
            {
                for (int i = 0; i < Entries.Count; i++)
                {
                    //get the current Group of Entries
                    Group<Spell> group = new Group<Spell>(Entries[i].Title);

                    for (int k = 0; k < Entries[i].Count; k++)
                    {
                        //get the spell at this index so we can modify it
                        Spell tempSpell = (Spell)Entries[i].ElementAt(k);
                        if (tempSpell.Level.Contains(filter))
                        {
                            //modify the level string to say what we want
                            tempSpell.Level = GetKey(tempSpell.Level, filter);
                            group.Add(tempSpell);
                        }
                    }
                    tempEntries.Add(group); //add the new Group<Spell> (and all the Spells it includes)
                }

                //Group the entries into level AND CLASS specific groups rather than mashing everything together as it is
                //in the database.
                items = GroupItemsByPropertyAndFilter<Spell>(tempEntries, e => e.Level);

                Entries.Clear();

                //readd newly filtered items to the public collection of entries for the list
                foreach (Group<Spell> group in items)
                {
                    Entries.Add(group);
                }
                RaisePropertyChanged("Entries");
                isFiltered = true; //we're now filtered
            }
        }

        /// <summary>
        /// Use a regex to look for a specific word in passed in string.  
        /// This is useful for filtering groups where the db column value brought back
        /// is not specific enough.  An example of this is the 'level' column in the spell table.
        /// It contains the levels for each spell for multiple classes.
        /// </summary>
        /// <param name="keyBeforeRegex"></param>
        /// <returns></returns>
        private static string GetKey(string keyBeforeRegex, string phraseToMatch)
        {
            //MAGIC
            string pattern = @phraseToMatch + @" [0-9][1-9]*";
            string result = string.Empty;

            result = Regex.Match(keyBeforeRegex, pattern).Value;

            return result;
        }
    }
}

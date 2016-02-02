using DnD35.SRD.Navigator8.MVVMLight;
using DnD35.SRD.Navigator8.Navigation;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Model.Search;
using DnDNavigator.Runtime.Sort;
using GalaSoft.MvvmLight.Ioc;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DnD35.SRD.Navigator8.ViewModel
{
    public class SearchViewModel : BaseListViewModel
    {
        private readonly ISearchService searchService;
        private readonly INavigationService navigationService;
        private IEnumerable<string> selectedCategories;

        public bool IsEmpty { get; private set; }

        public ObservableCollection<SearchOption> SearchOptions { get; private set; }
        public ObservableCollection<Group<IEntry>> SearchResults { get; private set; }

       

        private RelayCommand<string> getSearchResultsCommand;
        public RelayCommand<string> GetSearchResultsCommand
        {
            get
            {
                return getSearchResultsCommand
                    ?? (getSearchResultsCommand = new RelayCommand<string>(
                                                    async (query) =>
                                                    {
                                                        IsBusy = true;

                                                        await GetSearchResults(query);

                                                        IsBusy = false;
                                                    }));
            }
        }

        [PreferredConstructor]
        public SearchViewModel(ISearchService searchService, INavigationService navigationService, IFavoriteService favoriteService)
            : base(navigationService, favoriteService)
        {
            this.searchService = searchService;
            this.navigationService = navigationService;
            SearchOptions = new ObservableCollection<SearchOption>()
            {
                new SearchOption("Classes", "class"),
                new SearchOption("Domains", "domain"),
                new SearchOption("Equipment", "equipment"),
                new SearchOption("Feats", "feat"),
                new SearchOption("Items", "item"),
                new SearchOption("Monsters", "monster"),
                new SearchOption("Powers", "power"),
                new SearchOption("Skills", "skill"),
                new SearchOption("Spells", "spell"),
            };

            SearchResults = new ObservableCollection<Group<IEntry>>();
            selectedCategories = new List<string>();
        }

        public SearchViewModel() :
            this(new SearchService(), new NavigationService(), new FavoriteService())
        {
            SearchOptions = new ObservableCollection<SearchOption>()
            {
                new SearchOption("Classes", "class"),
                new SearchOption("Domains", "domain"),
                new SearchOption("Equipment", "equipment"),
                new SearchOption("Feats", "feat"),
                new SearchOption("Items", "item"),
                new SearchOption("Monsters", "monster"),
                new SearchOption("Powers", "power"),
                new SearchOption("Skills", "skill"),
                new SearchOption("Spells", "spell"),
            };
            SearchResults = new ObservableCollection<Group<IEntry>>();
            selectedCategories = new List<string>();
        }

        public string SummaryForSelectedItems(IList options)
        {
            if (options == null || options.Count == 0)
            {
                return "All ";
            }
            else
            {
                selectedCategories = (options.Cast<SearchOption>()).Select(option => option.Value);
                if (SearchResults.Count > 0)
                {
                    SearchResults.Clear();
                }
                return string.Join<string>(", ", selectedCategories).ToUpper() + " ";
            }
        }

        public async Task GetSearchResults(string userQuery)
        {
            //clear old results first so we don't stack
            if (SearchResults.Count > 0)
            {
                SearchResults.Clear();
            }

            //dump whitespaces
            userQuery = userQuery.Trim();

            List<IEntry> results = new List<IEntry>();
            string baseQuery = string.Empty;

            //if selectedCategories is null that means the user hasn't selected
            //TODO - let the user know they need to select at least one category! (or should we just query all categories...)
            if (selectedCategories.Count() > 0)
            {
                results.AddRange(await searchService.PerformSearch(userQuery, selectedCategories));
                
            }
            else
            {
                //use all categories
                selectedCategories = SearchOptions.Select(option => option.Value);
                results.AddRange(await searchService.PerformSearch(userQuery, selectedCategories));
            }

            var orderedEntries = searchService.OrderItems<IEntry>(results, e => e.Name);
            foreach (var entry in orderedEntries)
            {
                SearchResults.Add(entry);
            }

            if(GetResultTotalCount() > 0)
            {
                IsEmpty = false;
            }
            else
            {
                IsEmpty = true;
            }
            RaisePropertyChanged("IsEmpty");
        }

        /// <summary>
        /// Gets the number of results present not counting root group entries
        /// </summary>
        /// <returns></returns>
        private int GetResultTotalCount()
        {
            int result = 0;

            foreach(Group<IEntry> group in SearchResults)
            {
                foreach(IEntry entry in group)
                {
                    result++;
                }
            }

            return result;
        }
    }
}

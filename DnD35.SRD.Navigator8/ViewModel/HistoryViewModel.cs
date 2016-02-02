using DnD35.SRD.Navigator8.MVVMLight;
using DnD35.SRD.Navigator8.Navigation;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.Model.Entry;
using DnDNavigator.Runtime.Sort;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.ObjectModel;

namespace DnD35.SRD.Navigator8.ViewModel
{
    public class HistoryViewModel : BaseListViewModel
    {
        private readonly IHistoryService historyService;

        public ObservableCollection<Group<BaseEntry>> HistoryItems { get; private set; }

        private RelayCommand getHistoryItemsCommand;
        public RelayCommand GetHistoryItemsCommand
        {
            get
            {
                return getHistoryItemsCommand ?? (getHistoryItemsCommand = new RelayCommand(() =>
                    {
                        IsBusy = true;
                        GetHistoryItems();
                        IsBusy = false;
                    }));
            }
        }

        private RelayCommand clearHistoryCommand;
        public RelayCommand ClearHistoryCommand
        {
            get
            {
                return clearHistoryCommand ?? (clearHistoryCommand = new RelayCommand(() =>
                {
                    ClearHistory();
                    RaisePropertyChanged("HistoryItems");
                }));
            }
        }

        private void ClearHistory()
        {
            historyService.ClearHistory();
            GetHistoryItems(); //refresh
        }

        private RelayCommand<BaseEntry> navigateToHistoryEntryCommand;
        public RelayCommand<BaseEntry> NavigateToHistoryEntryCommand
        {
            get
            {
                return navigateToHistoryEntryCommand ?? (navigateToHistoryEntryCommand = new RelayCommand<BaseEntry>((entry) =>
                {
                    NavigateToHistoryEntry(entry);
                }));
            }
        }

        [PreferredConstructor]
        public HistoryViewModel(IHistoryService historyService, INavigationService navigationService, IFavoriteService favoriteService)
            : base(navigationService, favoriteService)
        {
            this.historyService = historyService;
            HistoryItems = new ObservableCollection<Group<BaseEntry>>();
        }

        public HistoryViewModel()
            : this(new HistoryService(), new NavigationService(), new FavoriteService())
        { }

        private void GetHistoryItems()
        {
            ClearHistoryItems();

            //returns entries grouped by alpha by default
            var historyItems = historyService.GetGroupedHistoryItems();
            foreach (Group<BaseEntry> entry in historyItems)
            {
                HistoryItems.Add(entry);
            }
        }

        private void ClearHistoryItems()
        {
            //non ui clear of history items
            HistoryItems.Clear(); 
        }

        private void NavigateToHistoryEntry(BaseEntry entry)
        {
            NavigateToEntry(entry);
        }
    }
}

using DnD35.SRD.Navigator8.Design;
using DnD35.SRD.Navigator8.MVVMLight;
using DnD35.SRD.Navigator8.Navigation;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Model.Entry;
using GalaSoft.MvvmLight.Ioc;
using System.ComponentModel;

namespace DnD35.SRD.Navigator8.ViewModel
{
    public class EntryViewModel : BaseListViewModel, INotifyPropertyChanged
    {
        private readonly IEntryService entryService;
        private readonly IHistoryService historyService;

        public BaseEntry Entry { get; private set; }

        public bool IsLicensed
        {
            get { return LicenseService.IsLicensed; }
        }

        private RelayCommand loadEntryDataCommand;
        public RelayCommand LoadEntryDataCommand
        {
            get
            {
                return loadEntryDataCommand
                    ?? (loadEntryDataCommand = new RelayCommand(
                                                () =>
                                                {
                                                    IsBusy = true;
                                                    LoadData();
                                                    IsBusy = false;
                                                }));
            }
        }

        [PreferredConstructor]
        public EntryViewModel(INavigationService navigationService, IEntryService entryService,
                        IHistoryService historyService, IFavoriteService favoriteService)
            : base(navigationService, favoriteService)
        {
            this.entryService = entryService;
            this.historyService = historyService;
        }

        public EntryViewModel()
            : this(new NavigationService(), new EntryService(), new HistoryService(), new FavoriteService())
        {
        }

        /// <summary>
        /// Load up the entry data from storage.  Will also put the entry into History after loading.
        /// </summary>
        private void LoadData()
        {
            BaseEntry entry = entryService.LoadEntryData();

            if (entry != null)
            {
                Entry = entry;
                AddEntryToHistory(entry);
                RaisePropertyChanged("Entry");
            }
            else
            {
                //TODO - add a toast letting the user know there was an issue getting the stored entry
            }
        }

        private void AddEntryToHistory(BaseEntry entry)
        {
            historyService.AddToHistory(entry);
        }
    }
}

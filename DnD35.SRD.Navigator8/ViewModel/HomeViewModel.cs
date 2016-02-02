using DnD35.SRD.Navigator8.Design;
using DnD35.SRD.Navigator8.MVVMLight;
using DnD35.SRD.Navigator8.Navigation;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Model.Menu;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace DnD35.SRD.Navigator8.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly IMenuOptionsService menuOptionsService;
        private readonly INavigationService navigationService;

        public ObservableCollection<MenuOption> MenuOptions
        {
            get;
            private set;
        }

        private MenuOption selectedOption;
        public MenuOption SelectedOption
        {
            get { return selectedOption; }
            set
            {
                if (selectedOption == value)
                {
                    return;
                }

                selectedOption = value;
                RaisePropertyChanged("SelectedOption");
            }
        }

        private RelayCommand refreshHomeMenuCommand;
        public RelayCommand RefreshHomeMenuCommand
        {
            get
            {
                return refreshHomeMenuCommand
                    ?? (refreshHomeMenuCommand = new RelayCommand(
                                                 async () =>
                                                 {
                                                     await RefreshHomeMenu();
                                                 }));
            }
        }

        private RelayCommand<MenuOption> navigateToSelectedOption;
        public RelayCommand<MenuOption> NavigateToSelectedOption
        {
            get
            {
                return navigateToSelectedOption
                    ?? (navigateToSelectedOption = new RelayCommand<MenuOption>(
                                                    (option) =>
                                                    {
                                                        NavigateToPage(option);
                                                    }));
            }
        }

        [PreferredConstructor]
        public HomeViewModel(IMenuOptionsService menuOptionsService, INavigationService navigationService)
        {
            this.menuOptionsService = menuOptionsService;
            this.navigationService = navigationService;
            MenuOptions = new ObservableCollection<MenuOption>();
        }

        public HomeViewModel()
            : this(
                DesignerProperties.IsInDesignTool
                ? (IMenuOptionsService)new DesignMenuOptionsService()
                : new MenuOptionsService(),
                new NavigationService())
        {}

        private void NavigateToPage(MenuOption option)
        {
            switch (option.EntryType)
            {
                case EntryType.Types.Search:
                    {
                        navigationService.NavigateTo(new Uri("/SearchPage.xaml", UriKind.Relative));
                        break;
                    }
                default:
                    {
                        navigationService.NavigateTo(new Uri("/BrowseListPage.xaml?entryType=" + option.EntryType, UriKind.Relative));
                        break;
                    }
            }
        }

        private async Task RefreshHomeMenu()
        {
            MenuOptions.Clear();
            var options = await menuOptionsService.RefreshHomeMenu();

            foreach (MenuOption option in options)
            {
                MenuOptions.Add(option);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

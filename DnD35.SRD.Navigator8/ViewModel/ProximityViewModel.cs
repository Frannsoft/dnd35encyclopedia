using DnD35.SRD.Navigator8.MVVMLight;
using DnD35.SRD.Navigator8.Navigation;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.Proximity;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.ComponentModel;
using System.Windows;

namespace DnD35.SRD.Navigator8.ViewModel
{
    public class ProximityViewModel : BaseListViewModel, INotifyPropertyChanged
    {
        private readonly INavigationService navigationService;

        public ProximityProcess ProximityProcess { get; set; }

        [PreferredConstructor]
        public ProximityViewModel(INavigationService navigationService, IFavoriteService favoriteService)
            : base(navigationService, favoriteService)
        {
            this.navigationService = navigationService;
            this.ProximityProcess = new ProximityProcess();
            this.ProximityProcess.PropertyChanged += ProximityProcess_PropertyChanged;
        }

        public ProximityViewModel()
            : this(new NavigationService(), null)
        { }

        private RelayCommand<bool> configureProximityCommand;
        public RelayCommand<bool> ConfigureProximityCommand
        {
            get
            {
                return configureProximityCommand
                   ?? (configureProximityCommand = new RelayCommand<bool>((value) =>
                        {
                            IsBusy = true;
                            InitiateShareProcedure(value);
                            IsBusy = false;
                        }));
            }
        }

        private RelayCommand cancelProximityCommand;
        public RelayCommand CancelProximityCommand
        {
            get
            {
                return cancelProximityCommand
                    ?? (cancelProximityCommand = new RelayCommand(() =>
                    {
                        CancelShare();
                    }));
            }
        }

        private void CancelShare()
        {
            ProximityProcess.StopProximityProcess();
        }

        private void InitiateShareProcedure(bool isHost)
        {
            UpdateInformation("Initiating...");

            try
            {
                ProximityProcess.InitializeProximity(isHost);
            }catch(InvalidOperationException)
            {
                throw;
            }
        }

        private void ClearInformation()
        {
            ProximityProcess.ClearInformation();
        }

        private void UpdateInformation(string message)
        {
            ProximityProcess.UpdateInformation(message);
        }

        private void ProximityProcess_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("HasReceivedData"))
            {
                if (ProximityProcess.HasReceivedData)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        navigationService.NavigateTo(new Uri("/EntryPage.xaml", UriKind.Relative));
                    });

                }
            }
            if (e.PropertyName.Equals("HasCompletedSendingData"))
            {
                if (ProximityProcess.HasCompletedSendingData)
                {
                    navigationService.GoBack();
                }
            }
        }
    }
}

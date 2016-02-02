using System.Windows;
using Microsoft.Phone.Controls;
using DnD35.SRD.Navigator8.ViewModel;
using System.Windows.Navigation;
using System.Windows.Controls;
using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace DnD35.SRD.Navigator8
{
    /// <summary>
    /// Handles Proximity sharing of an Entry
    /// </summary>
    public partial class SharePage : PhoneApplicationPage
    {
        private bool isHost;

        #region Constructors
        /// <summary>
        /// Send other value.
        /// </summary>
        /// <param name="_sendValue"></param>
        public SharePage()
        {
            InitializeComponent();
        }
        #endregion

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ProximityViewModel;
            if (vm != null)
            {
                try
                {
                    vm.ConfigureProximityCommand.Execute(isHost);
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("Unfortunately I can't detect that your device has NFC capabilities.  \nMaybe you need to turn NFC on?");
                    NavigationService.GoBack();
                }
            }
        }

        private bool IsHost(IDictionary<string, string> queryString)
        {
            bool result = false;
            string resultAsString = string.Empty;

            if (queryString.TryGetValue("IsHost", out resultAsString))
            {
                if (!string.IsNullOrEmpty(resultAsString))
                {
                    result = Convert.ToBoolean(resultAsString);
                }
            }
            return result;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            isHost = IsHost(this.NavigationContext.QueryString);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var vm = DataContext as ProximityViewModel;
            vm.CancelProximityCommand.Execute(null);
            base.OnNavigatedFrom(e);
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            var vm = DataContext as ProximityViewModel;
            vm.CancelProximityCommand.Execute(null);

            if(!isHost)
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            else
            {
                base.OnBackKeyPress(e);
            }
        }
    }
}
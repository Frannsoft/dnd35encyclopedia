using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace DnD_3._5_SRD_Navigator7
{
    /// <summary>
    /// Contains the browser control that displays the available html.
    /// </summary>
    public partial class ListHitViewer : PhoneApplicationPage
    {
        public ListHitViewer()
        {
            InitializeComponent();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            e.Cancel = true;

            NavigationService.GoBack();//go back to the list
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            ResultsBrowserControl.NavigateToString(MainPage.currentHTML);
        }
    }
}
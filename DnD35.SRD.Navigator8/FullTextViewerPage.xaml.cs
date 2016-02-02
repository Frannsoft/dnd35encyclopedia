using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DnDNavigator.Runtime.Model.Entry;
using DnDNavigator.Runtime.DataAccess;
using System.ComponentModel;

namespace DnD35.SRD.Navigator8
{
    public partial class FullTextViewerPage : PhoneApplicationPage
    {
        public FullTextViewerPage()
        {
            InitializeComponent();
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
        }

        /// <summary>
        /// When the browser control is loaded.  Gets the stored entry's full text and displays it.
        /// If there is no data to display a messagebox will be shown and then the user will be taken Back via NavigationService.GoBack().
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FullTextWebBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            string html = GetFullText();

            if(!string.IsNullOrEmpty(html))
            {
                FullTextWebBrowser.NavigateToString(html);
            }
            else
            {
                MessageBox.Show("Unable to show full text data.  It might not exist.  \nSorry!");
                this.NavigationService.GoBack();
            }
        }

        /// <summary>
        /// Gets the stored entry's full text as a string.
        /// </summary>
        /// <returns>string</returns>
        private string GetFullText()
        {
            BaseEntry entry = IsolatedStorage.TempEntry;
            string result = string.Empty;

            if(entry != null && !string.IsNullOrEmpty(entry.Full_Text))
            {
                result = entry.Full_Text;
            }

            return result;
        }
    }
}
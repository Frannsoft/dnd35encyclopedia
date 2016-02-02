using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DnDNavigator.Runtime.Model;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.Feedback;

namespace DnD35.SRD.Navigator8
{
    /// <summary>
    /// Contains the browser control that displays the available html.
    /// </summary>
    public partial class ListHitViewer : PhoneApplicationPage
    {
        public Windows.Networking.Proximity.ProximityDevice Device { get; private set; }
        private OldItem _item;
        private bool isFromNFC = false;
        private DBHelper db = null;

        public ListHitViewer()
        {
            InitializeComponent();
            db = new DBHelper();

            //load up the app bar
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            //get the item from the phone state!  woo!
            if (PhoneApplicationService.Current.State.ContainsKey("CurrentItem") && PhoneApplicationService.Current.State["CurrentItem"] != null)
            {
                _item = PhoneApplicationService.Current.State["CurrentItem"] as OldItem;

                //only show share if the _item is not null
                ApplicationBarIconButton btnShareAppBar = new ApplicationBarIconButton();
                btnShareAppBar.IconUri = new Uri("/Assets/AppBarIcons/share.png", UriKind.Relative);
                btnShareAppBar.Text = "Tap To Share";
                ApplicationBar.Buttons.Add(btnShareAppBar);
                btnShareAppBar.Click += new EventHandler(btnShareAppBar_Click);
            }

            //feedback button
            ApplicationBarIconButton btnFeedbackAppBar = new ApplicationBarIconButton();
            btnFeedbackAppBar.IconUri = new Uri("/Assets/AppBarIcons/questionmark.png", UriKind.Relative);
            btnFeedbackAppBar.Text = "Feedback";
            ApplicationBar.Buttons.Add(btnFeedbackAppBar);
            btnFeedbackAppBar.Click += new EventHandler(btnFeedbackAppBar_Click);
        }

        private void btnFeedbackAppBar_Click(object sender, EventArgs e)
        {
            //Compose an email for feedback
            new FeedbackEmail();
        }

        private void btnShareAppBar_Click(object sender, EventArgs e)
        {
            initiateProximityOperation(_item);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            e.Cancel = true;
            if (isFromNFC)
            {
                //go to search page
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            else
            {
                NavigationService.GoBack();//go back to the list
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //if this was opened up from an NFC share, otherwise it's a normal operation
            if (NavigationContext.QueryString.ContainsKey("IsFromNFC"))
            {
                isFromNFC = Convert.ToBoolean(NavigationContext.QueryString["IsFromNFC"]);
            }

        }

        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            ResultsBrowserControl.NavigateToString(MainPage.currentHTML);
        }

        /// <summary>
        /// Called when users wants to share an Item with another user from a search term or browser window.
        /// </summary>
        private void initiateProximityOperation(OldItem _item)
        {
            if (NFCIsSupportedOnDevice())
            {
                string itemName = _item.name.Trim();
                itemName = Uri.EscapeDataString(itemName);
                MainPage.currentHTML = retrieveHistoryOrFavoriteResult(_item.ItemSQL);
                if (!itemName.Contains("<script>") && !itemName.Contains("http://") && !itemName.Contains("/"))
                {
                    NavigationService.Navigate(new Uri("/ProximityInfo.xaml?Name=" + itemName + "&IsHost=true", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Invalid item detected.  Unable to initiate share operation.");
                }
            }
        }

        private string retrieveHistoryOrFavoriteResult(string itemSQL)
        {
            string fullHTML = string.Empty;
            OldItem item = db.Select(itemSQL); //execute query for results

            if (item != null)
            {
                if (item.full_text != null && item.full_text.Length > 0)
                {
                    fullHTML = item.full_text;
                }
                else
                {
                    MessageBox.Show("No Web Page currently exists for this item right now.  Sorry!");
                    return string.Empty;
                }
                return fullHTML;
            }
            else
            {
                MessageBox.Show("No Results found. ");
                return string.Empty;
            }
        }

        private bool NFCIsSupportedOnDevice()
        {
            Device = Windows.Networking.Proximity.ProximityDevice.GetDefault();
            if (Device != null)
            {
                return true;
            }
            else
            {
                System.Windows.MessageBox.Show("Uh-oh!  NFC does not seem to be supported or turned on for this device!  \n\n You can turn it on in Settings if your phone supports it.");
                return false;
            }
        }
    }
}
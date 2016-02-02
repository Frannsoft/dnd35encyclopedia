using Microsoft.Phone.Controls;
using System;
using System.Windows;

namespace DnD35.SRD.Navigator8.Navigation
{
    public class NavigationService : INavigationService
    {
        private PhoneApplicationFrame mainFrame;

        public void GoBack()
        {
            if (IsMainFrame() && mainFrame.CanGoBack)
            {
                mainFrame.GoBack();
            }
        }

        public void NavigateTo(Uri uri)
        {
            if (IsMainFrame())
            {
                mainFrame.Navigate(uri);
            }
        }

        private bool IsMainFrame()
        {
            if (mainFrame != null)
            {
                return true;
            }
            mainFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            return mainFrame != null;
        }
    }
}

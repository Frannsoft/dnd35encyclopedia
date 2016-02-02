using Microsoft.Phone.Controls;
using DnD35.SRD.Navigator8.ViewModel;
using System.Windows.Input;

namespace DnD35.SRD.Navigator8
{
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();

            var vm = (SearchViewModel)DataContext;
            SearchTypeListPicker.SummaryForSelectedItemsDelegate = vm.SummaryForSelectedItems;
        }

        private void SearchPhoneKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string userQuery = SearchPhoneTextBox.Text;
                if (!string.IsNullOrWhiteSpace(userQuery))
                {
                    var vm = (SearchViewModel)DataContext;
                    vm.GetSearchResultsCommand.Execute(userQuery);
                    SearchResultsLongListSelector.Focus();
                }
            }
        }
    }
}
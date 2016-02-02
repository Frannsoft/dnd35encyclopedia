using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Collections.ObjectModel;
using System.Reflection;
using System.ComponentModel;
using System.Collections;
using DnD_3._5_SRD_Navigator7.DataAccess;

namespace DnD_3._5_SRD_Navigator7
{
    /// <summary>
    /// Main Application window.
    /// </summary>
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        /// <summary>
        /// Holds the current retrieved result's html for the browser control.  The only reason this is a global variable is because I was running into 'shell page uri too long' errors since I was
        //was previously passing in the entire html string via a paramater in the NavigationService.Navigate call.
        /// </summary>
        public static string currentHTML = "";

        public string currentTable = String.Empty;

        IsoStorageHelper isoHelper;
        HistoryHandler hHandler = null;

        private bool historyListLoaded = false;  //stores whether or not the history is currently loaded.  

        private ObservableCollection<Model.Item> _items = new ObservableCollection<Model.Item>();
        public ObservableCollection<Model.Item> AllItems
        {
            get { return _items; }
            set { _items = value; }
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            isoHelper = new IsoStorageHelper();
            hHandler = new HistoryHandler();
            isoHelper.checkIfDBFileUpdateNeeded();

            //move to Search Pivot by default
            MasterPivot.SelectedItem = SearchPivot;
        }

        private string retrieveHistoryOrFavoriteResult(string itemSQL)
        {
            string fullHTML = string.Empty;
            Model.Item item = (Application.Current as App).db.Select(itemSQL); //execute query for results

            if (item != null)
            {
                if (browserControl.Visibility == System.Windows.Visibility.Collapsed)
                {
                    browserControl.Visibility = System.Windows.Visibility.Visible;
                }
                if (item.HTML != null && item.HTML.Length > 0)
                {
                    fullHTML = item.HTML;
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

        /// <summary>
        /// Use the passed in user entry text from the search bar and search the database for a result.  Then return the full_text html of the result if the column has data in it.
        /// </summary>
        /// <param name="usersEntry"></param>
        /// <returns></returns>
        private string retrieveResult(string usersEntry)
        {
            string fullHTML = string.Empty;

            bool noTableSelected = true;
            string tableValue = getCategory(out noTableSelected);
            if (!noTableSelected)
            {
                MessageBox.Show("No Table selected in Categories.  Defaulting to Monster."); //has already been set to Monster in getCategory()
            }
           
            //Select only 1 item from database!
            string strSelect = @"SELECT id, name, full_text FROM " + tableValue + " WHERE name LIKE " + usersEntry + " LIMIT 1 ";

            Model.Item item = (Application.Current as App).db.Select(strSelect); //execute query for results
           

            if (item != null)
            {
                item.ItemSQL = strSelect; //add the sql to get the item to the item's properties for the history list.
                if (browserControl.Visibility == System.Windows.Visibility.Collapsed)
                {
                    browserControl.Visibility = System.Windows.Visibility.Visible;
                }
                if (item.HTML != null && item.HTML.Length > 0)
                {
                   fullHTML = item.HTML;//set the result to global HTML variable
                   hHandler.addtoHistory(item);
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

        /// <summary>
        /// Search button is clicked on the Search pivot screen.  The user's entry is passed into a query and the result (if one) is returned.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text.Length > 0)
            {

                string fullHTML = retrieveResult(@"""" + searchTextBox.Text.Trim() + @"""");
                if (fullHTML != string.Empty)
                {
                    browserControl.NavigateToString(fullHTML);
                    browserControl.Focus(); //give focus to the browser control.  This should remove the soft keyboard.
                }
            }
            else
            {
                MessageBox.Show("Please enter a search term.");
            }
        }

        /// <summary>
        /// An previously searched item is pressed in the list and the result is searched for in the database and displayed to the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void historyTextBox_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            
            //enter the text value of the tapped item into the search box and search.
            Model.Item selectedItem = (Model.Item)HistoryListBox.SelectedItem;
            string fullHTML = retrieveHistoryOrFavoriteResult(selectedItem.ItemSQL);
            if (fullHTML != string.Empty)
            {
                browserControl.NavigateToString(fullHTML);
            }
            HistoryListBox.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// On tapping the textblock in the Browse list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //go to a web browser control with the html string passed in
            try
            {
                Model.Item selectedItem = (Model.Item)BrowseListBox.SelectedItem;
                string fullHTML = retrieveHistoryOrFavoriteResult(selectedItem.ItemSQL);
                if (fullHTML != string.Empty)
                {
                    currentHTML = fullHTML; //set the html to be the selected result
                    NavigationService.Navigate(new Uri("/ListHitViewer.xaml", UriKind.Relative));
                }
            }
            catch (InvalidCastException ice)
            {
                MessageBox.Show("Unable to load page for item: " + "\n" + ice.Message + "\n" + ice.StackTrace);
            }
        }


        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                //go to a web browser control with the html string passed in
                Model.Item selectedItem = (Model.Item)FavoritesListBox.SelectedItem;

                //todo - find the item in the favorites list so we can get the sql.
                FavoriteHandler fHandler = new FavoriteHandler();
                Model.Item item = null;
                try
                {
                    item = (Model.Item)fHandler.favorites.First(Item => Item.ItemName.Equals(selectedItem.ItemName));
                }
                catch (ArgumentNullException ane)
                {
                    MessageBox.Show("Error loading up Favorited Item: " + selectedItem.ItemName + "\n " + ane.Message + "||\n" + ane.StackTrace);
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("Error loading up Favorited Item: " + selectedItem.ItemName + "\n " + ioe.Message + "||\n" + ioe.StackTrace);
                }

                //we use history result here because we stored the sql when adding the item as a favorite.

                string fullHTML = retrieveHistoryOrFavoriteResult(item.ItemSQL);
                if (fullHTML != string.Empty)
                {
                    currentHTML = fullHTML; //set the html to be the selected result
                    NavigationService.Navigate(new Uri("/ListHitViewer.xaml", UriKind.Relative));
                }
            }
            catch (InvalidCastException ice)
            {
                MessageBox.Show("Unable to load page for item: " + "\n" + ice.Message + "\n" + ice.StackTrace);
            }
        }

        private void Pivot_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //BrowsePivot selected
            if (MasterPivot.SelectedItem == BrowsePivot)
            {
                //set focus to this to remove soft keyboard from screen (from Search page)
                this.Focus();

                bool noTableSelected = true;
                string tableValue = getCategory(out noTableSelected);
                if (!noTableSelected)
                {
                    MessageBox.Show("No Table selected in Categories.  Defaulting to Monster."); //has already been set to Monster in getCategory()
                }
                string strSelect = @"SELECT name FROM " + tableValue + " ORDER BY name ASC";//need to check for empty entry
                List<Model.Item> _ItemEntries = (Application.Current as App).db.SelectBrowse(strSelect); //execute query for results

                //add the sql property for each item.  This query will be used when the specific item is clicked.
                foreach(Model.Item i in _ItemEntries.ToList<Model.Item>())
                {
                    i.ItemSQL = @"SELECT id, name, full_text FROM " + tableValue + " WHERE name LIKE " + @"""" + i.ItemName + @"""" + " LIMIT 1";
                }

                BrowseListBox.ItemsSource = new ListHelper().populateList(_ItemEntries);
            }
            else if (MasterPivot.SelectedItem == FavoritePivot)
            {
                //set focus to this to remove soft keyboard from screen (from Search page)
                this.Focus();

                //set the source
                FavoriteHandler fHandler = new FavoriteHandler();
                FavoritesListBox.ItemsSource = new ListHelper().populateList(fHandler.favorites);
            }
            //SearchPivot selected
            else if (MasterPivot.SelectedItem == SearchPivot)
            {
                searchTextBox.Focus();

                //Collapse the browser control so the user doesn't see a blank white page.
                if (browserControl.Visibility == System.Windows.Visibility.Visible)
                {
                    browserControl.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
           
            else
            {
                //set focus to this to remove soft keyboard from screen (from Search page)
                this.Focus();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(null, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void btnSearch_Loaded(object sender, RoutedEventArgs e)
        {
            ImageBrush brush = new ImageBrush();
            brush.Stretch = Stretch.UniformToFill;

            if (DarkThemeUsed())
            {
                brush.ImageSource = new BitmapImage(new Uri(@"Assets\appbar.feature.search.rest.dark.png", UriKind.Relative));
                btnSearch.Background = brush;
            }
            else if (LightThemeUsed())
            {
                brush.ImageSource = new BitmapImage(new Uri(@"Assets\appbar.feature.search.rest.light.png", UriKind.Relative));
                btnSearch.Background = brush;
            }
        }

        /// <summary>
        /// Get the table to search in the database.
        /// </summary>
        /// <param name="isTableSelected"></param>
        /// <returns></returns>
        private string getCategory(out bool isTableSelected)
        {
            if ((bool)radioMonster.IsChecked)
            {
                isTableSelected = true;
                return "monster";
            }
            else if ((bool)radioSpell.IsChecked)
            {
                isTableSelected = true;
                return "spell";
            }
            else if ((bool)radioClass.IsChecked)
            {
                isTableSelected = true;
                return "class";
            }
            else if ((bool)radioDomain.IsChecked)
            {
                isTableSelected = true;
                return "domain";
            }
            else if ((bool)radioEquipment.IsChecked)
            {
                isTableSelected = true;
                return "equipment";
            }
            else if ((bool)radioFeat.IsChecked)
            {
                isTableSelected = true;
                return "feat";
            }
            else if ((bool)radioItem.IsChecked)
            {
                isTableSelected = true;
                return "item";
            }
            else if ((bool)radioPower.IsChecked)
            {
                isTableSelected = true;
                return "power";
            }
            else if ((bool)radioSkill.IsChecked)
            {
                isTableSelected = true;
                return "skill";
            }
            else
            {//in wp8 an exception is thrown when displaying the messagebox if it's displayed here.  Moved to Pivot_SelectionChanged method
                isTableSelected = false;
                radioMonster.IsChecked = true;
                return "monster";
            }
        }

        /// <summary>
        /// Check to see if the dark theme is being used.  True if yes.
        /// </summary>
        /// <returns></returns>
        bool DarkThemeUsed()
        {
            return Visibility.Visible == (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"];
        }

        /// <summary>
        /// Check to see if the dark theme is being used.  True if yes.
        /// </summary>
        /// <returns></returns>
        bool LightThemeUsed()
        {
            return Visibility.Visible == (Visibility)Application.Current.Resources["PhoneLightThemeVisibility"];
        }

        /// <summary>
        /// Catch when the SearchPivot is loaded and prepare the search history for this session.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchPivot_Loaded(object sender, RoutedEventArgs e)
        {
            Object val;
            if (isoHelper.settings.TryGetValue<Object>(Constants.IsolatedStorage.ISO_PREVIOUS_SEARCHES, out val))
            {
                if (val != null)
                {
                    //populate the list in the search pivot with all found previous searches
                    hHandler.searchHistory = (List<Model.Item>)isoHelper.settings[Constants.IsolatedStorage.ISO_PREVIOUS_SEARCHES];
                }
            }
            else
            {
                isoHelper.storeValues(Constants.IsolatedStorage.ISO_PREVIOUS_SEARCHES, hHandler.searchHistory); //this will create the key if it does not exist
            }
        }

        /// <summary>
        /// Detect when the text is changed in the search text box.  If empty display any available search queries in the history.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //There is s bug in WP7 that causes TextBox TextChanged event to be fired twice instead of once, as is expected.  The known fix for this is to remove the secondary textbox in the TextBox control
            //by editing the TextBox Template (most easily in Blend).  Not going to address this issue at this time.

            TextBox textBox = sender as TextBox;
            HistoryHandler hHandler = new HistoryHandler();
            if (textBox.Text.Length == 0)//only do this when the search box is empty
            {
                if (!historyListLoaded && hHandler.searchHistory.Count > 0)
                {
                    browserControl.Visibility = System.Windows.Visibility.Collapsed;
                    HistoryListBox.Visibility = System.Windows.Visibility.Visible;

                    //get the history
                    HistoryListBox.ItemsSource = hHandler.searchHistory;
                    historyListLoaded = !historyListLoaded;
                }
            }
            else
            {
                if (historyListLoaded == true)
                {
                    historyListLoaded = false;
                    browserControl.Visibility = System.Windows.Visibility.Collapsed;
                    HistoryListBox.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// If enter is pressed in search text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (searchTextBox.Text.Length > 0)
                {

                    string fullHTML = retrieveResult(@"""" + searchTextBox.Text.Trim() + @"""");
                    if (fullHTML != string.Empty)
                    {
                        browserControl.NavigateToString(fullHTML);
                        browserControl.Focus(); //give focus to the browser control.  This should remove the soft keyboard.
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a search term.");
                }
            }
        }

        /// <summary>
        /// Attempt to add the selected item to the list of favorites.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextFavorite_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (sender as MenuItem).DataContext;
            Model.Item item = null;
            try
            {
                item = (Model.Item)menuItem;
            }
            catch (InvalidCastException ice)
            {
                MessageBox.Show("Unable to add item to favorites: " + "\n" + ice.Message + "\n" + ice.StackTrace);
                return;
            }

            //The items in the browse list are BrowseItem objects, meaning the html associated with them is retrieved after the user clicks on one.  
            //This means we must run a query to get the html.  This is good for performance because it means that massive strings containing HTML won't be retrieved for every list item
            //as soon as the item is loaded.
            string fullHTML = retrieveResult(@"""" + item.ItemName + @""""); //escape commas and crap

            if (fullHTML.Length > 0)
            {
                //We want to add a Model.Item object to the list of favorites so a new instance will need to be created here, using the retrieved html.
                FavoriteHandler fHandler = new FavoriteHandler();
                bool noTableSelected = true;
                string tableValue = getCategory(out noTableSelected);
                if (!noTableSelected)
                {
                    MessageBox.Show("No Table selected in Categories.  Defaulting to Monster."); //has already been set to Monster in getCategory()
                }
                //Select only 1 item from database!
                string strSelect = @"SELECT id, name, full_text FROM " + tableValue + " WHERE name LIKE " + @"""" + item.ItemName + @"""" + " LIMIT 1 ";

                fHandler.addtoFavorites(new Model.Item(item.ItemName, strSelect, fullHTML));
            }
            else //Why would anyone add an item like this anyways - just say no.
            {
                MessageBox.Show("No page exists for this item.  Unable to add to favorites.");
            }

        }

        private void contextFavoriteDelete_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = (sender as MenuItem).DataContext;
            Model.Item item = null;
            try
            {
                item = (Model.Item)menuItem;
            }
            catch (InvalidCastException ice)
            {
                MessageBox.Show("Unable to remove item from favorites: " + "\n" + ice.Message + "\n" + ice.StackTrace);
                return;
            }

            FavoriteHandler fHandler = new FavoriteHandler();
            foreach(Model.Item i in fHandler.favorites.ToList())
            {
                if (item.ItemName.Equals(i.ItemName))
                {
                    fHandler.favorites.Remove(i);//remove the item
                }
            }
            FavoritesListBox.ItemsSource = new ListHelper().populateList(fHandler.favorites);
        }
    }
}
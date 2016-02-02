using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DnD_3._5_SRD_Navigator8.DataAccess
{
    /// <summary>
    /// Handles interactions with the list of user Favorites.
    /// </summary>
    class FavoriteHandler : INotifyPropertyChanged
    {

        private List<Model.Item> _favorite = new List<Model.Item>();

        public List<Model.Item> favorites
        {
            get { return _favorite; }
            set { _favorite = value; }
        }

        public FavoriteHandler()
        {
            //get existing favorites if they exist - otherwise, create the iso key
            DataAccess.IsoStorageHelper iHelper = new IsoStorageHelper();
            if (iHelper.settings.Contains(Constants.IsolatedStorage.ISO_FAVORITES))
            {
                _favorite = (List<Model.Item>)iHelper.settings[Constants.IsolatedStorage.ISO_FAVORITES];
            }
            else
            {
                iHelper.settings[Constants.IsolatedStorage.ISO_FAVORITES] = _favorite;
                
            }
        }

        /// <summary>
        /// Add the passed in item to the favorites list if it isn't already in there.
        /// </summary>
        /// <param name="_item"></param>
        public void addtoFavorites(Model.Item _item)
        {
            if (!isInFavorites(_item))//item does not exist in history, so add it
            {
                favorites.Add(_item);
                new DataAccess.IsoStorageHelper().storeValues(Constants.IsolatedStorage.ISO_FAVORITES, favorites);
            }
        }

        /// <summary>
        /// Checks to see if the passed in Item already exists in the Favorites list.
        /// </summary>
        /// <param name="_item"></param>
        /// <returns></returns>
        public bool isInFavorites(Model.Item _item)
        {
            bool inFavorites = false;
            foreach (Model.Item item in favorites)
            {
                if (item.name.Equals(_item.name))
                {
                    inFavorites = true;
                    break; //we're done here, exit the loop
                }
            }
            return inFavorites;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(null, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

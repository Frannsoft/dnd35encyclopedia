using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Model.Entry;
using System.Collections.Generic;
using System.ComponentModel;

namespace DnDNavigator.Runtime.DataAccess
{
    /// <summary>
    /// Handles interactions with the list of user Favorites.
    /// </summary>
    //public class FavoriteHandler : INotifyPropertyChanged
    //{

    //    private List<BaseEntry> favorite = new List<BaseEntry>();

    //    public List<BaseEntry> Favorites
    //    {
    //        get { return favorite; }
    //        set { favorite = value; }
    //    }

    //    public FavoriteHandler()
    //    {
    //        //get existing favorites if they exist - otherwise, create the iso key
    //        DataAccess.IsoStorageHelper iHelper = new IsoStorageHelper();
    //        if (iHelper.Settings.Contains(Constants.IsolatedStorage.ISO_FAVORITES))
    //        {
    //            favorite = (List<BaseEntry>)iHelper.Settings[Constants.IsolatedStorage.ISO_FAVORITES];
    //        }
    //        else
    //        {
    //            iHelper.Settings[Constants.IsolatedStorage.ISO_FAVORITES] = favorite;

    //        }
    //    }

    //    /// <summary>
    //    /// Add the passed in item to the favorites list if it isn't already in there.
    //    /// </summary>
    //    /// <param name="item"></param>
    //    public void AddtoFavorites(BaseEntry item)
    //    {
    //        if (!IsInFavorites(item))//item does not exist in history, so add it
    //        {
    //            Favorites.Add(item);
    //            new DataAccess.IsoStorageHelper().StoreValues(Constants.IsolatedStorage.ISO_FAVORITES, Favorites);
    //        }
    //    }

    //    /// <summary>
    //    /// Checks to see if the passed in Item already exists in the Favorites list.
    //    /// </summary>
    //    /// <param name="_item"></param>
    //    /// <returns></returns>
    //    private bool IsInFavorites(IEntry item)
    //    {
    //        bool inFavorites = false;
    //        foreach (IEntry _item in Favorites)
    //        {
    //            if (_item.Name.Equals(item.Name))
    //            {
    //                inFavorites = true;
    //                break;
    //            }
    //        }
    //        return inFavorites;
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public void OnPropertyChanged(string propertyName)
    //    {
    //        if (PropertyChanged != null)
    //        {
    //            PropertyChanged(null, new PropertyChangedEventArgs(propertyName));
    //        }
    //    }
    //}
}

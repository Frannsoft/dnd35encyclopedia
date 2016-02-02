using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Windows;
using Windows.Storage;
using Windows.ApplicationModel;
using Microsoft.Phone.Controls;
using DnDNavigator.Runtime.Model.Entry;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Model.Playlists;
using DnDNavigator.Runtime.Sort;

namespace DnDNavigator.Runtime.DataAccess
{
    /// <summary>
    /// Used to interact with IsolatedStorage on the Phone.
    /// </summary>
    //public class IsoStorageHelper
    //{

    //    public IsolatedStorageSettings Settings;

    //    private string savedExceptionData = string.Empty;

    //    public IsoStorageHelper()
    //    {
    //        Settings = IsolatedStorageSettings.ApplicationSettings;           
    //    }

    //    public void ShowExceptionMessageIfPresent()
    //    {
    //        //check if an unhandled exception occured last time
    //        if (GetExceptionFromIsoStorage())//if yes
    //        {
    //            Deployment.Current.Dispatcher.BeginInvoke(() =>
    //            {
    //                CustomMessageBox messageBox = new CustomMessageBox()
    //                {
    //                    Caption = "Uh-oh!  There was an error!",
    //                    Message = "Would you like to submit the error information to the developer? \n It would help speed up the process of fixing the issue!",
    //                    LeftButtonContent = "Yes",
    //                    RightButtonContent = "Nope",
    //                };
    //                messageBox.Dismissed += (s1, e1) =>
    //                {
    //                    switch (e1.Result)
    //                    {
    //                        case CustomMessageBoxResult.LeftButton:
    //                            {
    //                                //compose email
    //                                new Feedback.FeedbackEmail(savedExceptionData);
    //                                break;
    //                            }
    //                        case CustomMessageBoxResult.RightButton:
    //                            {
    //                                break;
    //                            }
    //                        case CustomMessageBoxResult.None:
    //                            {
    //                                break;
    //                            }
    //                        default:
    //                            {
    //                                break;
    //                            }
    //                    }
    //                };

    //                messageBox.Show();
    //            });

    //            Settings.Remove(Constants.IsolatedStorage.ISO_EXCEPTION);//clear the exception
    //        }
    //    }

    //    public void StoreEntry(BaseEntry entry)
    //    {
    //        if (Settings.Contains(Constants.IsolatedStorage.ISO_SELECTED_ENTRY))
    //        {
    //            //remove the old prior to adding the new
    //            Settings.Remove(Constants.IsolatedStorage.ISO_SELECTED_ENTRY);
    //        }
    //        Settings.Add(Constants.IsolatedStorage.ISO_SELECTED_ENTRY, entry);
    //    }

    //    /// <summary>
    //    /// Removes the passed in entry from the settings if the Key is present.
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="entry"></param>
    //    public void RemoveEntryFromSettings(string key, BaseEntry entry)
    //    {
    //        if(Settings.Contains(key))
    //        {
    //            List<BaseEntry> items = new List<BaseEntry>();
    //            items = (List<BaseEntry>)Settings[key];
    //            if(items.Contains(entry))
    //            {
    //                items.Remove(entry);
    //                Settings.Save();
    //            }
    //        }
    //    }

    //    public void ClearAllForValue(string key)
    //    {
    //        if(Settings.Contains(key))
    //        {
    //            Settings.Remove(key);
    //        }
    //        Settings.Save();
    //    }

    //    public BaseEntry GetStoredEntry()
    //    {
    //        return (BaseEntry)Settings[Constants.IsolatedStorage.ISO_SELECTED_ENTRY];
    //    }

    //    /// <summary>
    //    /// Checks to see what the user's stored DB version is and copies over the the latest version (if necessary)
    //    /// to their Isolated Storage.  This method DOES NOT run any queries against the Database, but will simply copy the latest
    //    /// version of the database over if necessary.
    //    /// </summary>
    //    public void CheckIfDBFileUpdateNeeded()
    //    {

    //        IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();

    //        //DETECTION OF LATEST VERSION OF DATABASE IS DONE HERE.
    //        if (Settings.Contains(Constants.IsolatedStorage.ISO_DB_VERSION))
    //        {
    //            if (Convert.ToInt32(Settings[Constants.IsolatedStorage.ISO_DB_VERSION].ToString()) < Constants.Database.DB_CURRENT_VERSION) //less than current version
    //            {
    //                //recreate the db in iso storage so the changes are copied over
    //                if (store.FileExists(Constants.Database.DATABASE_PATH))
    //                {
    //                    store.DeleteFile(Constants.Database.DATABASE_PATH);
    //                    CopyDatabase();
    //                }
    //                Settings[Constants.IsolatedStorage.ISO_DB_VERSION] = Constants.Database.DB_CURRENT_VERSION;//set the version in iso storage
    //            }
    //            else
    //            { //equal or greater to current version
    //                Settings[Constants.IsolatedStorage.ISO_DB_VERSION] = Constants.Database.DB_CURRENT_VERSION;//set the version in iso storage
    //            }
    //        }
    //        else
    //        { //version does not exist in iso storage
    //            Settings.Add(Constants.IsolatedStorage.ISO_DB_VERSION, Constants.Database.DB_CURRENT_VERSION);//set the version in iso storage
    //            if (!store.FileExists(Constants.Database.DATABASE_PATH)) //first time booting up app
    //            {
    //                CopyDatabase();
    //            }
    //            else //user had db before, but did not have the dbversion iso storage key to track upgrades.
    //            {
    //                store.DeleteFile(Constants.Database.DATABASE_PATH);
    //                CopyDatabase();
    //            }
    //        }
    //        Settings.Save();
    //    }

    //    public bool? GetLicenseValue()
    //    {
    //        bool? result = null;

    //        if(Settings.Contains(Constants.IsolatedStorage.ISO_LICENSE_KEY))
    //        {
    //            result = (bool)Settings[Constants.IsolatedStorage.ISO_LICENSE_KEY];
    //        }

    //        return result;
    //    }

    //    public void StoreLicenseValue(bool licenseValue)
    //    {
    //        if(Settings.Contains(Constants.IsolatedStorage.ISO_LICENSE_KEY))
    //        {
    //            Settings.Add(Constants.IsolatedStorage.ISO_LICENSE_KEY, licenseValue);
    //        }
    //        else
    //        {
    //            Settings[Constants.IsolatedStorage.ISO_LICENSE_KEY] = licenseValue;
    //        }

    //        Settings.Save();
    //    }

    //    public void SaveExceptionDetails(Exception eObject)
    //    {
    //        if (Settings.Contains(Constants.IsolatedStorage.ISO_EXCEPTION))
    //        {
    //            Settings[Constants.IsolatedStorage.ISO_EXCEPTION] = eObject.Message + Environment.NewLine + Environment.NewLine + 
    //eObject.StackTrace + Environment.NewLine + Environment.NewLine + 
    //Environment.OSVersion + Environment.NewLine + Environment.NewLine;
    //        }
    //        else //does not contain the key so we need to add it
    //        {
    //            Settings.Add(Constants.IsolatedStorage.ISO_EXCEPTION, eObject.Message + Environment.NewLine + Environment.NewLine + eObject.StackTrace + Environment.NewLine + Environment.NewLine + Environment.OSVersion + Environment.NewLine + Environment.NewLine);
    //        }
    //        Settings.Save();
    //    }

    //    private bool GetExceptionFromIsoStorage()
    //    {
    //        if (Settings.Contains(Constants.IsolatedStorage.ISO_EXCEPTION))
    //        {
    //            if (!Settings.TryGetValue(Constants.IsolatedStorage.ISO_EXCEPTION, out savedExceptionData))
    //            {
    //                return false;
    //            }
    //            else
    //            {
    //                return true; //exception data found!
    //            }
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }

    //    /// <summary>
    //    /// Using the passed in index in the application settings, store the passed ObservableCollection.
    //    /// </summary>
    //    /// <param name="settingsIndex"></param>
    //    /// <param name="itemsToAdd"></param>
    //    public void StoreValues(string settingsIndex, IList<BaseEntry> itemsToAdd)
    //    {
    //        if (Settings.Contains(settingsIndex))
    //        {
    //            Settings[settingsIndex] = itemsToAdd;
    //        }
    //        else
    //        {
    //            Settings.Add(settingsIndex, itemsToAdd);
    //        }
    //    }

    //    public void StoreAdditionalPlaylist(Playlist playlist)
    //    {
    //        List<Playlist> existingPlaylists = new List<Playlist>();

    //        if(Settings.Contains(Constants.IsolatedStorage.ISO_PLAYLISTS))
    //        {
    //            existingPlaylists = (List<Playlist>)Settings[Constants.IsolatedStorage.ISO_PLAYLISTS];

    //            bool isAlreadyInPlaylists = false;
    //            foreach(Playlist p in existingPlaylists)
    //            {
    //                if (p.Name.Equals(playlist.Name))
    //                {
    //                    isAlreadyInPlaylists = true;
    //                    break; //already exists, no need to save it
    //                }
    //            }
    //            if(!isAlreadyInPlaylists)
    //            {
    //                existingPlaylists.Add(playlist);
    //                Settings[Constants.IsolatedStorage.ISO_PLAYLISTS] = existingPlaylists;
    //            }
    //        }
    //        else
    //        {
    //            Settings.Add(Constants.IsolatedStorage.ISO_PLAYLISTS, new List<Playlist>() { playlist });
    //        }

    //        Settings.Save();
    //    }

    //    public void StoreAllPlaylists(List<Playlist> playlists)
    //    {
    //        if (Settings.Contains(Constants.IsolatedStorage.ISO_PLAYLISTS))
    //        {
    //            List<Playlist> existingPlaylists = (List<Playlist>)Settings[Constants.IsolatedStorage.ISO_PLAYLISTS];

    //            foreach(Playlist p in playlists)
    //            {
    //                if(!existingPlaylists.Contains(p))
    //                {
    //                    existingPlaylists.Add(p);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            Settings.Add(Constants.IsolatedStorage.ISO_PLAYLISTS, playlists);
    //        }

    //        Settings.Save();
    //    }

    //    /// <summary>
    //    /// Store a playlist for passing across pages
    //    /// </summary>
    //    /// <param name="playlist"></param>
    //    public void StoreTempPlaylist(Playlist playlist)
    //    {
    //        if(Settings.Contains(Constants.IsolatedStorage.ISO_PLAYLIST_TEMP_STORAGE))
    //        {
    //            Settings[Constants.IsolatedStorage.ISO_PLAYLIST_TEMP_STORAGE] = playlist;
    //        }
    //        else
    //        {
    //            Settings.Add(Constants.IsolatedStorage.ISO_PLAYLIST_TEMP_STORAGE, playlist);
    //        }
    //    }

    //    /// <summary>
    //    /// Get the stored temp playlist for passing across pages if it exists
    //    /// </summary>
    //    /// <returns></returns>
    //    public Playlist GetTempPlaylist()
    //    {
    //        Playlist playlist = null;

    //        if(Settings.Contains(Constants.IsolatedStorage.ISO_PLAYLIST_TEMP_STORAGE))
    //        {
    //            playlist = (Playlist)Settings[Constants.IsolatedStorage.ISO_PLAYLIST_TEMP_STORAGE];
    //        }

    //        return playlist;
    //    }

    //    public List<Playlist> LoadPlaylists()
    //    {
    //        List<Playlist> playlists = new List<Playlist>();

    //        if(Settings.Contains(Constants.IsolatedStorage.ISO_PLAYLISTS))
    //        {
    //            playlists = (List<Playlist>)Settings[Constants.IsolatedStorage.ISO_PLAYLISTS];
    //        }

    //        return playlists;
    //    }

    //    /// <summary>
    //    /// Stores a single BaseEntry in iso storage rather than a list
    //    /// </summary>
    //    /// <param name="settingsIndex"></param>
    //    /// <param name="entry"></param>
    //    public void StoreSingleValue(string settingsIndex, BaseEntry entry)
    //    {
    //        if (Settings.Contains(settingsIndex))
    //        {
    //            List<BaseEntry> existingItems = (List<BaseEntry>)Settings[settingsIndex];
    //            existingItems.Add(entry);
    //            Settings[settingsIndex] = existingItems;
    //        }
    //        else
    //        {
    //            List<BaseEntry> items = new List<BaseEntry>() 
    //            {
    //                entry
    //            };
    //            Settings.Add(settingsIndex, items);
    //        }
    //        Settings.Save();
    //    }

    //    /// <summary>
    //    /// Use new Wp8 APIs to copy the file to the local storage on the phone.
    //    /// </summary>
    //    private async void CopyDatabase()
    //    {
    //        StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync("dnd35.sqlite");
    //        await databaseFile.CopyAsync(ApplicationData.Current.LocalFolder, "dnd35.sqlite", NameCollisionOption.ReplaceExisting);
    //    }

    //    /// <summary>
    //    /// Using the passed in index in the application settings, store the passed ObservableCollection.
    //    /// </summary>
    //    /// <param name="settingsIndex"></param>
    //    /// <param name="itemsToAdd"></param>
    //    //public void storeValues(string settingsIndex, ObservableCollection<Model.Item> itemsToAdd)
    //    //{
    //    //    ObservableCollection<Model.Item> tempHistoryItems;//use this as a temporary check to see if the key exists.  There must be a better way to do this, but until I know how, this is how I check for the key
    //    //    //and add it if necessary without populating the searchHistory list with null items.

    //    //    if (!Settings.TryGetValue<ObservableCollection<Model.Item>>(settingsIndex, out tempHistoryItems))
    //    //    {
    //    //        Settings.Add(settingsIndex, itemsToAdd);
    //    //    }
    //    //    else
    //    //    {
    //    //        Settings[settingsIndex] = itemsToAdd;
    //    //    }
    //    //}
    //}
}

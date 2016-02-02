using DnDNavigator.Runtime.Model.Entry;
using DnDNavigator.Runtime.Model.Playlists;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Storage;
using System.Runtime.Serialization;
using DnDNavigator.Runtime.Model.DnDEntry;
using System.IO;

namespace DnDNavigator.Runtime.DataAccess
{
    public static class IsolatedStorage
    {
        public const string ISO_PREVIOUS_SEARCHES = "PreviousSearches";
        public const string ISO_OLDITEM_HTML = "OldItemHTML";
        public const string ISO_FAVORITES = "Favorites";
        public const string ISO_DB_VERSION = "DBVersion";
        public const string ISO_EXCEPTION = "LastException";
        public const string ISO_SHOW_DEFAULT_SEARCH_MESSAGE = "showDefaultMessage";
        public const string ISO_SELECTED_ENTRY = "SelectedEntry";
        public const string ISO_PLAYLISTS = "Playlists";
        public const string ISO_PLAYLIST_TEMP_STORAGE = "PlaylistTemp";
        public const string ISO_LICENSE_KEY = "License";
        private const string ISO_HISTORY_UPGRADED_FLAG = "HistoryUpgraded";
        private const string ISO_FAVORITES_UPGRADED_FLAG = "FavoritesUpgraded";
        private const string FILE_HISTORY = "HistoryItems";
        private const string FILE_FAVORITES = "FavoriteItems";
        private const string FILE_PLAYLISTS = "PlaylistItems";

        private static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        private static List<Type> knownTypes;
        private static IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForApplication();

        internal static bool HasConvertedHistory
        {
            get
            {
                return (bool)settings[ISO_HISTORY_UPGRADED_FLAG];
            }
            private set
            {
                settings[ISO_HISTORY_UPGRADED_FLAG] = value;
                SaveSettingsData();
            }
        }

        internal static bool HasConvertedFavorites
        {
            get
            {
                return (bool)settings[ISO_FAVORITES_UPGRADED_FLAG];
            }
            set
            {
                settings[ISO_FAVORITES_UPGRADED_FLAG] = value;
                SaveSettingsData();
            }
        }

        #region Properties
        public static bool? LicenseStatus
        {
            get
            {
                bool? result = null;

                if (settings.Contains(ISO_LICENSE_KEY))
                {
                    result = (bool?)settings[ISO_LICENSE_KEY];
                }
                return result;
            }
            set
            {
                settings[ISO_LICENSE_KEY] = value;
                SaveSettingsData();
            }
        }

        private static List<Playlist> playlists;
        public static List<Playlist> Playlists
        {
            get
            {
                if (playlists == null)
                {
                    TryGetFileValue<List<Playlist>>(FILE_PLAYLISTS, out playlists);
                }
                return playlists;
            }
            set
            {
                playlists = value;
                SaveFile<List<Playlist>>(FILE_PLAYLISTS, playlists);
            }
        }

        private static List<BaseEntry> history;
        public static List<BaseEntry> History
        {
            get
            {
                if (history == null)
                {
                    TryGetFileValue<List<BaseEntry>>(FILE_HISTORY, out history);
                }
                return history;
            }
            set
            {
                history = value;
                SaveFile<List<BaseEntry>>(FILE_HISTORY, history);
            }
        }

        private static List<BaseEntry> favorites;
        public static List<BaseEntry> Favorites
        {
            get
            {
                if (favorites == null)
                {
                    TryGetFileValue<List<BaseEntry>>(FILE_FAVORITES, out favorites);
                }
                return favorites;
            }
            set
            {
                favorites = value;
                SaveFile<List<BaseEntry>>(FILE_FAVORITES, favorites);
            }
        }

        public static string SavedExceptionData
        {
            get
            {
                string retVal = string.Empty;

                if (settings.Contains(ISO_EXCEPTION))
                {
                    retVal = (string)settings[ISO_EXCEPTION];
                }
                return retVal;
            }
            set
            {
                settings[ISO_EXCEPTION] = value;
                IsoDataChanged();
            }
        }

        private static BaseEntry tempEntry;
        public static BaseEntry TempEntry
        {
            get
            {
                if (tempEntry == null)
                {
                    TryGetValue<BaseEntry>(ISO_SELECTED_ENTRY, out tempEntry);
                }
                return tempEntry;
            }
            set
            {
                tempEntry = value;
            }
        }

        private static Playlist tempPlaylist;
        public static Playlist TempPlaylist
        {
            get
            {
                if (tempPlaylist == null)
                {
                    TryGetValue<Playlist>(ISO_PLAYLIST_TEMP_STORAGE, out tempPlaylist);
                }
                return tempPlaylist;
            }
            set
            {
                tempPlaylist = value;
            }
        }
        #endregion

        static IsolatedStorage()
        {
            knownTypes = new List<Type>()
            {
                typeof(Class),
                typeof(Domain),
                typeof(Equipment),
                typeof(Feat),
                typeof(DnDNavigator.Runtime.Model.DnDEntry.Item),
                typeof(Monster),
                typeof(Power),
                typeof(Skill),
                typeof(Spell),
                typeof(Playlist),
                typeof(BaseEntry)
            };

            //convert legacy stuff right away
            if (!HasUpgraded(ISO_HISTORY_UPGRADED_FLAG))
            {
                ConvertLegacyItems(ISO_PREVIOUS_SEARCHES, FILE_HISTORY);
                HasConvertedHistory = true;
            }

            if (!HasUpgraded(ISO_FAVORITES_UPGRADED_FLAG))
            {
                ConvertLegacyItems(ISO_FAVORITES, FILE_FAVORITES);
                HasConvertedFavorites = true;
            }
        }

        private static bool HasUpgraded(string key)
        {
            bool result = false;

            if (settings.Contains(key))
            {
                result = (bool)settings[key];
            }
            return result;
        }

        /// <summary>
        /// If a value is not found using the passed in key, a new key/value pair is created in settings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool TryGetValue<T>(string key, out T value)
        {
            bool foundKey = false;
            value = default(T);

            if (settings.Contains(key))
            {
                value = (T)settings[key];
                foundKey = true;
            }

            return foundKey;
        }

        /// <summary>
        /// If a file is not found, creates a new file with newly initialized value of type T written to it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool TryGetFileValue<T>(string fileName, out T value)
        {
            bool success = false;

            if (isoFile.FileExists(fileName))
            {
                using (IsolatedStorageFileStream stream = isoFile.OpenFile(fileName, FileMode.Open))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T), knownTypes);
                    value = (T)serializer.ReadObject(stream);
                    success = true;
                }
            }
            else
            {
                value = (T)Activator.CreateInstance(typeof(T));
                using (IsolatedStorageFileStream stream = isoFile.OpenFile(fileName, FileMode.Create))
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(T));
                    ser.WriteObject(stream, value);
                }
            }

            return success;
        }

        private static void SaveSettingsData()
        {
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public static void IsoDataChanged()
        {
            IsolatedStorageSettings.ApplicationSettings.Save();
            SaveAllFileData();
        }

        private static void SaveAllFileData()
        {
            SaveFile<List<BaseEntry>>(FILE_FAVORITES, Favorites);
            SaveFile<List<BaseEntry>>(FILE_HISTORY, History);
            SaveFile<List<Playlist>>(FILE_PLAYLISTS, Playlists);
        }

        private static void SaveFile<T>(string fileName, T content)
        {
            using (IsolatedStorageFileStream stream = isoFile.OpenFile(fileName, FileMode.Create))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T), knownTypes);
                serializer.WriteObject(stream, content);
            }
        }

        public static void ShowExceptionMessageIfPresent()
        {
            //check if an unhandled exception occured last time
            if (!string.IsNullOrEmpty(SavedExceptionData))//if yes
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    CustomMessageBox messageBox = new CustomMessageBox()
                    {
                        Caption = "Uh-oh!  There was an error!",
                        Message = "Would you like to submit the error information to the developer? \n It would help speed up the process of fixing the issue!",
                        LeftButtonContent = "Yes",
                        RightButtonContent = "Nope",
                    };
                    messageBox.Dismissed += (s1, e1) =>
                    {
                        switch (e1.Result)
                        {
                            case CustomMessageBoxResult.LeftButton:
                                {
                                    //compose email
                                    new Feedback.FeedbackEmail(SavedExceptionData);
                                    break;
                                }
                            case CustomMessageBoxResult.RightButton:
                                {
                                    break;
                                }
                            case CustomMessageBoxResult.None:
                                {
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    };

                    messageBox.Show();
                });

                settings.Remove(IsolatedStorage.ISO_EXCEPTION);//clear the exception
            }
        }

        /// <summary>
        /// Checks to see what the user's stored DB version is and copies over the the latest version (if necessary)
        /// to their Isolated Storage.  This method DOES NOT run any queries against the Database, but will simply copy the latest
        /// version of the database over if necessary.
        /// </summary>
        public static void CheckIfDBFileUpdateNeeded()
        {

            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();

            //DETECTION OF LATEST VERSION OF DATABASE IS DONE HERE.
            if (settings.Contains(IsolatedStorage.ISO_DB_VERSION))
            {
                if (Convert.ToInt32(settings[IsolatedStorage.ISO_DB_VERSION].ToString()) < Constants.Database.DB_CURRENT_VERSION) //less than current version
                {
                    //recreate the db in iso storage so the changes are copied over
                    if (store.FileExists(Constants.Database.DATABASE_PATH))
                    {
                        store.DeleteFile(Constants.Database.DATABASE_PATH);
                        CopyDatabase();
                    }
                    settings[IsolatedStorage.ISO_DB_VERSION] = Constants.Database.DB_CURRENT_VERSION;//set the version in iso storage
                }
                else
                { //equal or greater to current version
                    settings[IsolatedStorage.ISO_DB_VERSION] = Constants.Database.DB_CURRENT_VERSION;//set the version in iso storage
                }
            }
            else
            { //version does not exist in iso storage
                settings.Add(IsolatedStorage.ISO_DB_VERSION, Constants.Database.DB_CURRENT_VERSION);//set the version in iso storage
                if (!store.FileExists(Constants.Database.DATABASE_PATH)) //first time booting up app
                {
                    CopyDatabase();
                }
                else //user had db before, but did not have the dbversion iso storage key to track upgrades.
                {
                    store.DeleteFile(Constants.Database.DATABASE_PATH);
                    CopyDatabase();
                }
            }
            settings.Save();
        }

        public static void ConvertLegacyItems(string key, string filePathToSaveTo)
        {
            List<BaseEntry> items = new List<BaseEntry>();

            if (settings.Contains(key))
            {
                //holds both new and old entries
                List<BaseEntry> tempList = new List<BaseEntry>();
                List<DnDNavigator.Runtime.Model.Item> legacyTempList = new List<DnDNavigator.Runtime.Model.Item>();

                if (settings[key] is List<BaseEntry>)
                {
                    tempList = (List<BaseEntry>)settings[key];
                    items.AddRange(tempList);
                }
                else //found legacy items.  convert them.
                {
                    legacyTempList = (List<DnDNavigator.Runtime.Model.Item>)settings[key];
                    foreach (DnDNavigator.Runtime.Model.Item item in legacyTempList.ToList<DnDNavigator.Runtime.Model.Item>())
                    {
                        //convert it to a new Entry
                        BaseEntry convertedResult = LegacyItemConversionService.ConvertLegacyItemToEntry(item);
                        items.Add(convertedResult);
                        legacyTempList.Remove(item); //remove old item now that it has been converted
                    }
                }
            }

            SaveFile<List<BaseEntry>>(filePathToSaveTo, items);
        }

        /// <summary>
        /// Use new Wp8 APIs to copy the file to the local storage on the phone.
        /// </summary>
        private static async void CopyDatabase()
        {
            StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync("dnd35.sqlite");
            await databaseFile.CopyAsync(ApplicationData.Current.LocalFolder, "dnd35.sqlite", NameCollisionOption.ReplaceExisting);
        }
    }
}

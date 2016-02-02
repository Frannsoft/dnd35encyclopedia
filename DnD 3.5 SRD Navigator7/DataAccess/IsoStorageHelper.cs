using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO.IsolatedStorage;
using System.Text;
using System.Windows;

namespace DnD_3._5_SRD_Navigator7.DataAccess
{
    /// <summary>
    /// Used to interact with IsolatedStorage on the Phone.
    /// </summary>
   internal class IsoStorageHelper
    {

        public IsolatedStorageSettings settings;

        public IsoStorageHelper()
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
        }

       /// <summary>
       /// Checks to see what the user's stored DB version is and copies over the the latest version (if necessary)
       /// to their Isolated Storage.  This method DOES NOT run any queries against the Database, but will simply copy the latest
       /// version of the database over if necessary.
       /// </summary>
       public void checkIfDBFileUpdateNeeded()
       {
           IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
           IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

           //DETECTION OF LATEST VERSION OF DATABASE IS DONE HERE.
           if (settings.Contains(Constants.IsolatedStorage.ISO_DB_VERSION))
           {
               if (Convert.ToInt32(settings[Constants.IsolatedStorage.ISO_DB_VERSION].ToString()) < Constants.Database.DB_CURRENT_VERSION) //less than current version
               {
                   //recreate the db in iso storage so the changes are copied over
                   if (store.FileExists(Constants.Database.databasePath))
                   {
                       store.DeleteFile(Constants.Database.databasePath);
                       CopyFromContentToStorage();//recopy
                   }
                   settings[Constants.IsolatedStorage.ISO_DB_VERSION] = Constants.Database.DB_CURRENT_VERSION;//set the version in iso storage
               }
               else
               { //equal or greater (won't happen) to current version
                   settings[Constants.IsolatedStorage.ISO_DB_VERSION] = Constants.Database.DB_CURRENT_VERSION;//set the version in iso storage
               }
           }
           else
           { //version does not exist in iso storage
               settings.Add(Constants.IsolatedStorage.ISO_DB_VERSION, Constants.Database.DB_CURRENT_VERSION);//set the version in iso storage
               if (!store.FileExists(Constants.Database.databasePath)) //first time booting up app
               {
                   CopyFromContentToStorage();
               }
               else //user had db before, but did not have the dbversion iso storage key to track upgrades.
               {
                   store.DeleteFile(Constants.Database.databasePath);
                   CopyFromContentToStorage();//recopy
               }
           }

           //check and deal with update message
           //**1/3/13 - Save for later.  No need for this right now, but this system works.
           //if (settings.Contains(Constants.IsolatedStorage.ISO_UPDATE_MESSAGE))
           //{
           //    if(!settings[Constants.IsolatedStorage.ISO_UPDATE_MESSAGE].Equals(Constants.IsolatedStorage.ISO_UPDATE_MESSAGE_VERSION))
           //    {
           //        //show message
           //        MessageBox.Show(Constants.IsolatedStorage.ISO_UPDATE_MESSAGE_TEXT);

           //        //set the key value to the most recent version
           //        settings[Constants.IsolatedStorage.ISO_UPDATE_MESSAGE] = Constants.IsolatedStorage.ISO_UPDATE_MESSAGE_VERSION;
           //    }
           //}
           //else
           //{
           //    //show the message
           //    MessageBox.Show(Constants.IsolatedStorage.ISO_UPDATE_MESSAGE_TEXT);
           //    //add the key and set to true (shown).
           //    settings.Add(Constants.IsolatedStorage.ISO_UPDATE_MESSAGE, Constants.IsolatedStorage.ISO_UPDATE_MESSAGE_VERSION);//set the version in iso storage
           //}
           settings.Save();
       }

       /// <summary>
       /// Using the passed in index in the application settings, store the passed ObservableCollection.
       /// </summary>
       /// <param name="settingsIndex"></param>
       /// <param name="itemsToAdd"></param>
       public void storeValues(string settingsIndex, ObservableCollection<Model.Item> itemsToAdd)
       {
           ObservableCollection<Model.Item> tempHistoryItems;//use this as a temporary check to see if the key exists.  There must be a better way to do this, but until I know how, this is how I check for the key
           //and add it if necessary without populating the searchHistory list with null items.

           if (!settings.TryGetValue<ObservableCollection<Model.Item>>(settingsIndex, out tempHistoryItems))
           {
               settings.Add(settingsIndex, itemsToAdd);
           }
           else
           {
               settings[settingsIndex] = itemsToAdd;
           }
       }


       /// <summary>
       /// Using the passed in index in the application settings, store the passed ObservableCollection.
       /// </summary>
       /// <param name="settingsIndex"></param>
       /// <param name="itemsToAdd"></param>
       public void storeValues(string settingsIndex, List<Model.Item> itemsToAdd)
       {
           List<Model.Item> tempHistoryItems;//use this as a temporary check to see if the key exists.  There must be a better way to do this, but until I know how, this is how I check for the key
           //and add it if necessary without populating the searchHistory list with null items.

           if (!settings.TryGetValue<List<Model.Item>>(settingsIndex, out tempHistoryItems))
           {
               settings.Add(settingsIndex, itemsToAdd);
           }
           else
           {
               settings[settingsIndex] = itemsToAdd;
           }
       }

        /// <summary>
        /// Copy the database from the resources in the app to the user's Isolated Storage.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="dbName"></param>
        private void CopyFromContentToStorage()
        {
            //Get assembly for Resource Stream
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

            IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            System.IO.Stream src = Application.GetResourceStream(new Uri("/" + assembly.FullName.Substring(0, assembly.FullName.IndexOf(',')) + ";component/" + Constants.Database.databasePath, UriKind.Relative)).Stream;

            IsolatedStorageFileStream dest = new IsolatedStorageFileStream(Constants.Database.databasePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, store);
            src.Position = 0;
            CopyStream(src, dest);
            dest.Flush();
            dest.Close();
            src.Close();
        }

        /// <summary>
        /// Physically move over the database file.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        private static void CopyStream(System.IO.Stream input, IsolatedStorageFileStream output)
        {
            byte[] buffer = new byte[32768];
            long TempPos = input.Position;
            int readCount;
            do
            {
                readCount = input.Read(buffer, 0, buffer.Length);
                if (readCount > 0)
                {
                    output.Write(buffer, 0, readCount);
                }
            } while (readCount > 0);
            input.Position = TempPos;
        }
    }
}

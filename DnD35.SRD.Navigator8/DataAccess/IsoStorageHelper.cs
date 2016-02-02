using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO.IsolatedStorage;
using System.Text;
using System.Windows;
using Windows.Storage;
using Windows.ApplicationModel;
using Microsoft.Phone.Controls;

namespace DnD_3._5_SRD_Navigator8.DataAccess
{
    /// <summary>
    /// Used to interact with IsolatedStorage on the Phone.
    /// </summary>
   internal class IsoStorageHelper
    {

        public IsolatedStorageSettings settings;

        private string savedExceptionData = string.Empty;

        public IsoStorageHelper()
        {
            settings = IsolatedStorageSettings.ApplicationSettings;

            //check if an unhandled exception occured last time
            if (getUnhandledException())//if yes
            {
                //show custom message box asking if user wants to submit feedback on it
                //TODO - ask user if they want to submit feedback on the issue
                //if yes, compose email with exception details
                //if no, close the app
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
                                    new Feedback.FeedbackEmail(savedExceptionData);
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

                settings.Remove(Constants.IsolatedStorage.ISO_EXCEPTION);//clear the exception
            }
        }

       /// <summary>
       /// Checks to see what the user's stored DB version is and copies over the the latest version (if necessary)
       /// to their Isolated Storage.  This method DOES NOT run any queries against the Database, but will simply copy the latest
       /// version of the database over if necessary.
       /// </summary>
       public void checkIfDBFileUpdateNeeded()
       {

           IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();

           //DETECTION OF LATEST VERSION OF DATABASE IS DONE HERE.
           if (settings.Contains(Constants.IsolatedStorage.ISO_DB_VERSION))
           {
               if (Convert.ToInt32(settings[Constants.IsolatedStorage.ISO_DB_VERSION].ToString()) < Constants.Database.DB_CURRENT_VERSION) //less than current version
               {
                   //recreate the db in iso storage so the changes are copied over
                   if (store.FileExists(Constants.Database.databasePath))
                   {
                       store.DeleteFile(Constants.Database.databasePath);
                       CopyDatabase();
                   }
                   settings[Constants.IsolatedStorage.ISO_DB_VERSION] = Constants.Database.DB_CURRENT_VERSION;//set the version in iso storage
               }
               else
               { //equal or greater to current version
                   settings[Constants.IsolatedStorage.ISO_DB_VERSION] = Constants.Database.DB_CURRENT_VERSION;//set the version in iso storage
               }
           }
           else
           { //version does not exist in iso storage
               settings.Add(Constants.IsolatedStorage.ISO_DB_VERSION, Constants.Database.DB_CURRENT_VERSION);//set the version in iso storage
               if (!store.FileExists(Constants.Database.databasePath)) //first time booting up app
               {
                   CopyDatabase();
               }
               else //user had db before, but did not have the dbversion iso storage key to track upgrades.
               {
                   store.DeleteFile(Constants.Database.databasePath);
                   CopyDatabase();
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

       public void saveUnhandledExceptionDetails(Exception eObject)
       {
           if (settings.Contains(Constants.IsolatedStorage.ISO_EXCEPTION))
           {
               settings[Constants.IsolatedStorage.ISO_EXCEPTION] = eObject;
           }
           else //does not contain the key so we need to add it
           {
               settings.Add(Constants.IsolatedStorage.ISO_EXCEPTION, eObject.Message + Environment.NewLine + Environment.NewLine + eObject.StackTrace + Environment.NewLine + Environment.NewLine + Environment.OSVersion + Environment.NewLine + Environment.NewLine);
           }
           settings.Save();
       }

       private bool getUnhandledException()
       {
           if (settings.Contains(Constants.IsolatedStorage.ISO_DB_VERSION))
           {
               if (!settings.TryGetValue(Constants.IsolatedStorage.ISO_EXCEPTION, out savedExceptionData))
               {
                   return false;
               }
               else
               {
                   return true; //exception data found!
               }
           }
           else
           {
               return false;
           }
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
       /// Use new Wp8 APIs to copy the file to the local storage on the phone.
       /// </summary>
       private async void CopyDatabase()
       {
               StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync("dnd35.sqlite");
               await databaseFile.CopyAsync(ApplicationData.Current.LocalFolder, "dnd35.sqlite", NameCollisionOption.ReplaceExisting);
       }

        /// <summary>
        /// Copy the database from the resources in the app to the user's Isolated Storage.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="dbName"></param>
        [Obsolete]
        private void CopyFromContentToStorage()
        {
            ////Get assembly for Resource Stream
            //System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

            //IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
            //System.IO.Stream src = Application.GetResourceStream(new Uri("/" + assembly.FullName.Substring(0, assembly.FullName.IndexOf(',')) + ";component/" + Constants.Database.databasePath, UriKind.Relative)).Stream;

            //IsolatedStorageFileStream dest = new IsolatedStorageFileStream(Constants.Database.databasePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, store);
            //src.Position = 0;
            //CopyStream(src, dest);
            //dest.Flush();
            //dest.Close();
            //src.Close();
        }

        /// <summary>
        /// Physically move over the database file.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        [Obsolete]
        private static void CopyStream(System.IO.Stream input, IsolatedStorageFileStream output)
        {
            //byte[] buffer = new byte[32768];
            //long TempPos = input.Position;
            //int readCount;
            //do
            //{
            //    readCount = input.Read(buffer, 0, buffer.Length);
            //    if (readCount > 0)
            //    {
            //        output.Write(buffer, 0, readCount);
            //    }
            //} while (readCount > 0);
            //input.Position = TempPos;
        }


       /// <summary>
       /// Returns a bool storing whether or not the NFC messagebox has been shown to the user before.
       /// </summary>
       /// <returns></returns>
        public bool hasNFCMessageBoxBeenShown()
        {
            bool hasBeenShown = false;
            try
            {
                if (settings.Contains(Constants.IsolatedStorage.ISO_SHOW_MSGBOX_PROXIMITY_BLUETOOTH))
                {
                    try
                    {
                        int isoVal = int.Parse(settings[Constants.IsolatedStorage.ISO_SHOW_MSGBOX_PROXIMITY_BLUETOOTH].ToString());
                        if (isoVal > 0)
                        {
                            hasBeenShown = true;
                        }
                    }
                    catch (ArgumentNullException ane)
                    {
                        MessageBox.Show("Error retrieving NFC MSGBOX value: " + ane.StackTrace);
                    }
                    catch (FormatException fe)
                    {
                        MessageBox.Show("Error retrieving NFC MSGBOX value: " + fe.StackTrace);
                    }
                }
                else
                {
                    settings.Add(Constants.IsolatedStorage.ISO_SHOW_MSGBOX_PROXIMITY_BLUETOOTH, 0);
                }
            }
            catch (ArgumentNullException a)
            {
                MessageBox.Show("Error retrieving NFC MSGBOX value: " + a.StackTrace);
            }
            return hasBeenShown;
        }
    }
}

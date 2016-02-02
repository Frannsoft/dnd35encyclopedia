using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using Sqlite;
using SQLite;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;

namespace DnD_3._5_SRD_Navigator8.DataAccess
{
    /// <summary>
    /// TODO - needs to be reworked for WP8.
    /// </summary>
    public class DBHelper
    {
        private SQLiteConnection db = null;

        public DBHelper()
        {
        }
        ~DBHelper()
        {
            Close();
        }
        /// <summary>
        /// Open a connection to the database.
        /// </summary>
        private void Open()
        {
            if (db == null)
            {
                try
                {
                    //this opens a connection as well.
                    //db = new SQLiteConnection(Constants.Database.connectionString);
                    db = new SQLiteConnection(Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "dnd35.sqlite"));
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show("Error while connecting to Database: \n" + e.Message + "\n" + e.StackTrace);
                }
            }
        }

        /// <summary>
        /// Close the connection (if there is one) to the database and clear out the SQLite HandleTracker.
        /// </summary>
        private void Close()
        {
            if (db != null)
            {
                try
                {
                    db.Dispose();
                    db = null;
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show("Error while closing DB connection: \n" + e.Message + "\n" + e.StackTrace);
                }
            }
        }

        /// <summary>
        /// Select a single result using the passed in value to formulate a query.
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public Model.Item Select(String statement)
        {
            Open();
            List<Model.Item> result = null;
            try
            {
                using (var dbb = new SQLiteConnection(Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "dnd35.sqlite")))
                {
                    result = dbb.Query<Model.Item>(statement);
                }
            }
            catch (SQLiteException sle)
            {
                System.Windows.MessageBox.Show("Error while querying Database: \n" + sle.Message + "\n" + sle.StackTrace);
            }
            finally
            {
                Close();
            }
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Select from the database and return a list of results as a List<Model.Item>
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public List<Model.Item> SelectBrowse(String statement)
        {
            Open();
            List<Model.Item> results = null;

            try
            {
                using(var dbb = new SQLiteConnection(Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "dnd35.sqlite")))
                {
                     results = dbb.Query<Model.Item>(statement);
                }
            }
            catch (SQLiteException sle)
            {
                System.Windows.MessageBox.Show("Error while querying Database: \n" + sle.Message + "\n" + sle.StackTrace);
            }
            finally
            {
                Close();
            }
            return results;
        }
    }
}

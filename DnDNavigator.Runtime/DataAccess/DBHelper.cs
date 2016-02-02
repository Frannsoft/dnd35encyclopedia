using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using System.IO;
using System.Windows;
using Windows.Storage;
using DnDNavigator.Runtime.Model;
using DnDNavigator.Runtime.Constants;
using System.Threading.Tasks;
using DnDNavigator.Runtime.Model.Entry;

namespace DnDNavigator.Runtime.DataAccess
{
    /// <summary>
    /// Aids basic CRUD for the database
    /// </summary>
    public class DBHelper : IDisposable
    {
        private SQLiteConnection db = null;

        public DBHelper()
        { }

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
                    db = new SQLiteConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "dnd35.sqlite"));
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error while connecting to Database: \n" + e.Message + "\n" + e.StackTrace);
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
                    MessageBox.Show("Error while closing DB connection: \n" + e.Message + "\n" + e.StackTrace);
                }
            }
        }

        /// <summary>
        /// Get a single entry from the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryStatement"></param>
        /// <returns></returns>
        public T GetEntry<T>(string queryStatement) where T : new()
        {
            T result = default(T);
            try
            {
                using(var dbb = new SQLiteConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, Database.DATABASE_PATH)))
                {
                    result = dbb.Query<T>(queryStatement).First<T>();
                }
            }
            catch(SQLiteException sle)
            {
                MessageBox.Show("Error while querying Database: \n" + sle.Message + "\n" + sle.StackTrace);
            }

            return result;
        }

        public T GetSingleValue<T>(string queryStatement) where T : new()
        {
            T result = default(T);
            try
            {
                using(var dbb = new SQLiteConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, Database.DATABASE_PATH)))
                {
                    result = dbb.Query<T>(queryStatement).First<T>();
                }
            }
            catch(SQLiteException sle)
            {
                MessageBox.Show("Error while querying Database for single value: \n" + sle.Message + "\n" + sle.StackTrace);
            }

            return result;
        }

        public async Task<List<T>> GetEntries<T>(string queryStatement) where T : new()
        {
            List<T> results = new List<T>();
            try
            {
                using (var dbb = new SQLiteConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, Database.DATABASE_PATH)))
                {
                    results.AddRange(await Task.Run(() => dbb.Query<T>(queryStatement)));
                }
            }
            catch (SQLiteException sle)
            {
                MessageBox.Show("Error while querying Database: \n" + sle.Message + "\n" + sle.StackTrace);
            }
            return results;
        }

        public async Task<List<T>> SearchEntries<T>(string queryStatement) where T : new()
        {
            List<T> results = new List<T>();
            try
            {
                using (var dbb = new SQLiteConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, Database.DATABASE_PATH)))
                {
                    results.AddRange(await Task.Run(() => dbb.Query<T>(queryStatement)));
                }
            }
            catch (SQLiteException sle)
            {
                MessageBox.Show("Error while querying Database: \n" + sle.Message + "\n" + sle.StackTrace);
            }
            return results;
        }

        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }
    }
}

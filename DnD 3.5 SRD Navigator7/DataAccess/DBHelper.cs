using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Community.CsharpSqlite.SQLiteClient;
using Community.CsharpSqlite;
using System.IO.IsolatedStorage;
using System.Collections.ObjectModel;
using System.Windows;

namespace DnD_3._5_SRD_Navigator7.DataAccess
{
    /// <summary>
    /// Contains methods to handle all interactions with the database.
    /// </summary>
    public class DBHelper
    {
        private Community.CsharpSqlite.SQLiteClient.SqliteConnection db = null;

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
                    db = new SqliteConnection(Constants.Database.connectionString);
                    db.Open();
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
            Community.CsharpSqlite.FileStream.HandleTracker.Clear();
        }

        /// <summary>
        /// Select a single result using the passed in value to formulate a query.
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        public Model.Item Select(String statement)
        {
            Open();
            Model.Item result = null;
            try
            {
                SqliteCommand cmd = new SqliteCommand(statement, db);
                SqliteDataReader reader = cmd.ExecuteReader();  //all rows exist in this object only right now
                
                while (reader.Read())
                {
                    //value 0 = id
                    //value 1 = name
                    //value 2 = html
                    result = new Model.Item((int)reader.GetValue(0), (string)reader.GetValue(1), (string)reader.GetValue(2));
                }
            }
            catch (SqliteExecutionException ee)
            {
                System.Windows.MessageBox.Show("Error while querying Database: \n" + ee.Message + "\n" + ee.StackTrace);
            }
            catch (SqliteSyntaxException se)
            {
                System.Windows.MessageBox.Show("Error while querying Database: \n" + se.Message + "\n" + se.StackTrace);
            }
            catch (ArgumentException ae)
            {
                System.Windows.MessageBox.Show("Error while adding value to list: \n" + ae.Message + "\n" + ae.StackTrace);
            }
            finally
            {
                Close();
            }
            
            return result;
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

            try{
                SqliteCommand cmd = new SqliteCommand(statement, db);
                SqliteDataReader reader = cmd.ExecuteReader();  //all rows exist in this object only right now
                results = new List<Model.Item>();
                while (reader.Read())
                {
                    results.Add(new Model.Item((string)reader.GetValue(0)));
                }
            }
            catch (SqliteExecutionException ee)
            {
                System.Windows.MessageBox.Show("Error while querying Database: \n" + ee.Message + "\n" + ee.StackTrace);
            }
            catch (SqliteSyntaxException se)
            {
                System.Windows.MessageBox.Show("Error while querying Database: \n" + se.Message + "\n" + se.StackTrace);
            }
            catch (ArgumentException ae)
            {
                System.Windows.MessageBox.Show("Error while adding value to list: \n" + ae.Message + "\n" + ae.StackTrace);
            }
            finally
            {
                Close();
            }
            Close();
            return results;
        }
    }
}

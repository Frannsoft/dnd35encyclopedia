using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

namespace DnDNavigator.Runtime.DataAccess
{
    public class EntryDataService : IDataService
    {
        public async Task<List<TResult>> GetAllEntries<T, TResult>(string queryStatement) where T : new()
        {
            List<T> initialResults = new List<T>();
            using (DBHelper dbHelper = new DBHelper())
            {
                initialResults.AddRange(await dbHelper.GetEntries<T>(queryStatement));
            }

            List<TResult> results = new List<TResult>();
            foreach (T entity in initialResults)
            {
                results.Add(EntityToBusinessConverter<T>.ConvertTo<TResult>(entity));
            }
            return results;
        }

        public async Task<List<TResult>> SearchEntries<T, TResult>(string queryStatement, string userQueryText) where T : new()
        {
            List<T> initialResults = new List<T>();
            using (DBHelper dbHelper = new DBHelper())
            {
                string fullCommandText = BuildQuery(queryStatement, userQueryText);
                initialResults.AddRange(
                    await dbHelper.SearchEntries<T>(fullCommandText));
            }

            List<TResult> results = new List<TResult>();
            foreach (T entity in initialResults)
            {
                results.Add(EntityToBusinessConverter<T>.ConvertTo<TResult>(entity));
            }
            return results;
        }

        /// <summary>
        /// Get a single entry from the database as the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="queryStatement"></param>
        /// <param name="userQueryText"></param>
        /// <returns></returns>
        public TResult GetEntry<T, TResult>(string queryStatement, string userQueryText) where T : new()
        {
            //apostrophe fix
            userQueryText = userQueryText.Replace("'", "''");

            T returnedEntry = default(T);
            using (DBHelper dbHelper = new DBHelper())
            {
                string fullCommandText = BuildQuery(queryStatement, userQueryText);
                returnedEntry = dbHelper.GetEntry<T>(fullCommandText);
            }

            return EntityToBusinessConverter<T>.ConvertTo<TResult>(returnedEntry);
        }

        /// <summary>
        /// Get a single value from the database as the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryStatement"></param>
        /// <param name="userParams"></param>
        /// <remarks>I just want a string or other primitive back, but NO Sqlite for WP just HAS to only accept a parameterless cons Type</remarks>
        /// <returns></returns>
        public TResult GetSingleValue<T, TResult>(string queryStatement, params string[] userParams) where T : new()
        {
            T returnedValue = default(T);
            using(DBHelper dbHelper = new DBHelper())
            {
                string fullCommandText = BuildQuery(queryStatement, userParams);
                returnedValue = dbHelper.GetSingleValue<T>(fullCommandText);
            }

            return EntityToBusinessConverter<T>.ConvertTo<TResult>(returnedValue);
        }

        /// <summary>
        /// Tosses the user's value into the query since apparently sqlite on wp8 can't find ?'s 
        /// in LIKE statements with '%?%' surrounding them...
        /// </summary>
        /// <param name="baseQuery"></param>
        /// <param name="userQueryText"></param>
        /// <returns></returns>
        private string BuildQuery(string baseQuery, string userQueryText)
        {
            string result = string.Empty;

            result = baseQuery.Replace("?", @userQueryText);
            return result;
        }

        /// <summary>
        /// Takes in an array of params and sequentially (in order of first come first serve) applies them to the baseQuery when '?' is hit.  
        /// Returns a string with the filled in params from '?'.
        /// </summary>
        /// <param name="baseQuery"></param>
        /// <param name="userParams"></param>
        /// <returns></returns>
        private string BuildQuery(string baseQuery, params string[] userParams)
        {
            string result = string.Empty;
            int paramsCounter = 0;

            string[] characters = baseQuery.Select(c => c.ToString()).ToArray();

            for(int i = 0; i < characters.Length; i++)
            {
                if(characters[i].Equals("?"))
                {
                    characters[i] = userParams[paramsCounter];
                    paramsCounter++;
                }
            }

            result = string.Concat(characters);
            return result;
        }
    }
}

using DnDNavigator.Runtime.Constants;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.DataAccess.EntityObjects;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Sort;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace DnDNavigator.Runtime.Model.Search
{
    public class SearchService : ISearchService
    {
        private async Task<List<TModel>> GetSearchResults<TEntity, TModel>(string query, string userQueryText,
            Func<TModel, string> groupFunc) where TEntity : new()
        {
            EntryDataService eds = new EntryDataService();
            List<TModel> entries = await eds.SearchEntries<TEntity, TModel>(query, userQueryText);

            return entries;
        }

        public List<Group<T>> OrderItems<T>(List<T> entries, Func<T, string> groupFunc)
        {
            return PropertyKeyGroup<T>.GetItemGroupsByAlpha(entries, CultureInfo.CurrentUICulture, groupFunc);
        }

        /// <summary>
        /// Entry point for getting back search results.
        /// </summary>
        /// <param name="userQuery"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        public async Task<List<IEntry>> PerformSearch(string userQuery, IEnumerable<string> categories)
        {
            //ridiculousness due to sqlite lib not doing parameterized queries correctly imho
            userQuery = userQuery.Replace("'", "''");

            List<IEntry> results = new List<IEntry>();
            string baseQuery = string.Empty;

            foreach (string categoryName in categories)
            {
                //lower case since displayed names are proper noun cased.  Also removes pluralization
                switch (categoryName)
                {
                    case "monster":
                        {
                            baseQuery = DatabaseQueries.QUERY_MONSTER + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                            results.AddRange(await GetSearchResults<MonsterEntity, Monster>(baseQuery, userQuery, e => e.Name));
                            break;
                        }
                    case "spell":
                        {
                            baseQuery = DatabaseQueries.QUERY_SPELL + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                            results.AddRange(await GetSearchResults<SpellEntity, Spell>(baseQuery, userQuery, e => e.Name));
                            break;
                        }
                    case "domain":
                        {
                            baseQuery = DatabaseQueries.QUERY_DOMAIN + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                            results.AddRange(await GetSearchResults<DomainEntity, Domain>(baseQuery, userQuery, e => e.Name));
                            break;
                        }
                    case "class":
                        {
                            baseQuery = DatabaseQueries.QUERY_CLASS + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                            results.AddRange(await GetSearchResults<ClassEntity, Class>(baseQuery, userQuery, e => e.Name));
                            break;
                        }
                    case "equipment":
                        {
                            baseQuery = DatabaseQueries.QUERY_EQUIPMENT + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                            results.AddRange(await GetSearchResults<EquipmentEntity, Equipment>(baseQuery, userQuery, e => e.Name));
                            break;
                        }
                    case "feat":
                        {
                            baseQuery = DatabaseQueries.QUERY_FEAT + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                            results.AddRange(await GetSearchResults<FeatEntity, Feat>(baseQuery, userQuery, e => e.Name));
                            break;
                        }
                    case "item":
                        {
                            baseQuery = DatabaseQueries.QUERY_ITEM + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                            results.AddRange(await GetSearchResults<ItemEntity, DnDNavigator.Runtime.Model.DnDEntry.Item>(baseQuery, userQuery, e => e.Name));
                            break;
                        }
                    case "power":
                        {
                            baseQuery = DatabaseQueries.QUERY_POWER + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                            results.AddRange(await GetSearchResults<PowerEntity, Power>(baseQuery, userQuery, e => e.Name));
                            break;
                        }
                    case "skill":
                        {
                            baseQuery = DatabaseQueries.QUERY_SKILL + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                            results.AddRange(await GetSearchResults<SkillEntity, Skill>(baseQuery, userQuery, e => e.Name));
                            break;
                        }
                }
            }

            return results;
        }
    }
}

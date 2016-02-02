using DnDNavigator.Runtime.Constants;
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.DataAccess.EntityObjects;
using DnDNavigator.Runtime.Model.Entry;
using System;
using System.Collections.Generic;

namespace DnDNavigator.Runtime.Model.DnDEntry
{
    public class EntryService : IEntryService
    {
        public BaseEntry LoadEntryData()
        {
            BaseEntry baseEntry = IsolatedStorage.TempEntry;
            baseEntry = GetFullTextData(baseEntry);

            return baseEntry;
        }

        private BaseEntry GetFullTextData(BaseEntry entry)
        {
            BaseEntry tempEntry = default(BaseEntry);
            List<string> queryValues = new List<string>();
            EntryDataService eds = new EntryDataService();

            switch (GetEntryType(entry))
            {
                case EntryType.Types.Class:
                    {
                        queryValues.Add("class");
                        break;
                    }
                case EntryType.Types.Domain:
                    {
                        queryValues.Add("domain");
                        break;
                    }
                case EntryType.Types.Equipment:
                    {
                        queryValues.Add("equipment");
                        break;
                    }
                case EntryType.Types.Feat:
                    {
                        queryValues.Add("feat");
                        break;
                    }
                case EntryType.Types.Item:
                    {
                        queryValues.Add("item");
                        break;
                    }
                case EntryType.Types.Monster:
                    {
                        queryValues.Add("monster");
                        break;
                    }
                case EntryType.Types.Power:
                    {
                        queryValues.Add("power");
                        break;
                    }
                case EntryType.Types.Skill:
                    {
                        queryValues.Add("skill");
                        break;
                    }
                case EntryType.Types.Spell:
                    {
                        queryValues.Add("spell");
                        break;
                    }
            }

            //ridiculousness due to sqlite lib not doing parameterized queries correctly imho
            queryValues.Add(entry.Name.Replace("'", "''"));
            tempEntry = eds.GetSingleValue<BaseEntity, BaseEntry>(DatabaseQueries.QUERY_FULLTEXT, queryValues.ToArray());

            entry.Full_Text = tempEntry.Full_Text;
            return entry;
        }

        /// <summary>
        /// Gets the EntryType.Types enum value of the passed in BaseEntry object.  No exception handling
        /// done here because if this fails something has gone horribly wrong.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>EntryType.Types</returns>
        private EntryType.Types GetEntryType(BaseEntry entry)
        {
            EntryType.Types type = EntryType.Types.Unassigned;

            if (entry != null)
            {
                type = (EntryType.Types)Enum.Parse(typeof(EntryType.Types), entry.GetType().Name);
            }
            return type;
        }
    }
}

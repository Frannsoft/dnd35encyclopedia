using DnDNavigator.Runtime.DataAccess.EntityObjects;
using LegacyItem = DnDNavigator.Runtime.Model;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Model.Entry;
using System;
using DnDNavigator.Runtime.Constants;

namespace DnDNavigator.Runtime.DataAccess
{
    /// <summary>
    /// Provides a single entry point to convert a Legacy Item to a new Entry.
    /// </summary>
    public static class LegacyItemConversionService
    {
        private enum EntryType
        {
            Class,
            Domain,
            Equipment,
            Feat,
            Item,
            Monster,
            Power,
            Skill,
            Spell
        }

        /// <summary>
        /// Returns the legacy item passed as a properly typed new Entry.  The entry point for Legacy Item conversion.
        /// </summary>
        /// <param name="legacyItem"></param>
        /// <returns></returns>
        public static BaseEntry ConvertLegacyItemToEntry(LegacyItem.Item legacyItem)
        {
            return GetEntryFromLegacyItem(legacyItem);
        }

        /// <summary>
        /// Uses the passed in legacy item's data to determine it's EntryType and query the database for its data.
        /// </summary>
        /// <param name="legacyItem"></param>
        /// <returns></returns>
        private static BaseEntry GetEntryFromLegacyItem(LegacyItem.Item legacyItem)
        {
            var result = default(BaseEntry);

            EntryType entryType = DetermineLegacyItemType(legacyItem.ItemSQL);
            EntryDataService eds = new EntryDataService();

            switch (entryType)
            {
                case EntryType.Class:
                    {
                        string baseQuery = DatabaseQueries.QUERY_CLASS + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                        result = eds.GetEntry<ClassEntity, Class>(baseQuery, legacyItem.name);
                        break;
                    }
                case EntryType.Monster:
                    {
                        string baseQuery = DatabaseQueries.QUERY_MONSTER + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                        result = eds.GetEntry<MonsterEntity, Monster>(baseQuery, legacyItem.name);
                        break;
                    }
                case EntryType.Domain:
                    {
                        string baseQuery = DatabaseQueries.QUERY_DOMAIN + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                        result = eds.GetEntry<DomainEntity, Domain>(baseQuery, legacyItem.name);
                        break;
                    }
                case EntryType.Equipment:
                    {
                        string baseQuery = DatabaseQueries.QUERY_EQUIPMENT + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                        result = eds.GetEntry<EquipmentEntity, Equipment>(baseQuery, legacyItem.name);
                        break;
                    }
                case EntryType.Feat:
                    {
                        string baseQuery = DatabaseQueries.QUERY_FEAT + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                        result = eds.GetEntry<FeatEntity, Feat>(baseQuery, legacyItem.name);
                        break;
                    }
                case EntryType.Item:
                    {
                        string baseQuery = DatabaseQueries.QUERY_ITEM + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                        result = eds.GetEntry<ItemEntity, Item>(baseQuery, legacyItem.name);
                        break;
                    }
                case EntryType.Power:
                    {
                        string baseQuery = DatabaseQueries.QUERY_POWER + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                        result = eds.GetEntry<PowerEntity, Power>(baseQuery, legacyItem.name);
                        break;
                    }
                case EntryType.Skill:
                    {
                        string baseQuery = DatabaseQueries.QUERY_SKILL + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                        result = eds.GetEntry<SkillEntity, Skill>(baseQuery, legacyItem.name);
                        break;
                    }
                case EntryType.Spell:
                    {
                        string baseQuery = DatabaseQueries.QUERY_SPELL + DatabaseQueries.QUERY_CUSTOM_NAME_MODIFIER;
                        result = eds.GetEntry<SpellEntity, Spell>(baseQuery, legacyItem.name);
                        break;
                    }
            }

            return result;
        }

        /// <summary>
        /// Since the only determinate factor of a legacy Item is in its sql, uses that to get at the type as an Entry.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static EntryType DetermineLegacyItemType(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                if (sql.Contains("FROM monster"))
                {
                    return EntryType.Monster;
                }
                else if (sql.Contains("FROM spell"))
                {
                    return EntryType.Spell;
                }
                else if (sql.Contains("FROM class"))
                {
                    return EntryType.Class;
                }
                else if (sql.Contains("FROM domain"))
                {
                    return EntryType.Domain;
                }
                else if (sql.Contains("FROM equipment"))
                {
                    return EntryType.Equipment;
                }
                else if (sql.Contains("FROM feat"))
                {
                    return EntryType.Feat;
                }
                else if (sql.Contains("FROM item"))
                {
                    return EntryType.Item;
                }
                else if (sql.Contains("FROM power"))
                {
                    return EntryType.Power;
                }
                else if (sql.Contains("FROM skill"))
                {
                    return EntryType.Skill;
                }
                else
                {
                    throw new ArgumentException("Unable to determine legacy type from sql: \n" + sql);
                }
            }
            else
            {
                throw new ArgumentException("Invalid sql passed.  Unable to determine legacy type: \n" + sql);
            }
        }
    }
}

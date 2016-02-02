
namespace DnDNavigator.Runtime.Constants
{
    public static class Database
    {
        public const string DATABASE_PATH = "dnd35.sqlite";

        /// <summary>
        /// Stores the current version (not schema) of the DB.  Used to track updates made to the actual data.
        /// 12/29/13 - Removed duplicate entries in ITEM table with DELETE FROM item WHERE id NOT IN (SELECT MIN(id) FROM item GROUP BY name)
        /// 11-2-14 - Cleaned up empty entries in item table.  These were largely duplicates of entries in the spell table.
        ///     --verify with this one
        ///SELECT name, full_text FROM item WHERE name LIKE '%scroll%'
        ///--use this one
        ///UPDATE item SET name = name || ' (Scroll)' WHERE name IN (SELECT i.name FROM item i INNER JOIN spell s ON i.name = s.name)
        /// </summary>
        public const int DB_CURRENT_VERSION = 5;
    }
}

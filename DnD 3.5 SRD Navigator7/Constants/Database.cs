using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnD_3._5_SRD_Navigator7.Constants
{
    internal static class Database
    {
        public const string connectionString = @"Version=3,uri=file:" + databasePath;
        public const string databasePath = "dnd35.sqlite";

        /// <summary>
        /// Stores the current version (not schema) of the DB.  Used to track updates made to the actual data.
        /// 12/29/13 - Removed duplicate entries in ITEM table with DELETE FROM item WHERE id NOT IN (SELECT MIN(id) FROM item GROUP BY name)
        /// </summary>
        public const int DB_CURRENT_VERSION = 3;
    }
}

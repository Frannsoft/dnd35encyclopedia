using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnD_3._5_SRD_Navigator8.Model
{
    /// <summary>
    /// The Business class for an entry returned from the database.
    /// </summary>
    public class Item
    {
        private int _id = 0;
        private string _sql = string.Empty;

        /// <summary>
        /// The actual name of the entry.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// The full HTML of the entry.
        /// </summary>
        public string full_text { get; set; }

        public string ItemSQL
        {
            set { _sql = value; }
            get { return _sql; }
        }

        public Item(string _name, string query)
        {
            name = _name;
            _sql = query;
        }

        public Item(int _dbID, string _name, string _fullText)
        {
            _id = _dbID;
            name = _name;
        }

        public Item(string _name, string _query, string _fullText)
        {
            name = _name;
            _sql = _query;
        }

        public Item(string _name)
        {
            name = _name;
        }

        public Item()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnD_3._5_SRD_Navigator7.Model
{
    public class Item
    {
        string _thename = string.Empty;
        private int _id = 0;
        private string _sql = string.Empty;
        private string _html = string.Empty;

        public string HTML
        {
            set { _html = value; }
            get { return _html; }
        }

        public string ItemName
        {
            set { _thename = value; }
            get { return _thename; }
        }

        public string ItemSQL
        {
            set { _sql = value; }
            get { return _sql; }
        }

        public Item(string _name, string query)
        {
            _thename = _name;
            _sql = query;
        }

        public Item(int _dbID, string _name, string _fullText)
        {
            _id = _dbID;
            _thename = _name;
            _html = _fullText;
        }

        public Item(string _name, string _query, string _fullText)
        {
            _thename = _name;
            _html = _fullText;
            _sql = _query;
        }

        public Item(string _name)
        {
            _thename = _name;
        }

        public Item()
        {

        }
    }
}

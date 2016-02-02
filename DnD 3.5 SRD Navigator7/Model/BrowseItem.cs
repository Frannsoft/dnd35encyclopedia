using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnD_3._5_SRD_Navigator7.Model
{
    public class BrowseItem
    {
        string _thename;
        public string sql;

        public string ItemName
        {
            set { _thename = value; }
            get { return _thename; }
        }

        public BrowseItem(string itemName)
        {
            ItemName = itemName;
        }
    }
}

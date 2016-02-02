using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnD_3._5_SRD_Navigator7.Model
{
    public class ItemResult
    {
        string _itemName;
        public string html ;

        public string Name
        {
            set { _itemName = value; }
            get { return _itemName; }
        }

        public ItemResult()
        {

        }

        public ItemResult(string Name)
        {
            _itemName = Name;
        }

        public ItemResult(int ID, string Name, string Html)
        {
            _itemName = Name;
            html = Html;
        }
    }
}

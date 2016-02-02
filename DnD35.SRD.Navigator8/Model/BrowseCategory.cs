using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnD_3._5_SRD_Navigator8.Model
{
    internal class BrowseCategory : IEnumerable
    {
        private string _name;

        public string Name
        {
            set{_name = value;}
            get{return _name;}
        }

        public List<Item> Items { get; private set; }

        public BrowseCategory(string categoryName)
        {
            Name = categoryName;
            Items = new List<Item>();
        }

        public BrowseCategory()
        {
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public IEnumerator GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }
    }
}

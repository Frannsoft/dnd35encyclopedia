
using DnDNavigator.Runtime.Model.Entry;
using System.Runtime.Serialization;
namespace DnDNavigator.Runtime.Model
{
    /// <summary>
    /// The Business class for an entry returned from the database. LEGACY!!
    /// </summary>
    [DataContract(Name = "Item")]
    public class Item : BaseEntry
    {
        private int id = 0;
        private string sql = string.Empty;

        /// <summary>
        /// The actual name of the entry.
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        /// The full HTML of the entry.
        /// </summary>
        [DataMember]
        public string full_text { get; set; }

        [DataMember]
        public string ItemSQL
        {
            set { sql = value; }
            get { return sql; }
        }


        public Item(string name, string query)
        {
            this.name = name;
            this.Name = name;
            this.sql = query;
        }

        public Item(int dbID, string name, string fullText)
        {
            this.id = dbID;
            this.name = name;
            this.Name = name;
        }

        public Item(string name, string query, string fullText)
        {
            this.name = name;
            this.Name = name;
            this.sql = query;
        }

        public Item(string name)
        {
            this.name = name;
            this.Name = name;
        }

        public Item()
        { }

        public override bool Equals(object obj)
        {
            bool result = false;

            if(obj != null)
            {
                Item compItem = obj as Item;
                if(compItem != null)
                {
                    result = this.name.Equals(compItem.name);
                }
            }
            return result;
        }

        public override int GetHashCode()
        {
            return new { this.name }.GetHashCode();
        }

        public string GetExtraText(string xpath)
        {
            throw new System.NotImplementedException();
        }
    }
}

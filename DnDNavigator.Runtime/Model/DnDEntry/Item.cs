
using DnDNavigator.Runtime.Model.Entry;
using System.Runtime.Serialization;
namespace DnDNavigator.Runtime.Model.DnDEntry
{
    [DataContract(Name = "Item")]
    [KnownType(typeof(Item))]
    public class Item : BaseEntry
    {
        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public string Subcategory { get; set; }

        [DataMember]
        public string Special_Ability { get; set; }

        [DataMember]
        public string Aura { get; set; }

        [DataMember]
        public string Caster_Level { get; set; }

        [DataMember]
        public string Price { get; set; }

        [DataMember]
        public string Manifester_Level { get; set; }

        [DataMember]
        public string Prereq { get; set; }

        [DataMember]
        public string Cost { get; set; }

        [DataMember]
        public string Weight { get; set; }

        [DataMember]
        public string Full_Text { get; set; }
    }
}

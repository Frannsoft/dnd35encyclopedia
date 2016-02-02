using DnDNavigator.Runtime.Model.Entry;
using System.Runtime.Serialization;

namespace DnDNavigator.Runtime.Model.DnDEntry
{
    [DataContract(Name = "Equipment")]
    [KnownType(typeof(Equipment))]
    public class Equipment : BaseEntry
    {
        [DataMember]
        public string Family { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public string Subcategory { get; set; }

        [DataMember]
        public string Cost { get; set; }

        [DataMember]
        public string Dmg_s { get; set; }

        [DataMember]
        public string Armor_Shield_Bonus { get; set; }

        [DataMember]
        public string Maximum_Dex_Bonus { get; set; }

        [DataMember]
        public string Dmg_m { get; set; }

        [DataMember]
        public string Weight { get; set; }

        [DataMember]
        public string Critical { get; set; }

        [DataMember]
        public string Armor_Check_Penalty { get; set; }

        [DataMember]
        public string Arcane_Spell_Failure_Chance { get; set; }

        [DataMember]
        public string Range_Increment { get; set; }

        [DataMember]
        public string Speed_30 { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Speed_20 { get; set; }

        [DataMember]
        public string Full_Text { get; set; }

        [DataMember]
        public int ID { get; set; }
    }
}

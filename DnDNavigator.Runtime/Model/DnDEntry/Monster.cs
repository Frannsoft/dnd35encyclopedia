
using DnDNavigator.Runtime.Model.Entry;
using System.Runtime.Serialization;
namespace DnDNavigator.Runtime.Model.DnDEntry
{
    [DataContract(Name = "Monster")]
    [KnownType(typeof(Monster))]
    public class Monster : BaseEntry
    {
        [DataMember]
        public string Size { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Challenge_Rating { get; set; }

        [DataMember]
        public string Hit_Dice { get; set; }

        [DataMember]
        public string Initiative { get; set; }

        [DataMember]
        public string Speed { get; set; }

        [DataMember]
        public string Armor_Class { get; set; }

        [DataMember]
        public string Base_Attack { get; set; }

        [DataMember]
        public string Grapple { get; set; }

        [DataMember]
        public string Full_Attack { get; set; }

        [DataMember]
        public string Space { get; set; }

        [DataMember]
        public string Reach { get; set; }

        [DataMember]
        public string Special_Attacks { get; set; }

        [DataMember]
        public string Special_Qualities { get; set; }

        [DataMember]
        public string Family { get; set; }

        [DataMember]
        public string Saves { get; set; }

        [DataMember]
        public string Abilities { get; set; }

        [DataMember]
        public string Skills { get; set; }

        [DataMember]
        public string Bonus_Feats { get; set; }

        [DataMember]
        public string Feats { get; set; }

        [DataMember]
        public string Epic_Feats { get; set; }

        [DataMember]
        public string Environment { get; set; }

        [DataMember]
        public string Organization { get; set; }

        [DataMember]
        public string Treasure { get; set; }

        [DataMember]
        public string Alignment { get; set; }

        [DataMember]
        public string Advancement { get; set; }

        [DataMember]
        public string Level_Adjustment { get; set; }

        [DataMember]
        public string Special_Abilities { get; set; }

    }
}


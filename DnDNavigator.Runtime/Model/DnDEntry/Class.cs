using DnDNavigator.Runtime.Model.Entry;
using System.Runtime.Serialization;

namespace DnDNavigator.Runtime.Model.DnDEntry
{
    [DataContract(Name = "Class")]
    [KnownType(typeof(Class))]
    public class Class : BaseEntry
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Alignment { get; set; }

        [DataMember]
        public string Hit_Die { get; set; }

        [DataMember]
        public string Class_Skills { get; set; }

        [DataMember]
        public string Skill_Points { get; set; }

        [DataMember]
        public string Skill_Points_Ability { get; set; }

        [DataMember]
        public string Spell_Stat { get; set; }

        [DataMember]
        public string Proficiencies { get; set; }

        [DataMember]
        public string Spell_Type { get; set; }

        [DataMember]
        public string Epic_Feat_Base_Level { get; set; }

        [DataMember]
        public string Epic_Feat_Interval { get; set; }

        [DataMember]
        public string Req_Race { get; set; }

        [DataMember]
        public string Req_Weapon_Proficiency { get; set; }

        [DataMember]
        public string Req_Base_Attack_Bonus { get; set; }

        [DataMember]
        public string Req_Skill { get; set; }

        [DataMember]
        public string Req_Feat { get; set; }

        [DataMember]
        public string Req_Spells { get; set; }

        [DataMember]
        public string Req_Languages { get; set; }

        [DataMember]
        public string Req_Psionics { get; set; }

        [DataMember]
        public string Req_Epic_Feat { get; set; }

        [DataMember]
        public string Req_Special { get; set; }

        [DataMember]
        public string Spell_List_1 { get; set; }

        [DataMember]
        public string Spell_List_2 { get; set; }

        [DataMember]
        public string Spell_List_3 { get; set; }

        [DataMember]
        public string Spell_List_4 { get; set; }

        [DataMember]
        public string Spell_List_5 { get; set; }

    }
}

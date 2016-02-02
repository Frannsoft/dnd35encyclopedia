
using DnDNavigator.Runtime.Model.Entry;
using System.Runtime.Serialization;
namespace DnDNavigator.Runtime.Model.DnDEntry
{
    [DataContract(Name = "Skill")]
    [KnownType(typeof(Skill))]
    public class Skill : BaseEntry
    {
        [DataMember]
        public string SubType { get; set; }

        [DataMember]
        public string Key_Ability { get; set; }

        [DataMember]
        public string Psionic { get; set; }

        [DataMember]
        public string Trained { get; set; }

        [DataMember]
        public string Armor_Check { get; set; }

        [DataMember]
        public string Skill_Check { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public string Try_Again { get; set; }

        [DataMember]
        public string Special { get; set; }

        [DataMember]
        public string Restriction { get; set; }

        [DataMember]
        public string Synergy { get; set; }

        [DataMember]
        public string Untrained { get; set; }
    }
}

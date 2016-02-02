using DnDNavigator.Runtime.Model.Entry;
using System.Runtime.Serialization;

namespace DnDNavigator.Runtime.Model.DnDEntry
{
    [DataContract(Name = "Domain")]
    [KnownType(typeof(Domain))]
    public class Domain : BaseEntry
    {
        [DataMember]
        public string Granted_Powers { get; set; }

        [DataMember]
        public string Spell_1 { get; set; }

        [DataMember]
        public string Spell_2 { get; set; }

        [DataMember]
        public string Spell_3 { get; set; }

        [DataMember]
        public string Spell_4 { get; set; }

        [DataMember]
        public string Spell_5 { get; set; }

        [DataMember]
        public string Spell_6 { get; set; }

        [DataMember]
        public string Spell_7 { get; set; }

        [DataMember]
        public string Spell_8 { get; set; }

        [DataMember]
        public string Spell_9 { get; set; }
    }
}

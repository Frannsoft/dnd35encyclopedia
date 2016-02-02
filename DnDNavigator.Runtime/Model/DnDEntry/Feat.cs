using DnDNavigator.Runtime.Model.Entry;
using System.Runtime.Serialization;

namespace DnDNavigator.Runtime.Model.DnDEntry
{
    [DataContract(Name = "Feat")]
    [KnownType(typeof(Feat))]
    public class Feat : BaseEntry
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Stack { get; set; }

        [DataMember]
        public string Choice { get; set; }

        [DataMember]
        public string Prerequisite { get; set; }

        [DataMember]
        public string Benefit { get; set; }

        [DataMember]
        public string Normal { get; set; }

        [DataMember]
        public string Special { get; set; }

        [DataMember]
        public int ID { get; set; }
    }
}

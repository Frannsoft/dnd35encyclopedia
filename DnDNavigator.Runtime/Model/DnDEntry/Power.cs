using DnDNavigator.Runtime.Model.Entry;
using System.Runtime.Serialization;

namespace DnDNavigator.Runtime.Model.DnDEntry
{
    [DataContract(Name = "Power")]
    [KnownType(typeof(Power))]
    public class Power : BaseEntry
    {
        [DataMember]
        public string Discipline { get; set; }

        [DataMember]
        public string Subdiscipline { get; set; }

        [DataMember]
        public string Level { get; set; }

        [DataMember]
        public string Display { get; set; }

        [DataMember]
        public string Manifesting_Time { get; set; }

        [DataMember]
        public string Range { get; set; }

        [DataMember]
        public string Target { get; set; }

        [DataMember]
        public string Area { get; set; }

        [DataMember]
        public string Effect { get; set; }

        [DataMember]
        public string Duration { get; set; }

        [DataMember]
        public string Saving_Throw { get; set; }

        [DataMember]
        public string Power_Points { get; set; }

        [DataMember]
        public string Power_Resistance { get; set; }

        [DataMember]
        public string Short_Description { get; set; }

        [DataMember]
        public string Xp_Cost { get; set; }

        /// <summary>
        /// Needs html stripped.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Needs html stripped.
        /// </summary>
        [DataMember]
        public string Augment { get; set; }
    }
}

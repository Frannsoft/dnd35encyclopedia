using DnDNavigator.Runtime.Model.Entry;
using System.Runtime.Serialization;

namespace DnDNavigator.Runtime.Model.DnDEntry
{
    [DataContract(Name = "Spell")]
    [KnownType(typeof(Spell))]
    public class Spell : BaseEntry
    {
        [DataMember]
        public string School { get; set; }

        [DataMember]
        public string SubSchool { get; set; }

        [DataMember]
        public string SpellCraftDC { get; set; }

        [DataMember]
        public string Level { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Components { get; set; }

        [DataMember]
        public string Casting_Time { get; set; }

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
        public string Spell_Resistance { get; set; }

        [DataMember]
        public string ToDevelop { get; set; }

        [DataMember]
        public string MaterialComponents { get; set; }

        [DataMember]
        public string ArcaneMaterialComponents { get; set; }

        [DataMember]
        public string Focus { get; set; }

        [DataMember]
        public string XPCost { get; set; }

        [DataMember]
        public string ArcaneFocus { get; set; }

        [DataMember]
        public string WizardFocus { get; set; }

        [DataMember]
        public string VerbalComponents { get; set; }

        [DataMember]
        public string SorcererFocus { get; set; }

        [DataMember]
        public string BardFocus { get; set; }

        [DataMember]
        public string ClericFocus { get; set; }

        [DataMember]
        public string DruidFocus { get; set; }
    }
}

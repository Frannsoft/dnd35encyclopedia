
namespace DnDNavigator.Runtime.DataAccess.EntityObjects
{
    public class SkillEntity
    {
        public string name { get; set; }
        public string subtype { get; set; }
        public string key_ability { get; set; }
        public string psionic { get; set; }
        public string trained { get; set; }
        public string armor_check { get; set; }
        public string skill_check { get; set; }
        public string action { get; set; }
        public string try_again { get; set; }
        public string special { get; set; }
        public string restriction { get; set; }
        public string synergy { get; set; }
        public string untrained { get; set; }

        public SkillEntity()
        { }
    }
}

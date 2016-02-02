
namespace DnDNavigator.Runtime.Constants
{
    /// <summary>
    /// I currently have no need for anything other than a single static SELECT query to retrieve information
    /// from the database.  Therefore, all queries reside here for use.  
    /// </summary>
    public static class DatabaseQueries
    {
        /// <summary>
        /// Has a '?' where the user argument should be passed in.
        /// </summary>
        public const string QUERY_CUSTOM_NAME_MODIFIER = " WHERE name LIKE '%?%'";

        public const string QUERY_CLASS = "SELECT name, type, alignment, hit_die, class_skills, skill_points, skill_points_ability, spell_stat, proficiencies," +
            "spell_type, epic_feat_base_level, epic_feat_interval, req_race, req_weapon_proficiency, req_base_attack_bonus, req_skill, req_feat, req_spells," +
            "req_languages, req_psionics, req_epic_feat, req_special, spell_list_1, spell_list_2, spell_list_3, spell_list_4, spell_list_5 FROM class";

        public const string QUERY_DOMAIN = "SELECT name, granted_powers, spell_1, spell_2, spell_3, spell_4, spell_5, spell_6, spell_7," +
            "spell_8, spell_9 from domain";

        public const string QUERY_EQUIPMENT = "SELECT name, family, category, subcategory, cost, dmg_s, armor_shield_bonus, maximum_dex_bonus," +
            "dmg_m, weight, critical, armor_check_penalty, arcane_spell_failure_chance, range_increment, speed_30, type, speed_20, full_text FROM equipment";

        public const string QUERY_FEAT = "SELECT name, type, stack, choice, prerequisite, benefit, normal, special FROM feat";

        public const string QUERY_ITEM = "SELECT name, category, subcategory, special_ability, aura, caster_level, price, manifester_level, prereq," +
            "cost, weight, full_text FROM item";

        public const string QUERY_MONSTER = "SELECT family, name, size, type, hit_dice, initiative, speed, armor_class, base_attack, grapple, full_attack, space," +
            "reach, special_attacks, special_qualities, saves, abilities, skills, bonus_feats, feats, epic_feats, environment, organization, challenge_rating," +
            "treasure, alignment, advancement, level_adjustment, special_abilities FROM monster";

        public const string QUERY_POWER = "SELECT name, discipline, subdiscipline, level, display, manifesting_time, range," +
            "target, area, effect, duration, saving_throw, power_points, power_resistance, short_description, xp_cost, description FROM power";

        public const string QUERY_SKILL = "SELECT name, subtype, key_ability, psionic, trained, armor_check, skill_check, action," +
            "try_again, special, restriction, synergy, untrained FROM skill";

        public const string QUERY_SPELL = "SELECT name, school, subschool, spellcraft_dc, level, components, casting_time, range, target, area, effect, duration," +
            "saving_throw, spell_resistance, to_develop, material_components, arcane_material_components, focus, xp_cost, arcane_focus, wizard_focus," +
            "verbal_components, sorcerer_focus, bard_focus, cleric_focus, druid_focus FROM spell";

        public const string QUERY_FULLTEXT = "SELECT full_text FROM ?" + QUERY_CUSTOM_NAME_MODIFIER;

    }
}


using DnDNavigator.Runtime.Model;
using DnDNavigator.Runtime.Model.DnDEntry;
using System;
using System.Windows;
using LegacyItem = DnDNavigator.Runtime.Model.Item;
namespace DnD35.SRD.Navigator8.Styles
{
    /// <summary>
    /// Used in BrowseListPage.xaml as the root for all possible datatemplate that can be used in the View.
    /// </summary>
    public class EntryDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Monster { get; set; }
        public DataTemplate Spell { get; set; }
        public DataTemplate Domain { get; set; }
        public DataTemplate Skill { get; set; }
        public DataTemplate Power { get; set; }
        public DataTemplate Class { get; set; }
        public DataTemplate Feat { get; set; }
        public DataTemplate Item { get; set; }
        public DataTemplate Equipment { get; set; }
        public DataTemplate OldItem { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                if (item is Monster)
                {
                    Monster monsterItem = item as Monster;
                    return Monster;

                }
                else if (item is Spell)
                {
                    Spell spellItem = item as Spell;
                    return Spell;
                }
                else if (item is Domain)
                {
                    Domain domainItem = item as Domain;
                    return Domain;
                }
                else if(item is Skill)
                {
                    Skill skillItem = item as Skill;
                    return Skill;
                }
                else if(item is Power)
                {
                    Power powerItem = item as Power;
                    return Power;
                }
                else if(item is Class)
                {
                    Class classItem = item as Class;
                    return Class;
                }
                else if(item is Feat)
                {
                    Feat featItem = item as Feat;
                    return Feat;
                }
                else if(item is DnDNavigator.Runtime.Model.DnDEntry.Item)
                {
                    DnDNavigator.Runtime.Model.DnDEntry.Item itemItem = item as DnDNavigator.Runtime.Model.DnDEntry.Item;
                    return Item;
                }
                else if(item is Equipment)
                {
                    Equipment equipmentItem = item as Equipment;
                    return Equipment;
                }
                else if (item is LegacyItem)
                {
                    LegacyItem oldItem = item as LegacyItem;
                    return OldItem;
                }
                else
                {
                    //TODO - what happens if no template is returned?
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}

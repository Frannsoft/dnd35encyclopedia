
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Model.Menu;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DnD35.SRD.Navigator8.Design
{
    public class DesignMenuOptionsService : IMenuOptionsService
    {
        public async Task<IEnumerable<MenuOption>> RefreshHomeMenu()
        {
            return new List<MenuOption>(){
                new MenuOption("Monsters", "The creatures of DND 3.5.", EntryType.Types.Monster),
                new MenuOption("Spells", "Potent (and some not so potent) spells.", EntryType.Types.Spell),
                new MenuOption("Classes", "What can your Class(es) do?", EntryType.Types.Class),
                new MenuOption("Domains", "Domain specific information.", EntryType.Types.Domain),
                new MenuOption("Equipment", "The Armory.", EntryType.Types.Equipment),
                new MenuOption("Items", "Items acquired on your adventure.", EntryType.Types.Item),
                new MenuOption("Powers", "Powers mighty to behold.", EntryType.Types.Power),
                new MenuOption("Skills", "What makes you a critical part of the team?", EntryType.Types.Skill)
            };
        }


        public Task<string> ShowReleaseNotes()
        {
            throw new System.NotImplementedException();
        }
    }
}

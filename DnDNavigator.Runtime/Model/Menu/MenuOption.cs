
using DnDNavigator.Runtime.Model.DnDEntry;
using Newtonsoft.Json;
namespace DnDNavigator.Runtime.Model.Menu
{
    public class MenuOption
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("EntryType")]
        public EntryType.Types EntryType { get; set; }

        public MenuOption(string name, string description, EntryType.Types entryType)
        {
            Name = name;
            Description = description;
            EntryType = entryType;
        }
    }
}


namespace DnDNavigator.Runtime.Model.Search
{
    public class SearchOption
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public SearchOption(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}

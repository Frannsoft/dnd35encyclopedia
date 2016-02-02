using System.Collections.Generic;

namespace DnDNavigator.Runtime.Sort
{
    public class Group<T> : List<T>
    {
        public string Title { get; set; }

        public Group(string name, IEnumerable<T> items) : base(items)
        {
            Title = name;
        }

        public Group(string name) : base(new List<T>())
        {
            Title = name;
        }

    }
}

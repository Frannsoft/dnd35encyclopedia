
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Sort;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace DnDNavigator.Runtime.Model.Browse
{
    public interface IBrowseListService<T>
    {
       Task<List<Group<T>>> GetEntries();
    }
}

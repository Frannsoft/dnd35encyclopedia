using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Phone.Globalization;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace DnDNavigator.Runtime.Sort
{
    public class PropertyKeyGroup<T> : List<T>
    {


        public static List<Group<T>> GetItemGroupsBySpecialOrderAndFilter(IEnumerable<T> itemList, Func<T, string> getKeyFunc)
        {
            IEnumerable<Group<T>> groupList = from item in itemList
                                              group item by getKeyFunc(item) into g
                                              orderby g.Key
                                              select new Group<T>(g.Key.ToUpper(), g);
            return groupList.ToList();
        }

        public static List<Group<T>> GetItemGroupsBySpecialOrder(IEnumerable<T> itemList, Func<T, string> getKeyFunc)
        {
            IEnumerable<Group<T>> groupList = from item in itemList
                                              group item by getKeyFunc(item) into g
                                              orderby g.Key
                                              select new Group<T>(g.Key, g);
            return groupList.ToList();
        }

        public static List<Group<T>> GetItemGroupsByAlpha(IEnumerable<T> itemList, CultureInfo ci, Func<T, string> getKeyFunc)
        {
            SortedLocaleGrouping slg = new SortedLocaleGrouping(ci);

            List<Group<T>> list = new List<Group<T>>();
            foreach (string key in slg.GroupDisplayNames)
            {
                list.Add(new Group<T>(key.ToUpper()));
            }

            foreach (T item in itemList)
            {
                int index = 0;
                if (slg.SupportsPhonetics)
                { }
                else
                {
                    index = slg.GetGroupIndex(getKeyFunc(item));
                }
                if (index >= 0 && index < list.Count())
                {
                    list[index].Add(item);
                }
            }

            foreach (Group<T> group in list)
            {
                group.Sort((c0, c1) => { return ci.CompareInfo.Compare(getKeyFunc(c0), getKeyFunc(c1)); });
            }

            return list;
        }
    }


}

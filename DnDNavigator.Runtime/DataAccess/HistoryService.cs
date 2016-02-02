using DnDNavigator.Runtime.Model.Entry;
using DnDNavigator.Runtime.Sort;
using System.Collections.Generic;
using System.Globalization;

namespace DnDNavigator.Runtime.DataAccess
{
    public class HistoryService : IHistoryService
    {
        /// <summary>
        /// Add the passed in item to the history list if it isn't already in there.
        /// </summary>
        /// <param name="entry"></param>
        public void AddToHistory(BaseEntry entry)
        {
            if (!IsInHistory(entry))//item does not exist in history, so add it
            {
                IsolatedStorage.History.Add(entry);
            }
        }

        public void ClearHistory()
        {
            IsolatedStorage.History = new List<BaseEntry>();
        }

        public List<Group<BaseEntry>> GetGroupedHistoryItems()
        {
            List<BaseEntry> items = IsolatedStorage.History;

            return PropertyKeyGroup<BaseEntry>.GetItemGroupsByAlpha(items, CultureInfo.CurrentUICulture, e => e.Name);
        }

        /// <summary>
        /// Checks to see if the passed in Item already exists in the History.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsInHistory(BaseEntry item)
        {
            var tempList = GetGroupedHistoryItems();

            bool inHistory = false;
            foreach (Group<BaseEntry> group in tempList)
            {
                foreach (BaseEntry baseEntry in group)
                {
                    if (baseEntry.Name.Equals(item.Name))
                    {
                        inHistory = true;
                        break; //we're done here, exit the loop
                    }
                }
            }
            return inHistory;
        }
    }
}

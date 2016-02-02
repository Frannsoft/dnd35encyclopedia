using DnDNavigator.Runtime.Model.Entry;
using DnDNavigator.Runtime.Sort;
using System.Collections.Generic;
using System.Globalization;

namespace DnDNavigator.Runtime.DataAccess
{
    public class FavoriteService : IFavoriteService
    {
        public void AddToFavorites(BaseEntry entry)
        {
            if (!IsInFavorites(entry))
            {
                IsolatedStorage.Favorites.Add(entry);
            }
        }

        public void RemoveFromFavorites(BaseEntry entry)
        {
            if (entry != null && IsInFavorites(entry))
            {
                IsolatedStorage.Favorites.Remove(entry);
            }
        }

        /// <summary>
        /// Checks to see if the passed in Entry already exists in the Favorites.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool IsInFavorites(BaseEntry entry)
        {
            var tempList = GetGroupedFavoriteItems();

            bool inFavorites = false;
            foreach (Group<BaseEntry> group in tempList)
            {
                foreach (BaseEntry baseEntry in group)
                {
                    if (baseEntry.Name.Equals(entry.Name))
                    {
                        inFavorites = true;
                        break; //we're done here, exit the loop
                    }
                }
            }

            return inFavorites;
        }

        /// <summary>
        /// Removes all of the passed favorites from the stored favorites.
        /// </summary>
        /// <param name="favorites"></param>
        public void DeleteFavorites(List<BaseEntry> favorites)
        {
            var existingFavorites = GetFavoritesAsList();

            foreach (BaseEntry favoriteEntry in favorites)
            {
                if (existingFavorites.Contains(favoriteEntry))
                {
                    existingFavorites.Remove(favoriteEntry);
                }
            }
        }

        /// <summary>
        /// For internal use only.  Gets the favorites as a non grouped list.
        /// </summary>
        /// <returns></returns>
        private List<BaseEntry> GetFavoritesAsList()
        {
            List<BaseEntry> items = IsolatedStorage.Favorites;

            return items;
        }

        /// <summary>
        /// Gets the favorites as a grouped list ready to display to the user.  Use this when looking
        /// for something to display the favorites.
        /// </summary>
        /// <returns></returns>
        public List<Group<BaseEntry>> GetGroupedFavoriteItems()
        {
            List<BaseEntry> items = IsolatedStorage.Favorites;

            return PropertyKeyGroup<BaseEntry>.GetItemGroupsByAlpha(items, CultureInfo.CurrentUICulture, e => e.Name);
        }
    }
}

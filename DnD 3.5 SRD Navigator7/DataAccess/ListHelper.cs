using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DnD_3._5_SRD_Navigator7.DataAccess
{
    internal class ListHelper
    {
        public List<Model.BrowseCategory> populateList(List<Model.Item> inputList)
        {
            List<Model.BrowseCategory> browseCategories = new List<Model.BrowseCategory>();
            Model.BrowseCategory currBrowseCategory = null;
            List<Model.Item> currCategoryResults = null;
            Model.Item bItem = null;

            // - loop through the 1 - 10 number set - or use ifDigit
            for (int k = 0; k < 10; k++) //loop through the alphabet
            {
                currBrowseCategory = new Model.BrowseCategory((k + 1).ToString()); //increment by 1 because no category starts with 0
                currCategoryResults = inputList.Where(Item => Item.ItemName.StartsWith((k + 1).ToString())).ToList(); //get a list of the current group

                foreach (Model.Item i in currCategoryResults) //loop through returned list for the specific category
                {
                    bItem = new Model.Item(i.ItemName, i.ItemSQL); //create a new specific item to be added to the specific category list
                    currBrowseCategory.AddItem(bItem); //add the item to the specific category list
                }
                if (currBrowseCategory.Items.Count > 0) //if there is at least one result
                {
                    browseCategories.Add(currBrowseCategory);
                }
            }

            char alphabetLooper = 'A';
            for (int k = 0; k < 26; k++) //loop through the alphabet
            {
                currBrowseCategory = new Model.BrowseCategory(alphabetLooper.ToString());
                currCategoryResults = inputList.Where(Item => Item.ItemName.StartsWith(alphabetLooper.ToString())).ToList(); //get a list of the current group
                bItem = null;

                foreach (Model.Item i in currCategoryResults) //loop through returned list for the specific category
                {
                    bItem = new Model.Item(i.ItemName, i.ItemSQL); //create a new specific item to be added to the specific category list
                    currBrowseCategory.AddItem(bItem); //add the item to the specific category list
                }
                if (currBrowseCategory.Items.Count > 0) //if there is at least one result
                {
                    browseCategories.Add(currBrowseCategory);
                }
                alphabetLooper++; //go to next alphabet letter
            }

            return browseCategories;
        }
    }
}

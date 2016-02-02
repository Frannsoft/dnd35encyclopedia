using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DnD_3._5_SRD_Navigator8.DataAccess
{
    class HistoryHandler : INotifyPropertyChanged
    {
        private List<Model.Item> _search = new List<Model.Item>();
        public List<Model.Item> searchHistory
        {
            get { return _search; }
            set { _search = value; }
        }

     
        public HistoryHandler()
        {
            //get existing searchhistory if exists and set it to the local property
            DataAccess.IsoStorageHelper iHelper = new IsoStorageHelper();
            if (iHelper.settings.Contains(Constants.IsolatedStorage.ISO_PREVIOUS_SEARCHES))
            {
                _search = (List<Model.Item>)iHelper.settings[Constants.IsolatedStorage.ISO_PREVIOUS_SEARCHES];
            }
            else
            {
                iHelper.settings[Constants.IsolatedStorage.ISO_PREVIOUS_SEARCHES] = _search;
            }
        }

        /// <summary>
        /// Add the passed in item to the history list if it isn't already in there.
        /// </summary>
        /// <param name="_item"></param>
        public void addtoHistory(Model.Item _item)
        {
            if (!isInHistory(_item))//item does not exist in history, so add it
            {
                searchHistory.Add(_item);
                new DataAccess.IsoStorageHelper().storeValues(Constants.IsolatedStorage.ISO_PREVIOUS_SEARCHES, searchHistory);
            }
        }

        /// <summary>
        /// Checks to see if the passed in Item already exists in the History.
        /// </summary>
        /// <param name="_item"></param>
        /// <returns></returns>
        public bool isInHistory(Model.Item _item)
        {
            bool inHistory = false;
            foreach (Model.Item item in searchHistory)
            {
                if (item.name.Equals(_item.name))
                {
                    inHistory = true;
                    break; //we're done here, exit the loop
                }
            }
            return inHistory;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(null, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

using DnDNavigator.Runtime.Model.Entry;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace DnDNavigator.Runtime.Model.Playlists
{
    [DataContract(Name = "Playlist")]
    public class Playlist : INotifyPropertyChanged
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ObservableCollection<BaseEntry> Items { get; set; }

        public Playlist(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                this.Name = name;
            }
            else
            {
                throw new ArgumentException("Invalid name passed into Playlist constructor.");
            }

            Items = new ObservableCollection<BaseEntry>();
        }

        public Playlist()
        {
            Items = new ObservableCollection<BaseEntry>();
        }

        /// <summary>
        /// Checks to see if the passed in entry is already in this playlist.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private bool IsInPlaylist(BaseEntry entry)
        {
            bool result = false;

            if (entry != null)
            {
                foreach (BaseEntry playlistEntry in Items)
                {
                    if (playlistEntry.Name.Equals(entry.Name))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        public override int GetHashCode()
        {
            int hash = 31 * Name.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            bool result = false;
            Playlist comp = obj as Playlist;
            if (comp != null)
            {
                if (this.Name.Equals(comp.Name) && this.GetHashCode() == comp.GetHashCode())
                {
                    result = true;
                }
            }

            return result;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}

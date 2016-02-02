
using DnDNavigator.Runtime.DataAccess;
using DnDNavigator.Runtime.Model.DnDEntry;
using System.ComponentModel;
using System.Runtime.Serialization;
namespace DnDNavigator.Runtime.Model.Entry
{
    [DataContract(Name = "BaseEntry")]
    public class BaseEntry : IEntry, INotifyPropertyChanged
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Descriptor { get; set; }

        [DataMember]
        public string Full_Text { get; set; }

        public bool IsFavorite
        {
            get
            {
                return new FavoriteService().IsInFavorites(this);
            }
        }

        public BaseEntry()
        { }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj != null)
            {
                BaseEntry compEntry = obj as BaseEntry;

                if (compEntry != null)
                {
                    result = this.Name.Equals(compEntry.Name);
                }
            }

            return result;
        }

        public override int GetHashCode()
        {
            return new {Name}.GetHashCode();
        }
    }
}

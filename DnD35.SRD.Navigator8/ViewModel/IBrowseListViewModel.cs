
using DnD35.SRD.Navigator8.MVVMLight;
using DnDNavigator.Runtime.Model.DnDEntry;
using DnDNavigator.Runtime.Sort;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DnD35.SRD.Navigator8.ViewModel
{
    public interface IBrowseListViewModel<T> : INotifyPropertyChanged
    {
        ObservableCollection<Group<T>> Entries { get; }
        EntryType.Types EntryType { get; }

        RelayCommand GetEntriesCommand { get; }

        /// <summary>
        /// This is typed to receive a string since I can't specify an enum via
        /// generics.  The implementation needs to parse the string into an enum of the
        /// proper type.  I have no idea why I can't contain a generic to an enum
        /// right now.
        /// </summary>
        RelayCommand<string> ReorderEntriesCommand { get; }

        Task GetEntries();
    }
}

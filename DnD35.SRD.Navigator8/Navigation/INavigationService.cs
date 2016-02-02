using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD35.SRD.Navigator8.Navigation
{
    public interface INavigationService
    {
        void GoBack();
        void NavigateTo(Uri uri);
    }
}

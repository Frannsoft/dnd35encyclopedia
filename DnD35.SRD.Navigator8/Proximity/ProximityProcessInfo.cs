using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DnD_3._5_SRD_Navigator8.Proximity
{
    public class ProximityProcessInfo : INotifyPropertyChanged
    {
        private string _info;
        public event PropertyChangedEventHandler PropertyChanged;

        public string InformationPanel
        {
            get { return _info; }
            set
            {
                _info = value;
                OnPropertyChanged("InformationPanel");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #region textbox Information methods
        public void updateInformation(string _value)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    InformationPanel += _value;
                });
        }

        public void clearInformation()
        {
            InformationPanel = string.Empty;
            OnPropertyChanged("InformationPanel");
        }
        #endregion
    }
}

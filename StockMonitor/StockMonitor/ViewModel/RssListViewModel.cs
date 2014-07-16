using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using StockMonitor.Loader;
using StockMonitor.Model;

namespace StockMonitor.ViewModel
{
    class RssListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<RssInfo> rssList = new ObservableCollection<RssInfo>();
        public ObservableCollection<RssInfo> RssList
        {
            get
            {
                return rssList;
            }
            set
            {
                rssList = value;
                OnPropertyChanged("RssList");
            }
        }

        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

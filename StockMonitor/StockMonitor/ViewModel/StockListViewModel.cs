using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using StockMonitor.Model;

namespace StockMonitor.ViewModel
{
    class StockListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<StockInfo> stockList = new ObservableCollection<StockInfo>();
        public ObservableCollection<StockInfo> StockList
        {
            get
            {
                return stockList;
            }
            set
            {
                stockList = value;
                OnPropertyChanged("StockList");
            }
        }

        public void AddStock(List<StockInfo> stocks)
        {
            if (stocks != null && stocks.Count > 0)
                StockList.Add(stocks[0]);
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

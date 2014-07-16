using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace StockMonitor.Model
{
    public class StockInfo : INotifyPropertyChanged
    {
        public string Symbol { get; set; }

        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        private double open;
        public double Open
        {
            get { return open; }
            set
            {
                open = value;
                OnPropertyChanged("Open");
            }
        }

        private double close;
        public double Close
        {
            get { return close; }
            set
            {
                close = value;
                OnPropertyChanged("Close");
            }
        }

        private double high;
        public double High
        {
            get { return high; }
            set
            {
                high = value;
                OnPropertyChanged("High");
            }
        }

        private double low;
        public double Low
        {
            get { return low; }
            set
            {
                low = value;
                OnPropertyChanged("Low");
            }
        }

        private double volume;
        public double Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                OnPropertyChanged("Volume");
            }
        }

        private double adjClose;
        public double AdjClose
        {
            get { return adjClose; }
            set
            {
                adjClose = value;
                OnPropertyChanged("AdjClose");
            }
        }

        private double ma;
        public double MA 
        { 
            get { return ma; }
            set
            {
                ma = value;
                OnPropertyChanged("MA");
            }
        }

        private double volatility;
        public double Volatility
        {
            get { return volatility; }
            set
            {
                volatility = value;
                OnPropertyChanged("Volatility");
            }
        }

        public static StockInfo Create(string symbol, string csvContent)
        {
            var cols = csvContent.Split(',');
            StockInfo ret = null;
            try
            {
                ret = new StockInfo
                {
                    Symbol = symbol,
                    Date = Convert.ToDateTime(cols[0]),
                    Open = Convert.ToDouble(cols[1]),
                    Close = Convert.ToDouble(cols[2]),
                    Low = Convert.ToDouble(cols[3]),
                    High = Convert.ToDouble(cols[4]),
                    Volume = Convert.ToDouble(cols[5]),
                    AdjClose = Convert.ToDouble(cols[6])
                };
            } 
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            return ret;
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

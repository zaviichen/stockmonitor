using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Windows.Controls.Ribbon;
using StockMonitor.dataloader;
using System.Threading;
using System.Globalization;
using StockMonitor.Indicator;
using System.Collections.ObjectModel;
using StockMonitor.ViewModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using StockMonitor.Loader;
using StockMonitor.View;
using StockMonitor.Model;
using Syncfusion.Windows.Tools.Controls;

namespace StockMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Microsoft.Windows.Controls.Ribbon.RibbonWindow
    {
        private Dictionary<string, ObservableCollection<StockInfo>> rawDataDict = 
            new Dictionary<string, ObservableCollection<StockInfo>>();

        private Dictionary<string, TabItemExt> stockChartTabDict = 
            new Dictionary<string, TabItemExt>();

        private Dictionary<string, List<RssInfo>> stockRssDict = 
            new Dictionary<string, List<RssInfo>>();

        private StockListViewModel stockListVM = new StockListViewModel();
        private RssListViewModel rssListVM = new RssListViewModel();

        public ObservableCollection<StockInfo> CurrentStocks
        {
            get
            {
                string symbol = CurrentStockSymbol;
                if (symbol != null)
                {
                    if (rawDataDict.ContainsKey(symbol))
                        return rawDataDict[symbol];
                }
                return null;
            }
        }

        public string CurrentStockSymbol
        {
            get
            {
                int curId = this.stockOverriewView.SelectedIndex;
                if (curId >= 0)
                {
                    var item = this.stockOverriewView.Items[curId] as StockInfo;
                    if (item != null)
                    {
                        return item.Symbol;
                    }
                }
                return null;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            // Insert code required on object creation below this point.
            this.stockOverriewView.ItemsSource = stockListVM.StockList;
            this.rssListView.ItemsSource = rssListVM.RssList;
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string dir = @"data/";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var csvLoader = new CsvLoader();
            string[] files = Directory.GetFiles(dir, "*.csv");
            foreach (var file in files)
            {
                string csvName = file.Split('/').Last();
                string symbol = csvName.Substring(0, csvName.Length - 4);

                var data = csvLoader.LoadData(symbol);
                rawDataDict.Add(symbol, new ObservableCollection<StockInfo>(data));
                stockListVM.AddStock(data);
            }
        }

        private void YahooBtn_Click(object sender, RoutedEventArgs e)
        {
            var yahoo = new YahooLoader();
            string symbol = this.SymbolText.Text.ToUpper().Trim();
            if (string.IsNullOrEmpty(symbol))
            {
                MessageBox.Show("Please enter the stock symbol.");
                return;
            }

            var data = yahoo.LoadData(symbol);
            if (rawDataDict.ContainsKey(symbol))
            {
                rawDataDict[symbol] = new ObservableCollection<StockInfo>(data);
            }
            else
            {
                rawDataDict.Add(symbol, new ObservableCollection<StockInfo>(data));
                stockListVM.AddStock(data);
            }
            
            MessageBox.Show("Load stock information success: " + symbol + 
                ", records: " + data.Count.ToString());
        }

        private void OpenCsvBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.RestoreDirectory = true;
            dialog.ShowDialog();

            if (!string.IsNullOrEmpty(dialog.FileName))
            {
                // TODO
            }

            MessageBox.Show("TODO");
        }

        private void stockOverriewView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CurrentStockSymbol == null)
                return;

            if (stockChartTabDict.ContainsKey(CurrentStockSymbol))
            {
                // make the target tab selected
                for (int i = 0; i < this.chartTabs.Items.Count; i++)
                {
                    var tab = this.chartTabs.Items[i] as TabItemExt;
                    if (tab != null && tab.Header.Equals(CurrentStockSymbol))
                    {
                        this.chartTabs.SelectedIndex = i;
                        break;
                    }
                }
                return;
            }
        }

        private void ShowChart(IList<StockInfo> stocks)
        {
            string symbol = stocks[0].Symbol;
            if (stockChartTabDict.ContainsKey(symbol))
            {
                // make the target tab selected
                for(int i = 0; i < this.chartTabs.Items.Count; i++)
                {
                    var tab = this.chartTabs.Items[i] as TabItemExt;
                    if (tab != null && tab.Header.Equals(symbol)) 
                    {
                        this.chartTabs.SelectedIndex = i;
                        break;
                    }
                }
                return;
            }

            var chartView = new StockChartView(stocks);
            chartView.Rebind();

            var chartTab = new TabItemExt();
            chartTab.Header = symbol;
            chartTab.Content = chartView;

            this.chartTabs.Items.Add(chartTab);
            stockChartTabDict.Add(symbol, chartTab);
        }

        private void InitIndicator(IList<StockInfo> stocks)
        {
            var indicators = new List<BaseIndicator>
            {
                new MovingAvarageIndicator(),
                new VolatilityIndicator()
            };

            foreach (var indicator in indicators)
            {
                indicator.Calculate(stocks);
            }
        }

        private void FillRssInfo(string symbol)
        {
            List<RssInfo> rssList = null;
            if (stockRssDict.ContainsKey(symbol))
            {
                rssList = stockRssDict[symbol];
            }
            else
            {
                var rssLoader = new RssLoader();
                rssList = rssLoader.LoadRSS(symbol);
                stockRssDict.Add(symbol, rssList);
            }
            rssListVM.RssList = new ObservableCollection<RssInfo>(rssList);
            this.rssListView.ItemsSource = rssListVM.RssList;
        }

        private void rssListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int curId = this.rssListView.SelectedIndex;
            if (curId == -1)
                return;

            var rss = this.rssListView.Items[curId] as RssInfo;
            if (rss != null)
            {
                this.rssContentText.Text = rss.Description + "\n\n" + rss.Link;
            }
        }

        private void ShowChartBtn_Click(object sender, RoutedEventArgs e)
        {
            InitIndicator(CurrentStocks);
            ShowChart(CurrentStocks);
            FillRssInfo(CurrentStockSymbol);
        }

        private void ShowGridBtn_Click(object sender, RoutedEventArgs e)
        {
            InitIndicator(CurrentStocks);
            var gridView = new StockGridView(CurrentStocks);
            FillRssInfo(CurrentStockSymbol);
            gridView.Show();
        }

        private void indMABtn_Click(object sender, RoutedEventArgs e)
        {
            var view = new IndicatorMAView(CurrentStocks);
            view.Show();
        }

        private void indVolBtn_Click(object sender, RoutedEventArgs e)
        {
            var view = new IndicatorVolView(CurrentStocks);
            view.Show();
        }
    }
}

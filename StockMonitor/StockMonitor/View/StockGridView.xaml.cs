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
using StockMonitor.Model;

namespace StockMonitor.View
{
    /// <summary>
    /// Interaction logic for StockGridView.xaml
    /// </summary>
    public partial class StockGridView : Window
    {
        public StockGridView(IList<StockInfo> stocks)
        {
            InitializeComponent();

            this.data = stocks;
            this.StockGrid.ItemsSource = data;

            if (data != null && data.Count > 0)
            {
                this.Title = data[0].Symbol;
                this.EndTimeDatePicker.SelectedDate = data[0].Date;
                this.StartTimeDatePicker.SelectedDate = data[data.Count - 1].Date;
                this.RecordText.Text = data.Count.ToString();
            }
        }

        private IList<StockInfo> data;

        private void FilterBtn_Click(object sender, RoutedEventArgs e)
        {
            DateTime? start = this.StartTimeDatePicker.SelectedDate;
            DateTime? end = this.EndTimeDatePicker.SelectedDate;

            if (start == null || end == null)
            {
                MessageBox.Show("Please select the date.");
                return;
            }

            var source = (from p in data
                          where p.Date >= start && p.Date <= end
                          select p).ToList();

            StockGrid.ItemsSource = source;
            RecordText.Text = source.Count.ToString();
        }

        private void RevertBtn_Click(object sender, RoutedEventArgs e)
        {
            StockGrid.ItemsSource = data;
            RecordText.Text = data.Count.ToString();
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            var filtered = StockGrid.ItemsSource as List<StockInfo>;
            if (filtered != null)
            {
                MessageBox.Show("TODO");
            }
        }
    }
}

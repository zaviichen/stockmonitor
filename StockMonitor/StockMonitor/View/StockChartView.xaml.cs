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
using StockMonitor.Indicator;
using StockMonitor.Model;

namespace StockMonitor.View
{
    /// <summary>
    /// Interaction logic for StockChartView.xaml
    /// </summary>
    public partial class StockChartView : UserControl
    {
        public StockChartView(IList<StockInfo> stocks)
        {
            InitializeComponent();

            this.data = stocks;

            this.candleGraph.DataSet = this.stockSet1;
            this.indMAGraph.DataSet = this.maSet;
            this.volGraph.DataSet = this.volSet;

            // fill the basic stock information
            this.stockSet1.ItemsSource = this.data;
        }

        private IList<StockInfo> data;

        public void ApplyIndicator(List<BaseIndicator> indicators)
        {
            foreach (var indicator in indicators)
            {
                indicator.Calculate(this.data);
            }
            Rebind();
        }

        public void Rebind()
        {
            this.stockSet1.ItemsSource = data;
            this.maSet.ItemsSource = data;
            this.volSet.ItemsSource = data;
            this.candleGraph.DataSet = this.stockSet1;
            this.indMAGraph.DataSet = this.maSet;
            this.volGraph.DataSet = this.volSet;
        }
    }
}

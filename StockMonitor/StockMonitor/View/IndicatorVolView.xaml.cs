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
using System.Windows.Shapes;
using StockMonitor.Indicator;
using StockMonitor.Model;

namespace StockMonitor.View
{
    /// <summary>
    /// Interaction logic for IndicatorVolView.xaml
    /// </summary>
    public partial class IndicatorVolView : Window
    {
        public IndicatorVolView(IList<StockInfo> stocks)
        {
            InitializeComponent();

            this.data = stocks;
        }

        private IList<StockInfo> data;

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            string periodStr = this.PeriodText.Text.Trim();
            int period = 30;

            if (!int.TryParse(periodStr, out period))
            {
                MessageBox.Show("Period should be integer: " + periodStr);
                return;
            }

            var indicator = new VolatilityIndicator
            {
                PriceType = (BaseIndicator.ApplyTo)(this.ApplyToCombo.SelectedIndex),
                Period = period
            };

            indicator.Calculate(data);
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

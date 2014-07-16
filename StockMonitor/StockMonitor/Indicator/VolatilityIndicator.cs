using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockMonitor.Model;

namespace StockMonitor.Indicator
{
    public class VolatilityIndicator : BaseIndicator
    {
        private int period = 30;
        public int Period
        {
            get { return period; }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Period must greater than 0.");
                }
                period = value;
            }
        }

        private List<double> results = new List<double>();
        public List<double> Results
        {
            get { return results; }
        }

        public override void Calculate(IList<StockInfo> stocks)
        {
            if (stocks == null || stocks.Count == 0)
                return;

            for (int i = 0; i < stocks.Count; i++ )
            {
                var stock = stocks[i];
                double curPrice = GetAppliedPrice(stock);

                int preId = GetPeriodAgoIndex(i, period, stocks.Count);
                double prePrice = GetAppliedPrice(stocks[preId]);

                double vol = (prePrice > 0) ? 
                    Math.Abs(curPrice - prePrice) / prePrice : 0;
                stock.Volatility = vol * 100;
            }
        }
    }
}

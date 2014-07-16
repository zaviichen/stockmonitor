using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockMonitor.Model;

namespace StockMonitor.Indicator
{
    public class MovingAvarageIndicator : BaseIndicator
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

        public void Calculate2(List<StockInfo> stocks)
        {
            double sum = 0;
            results.Clear();

            for (int i = 0; i < stocks.Count; i++)
            {
                if (i < period)
                {
                    sum += GetAppliedPrice(stocks[i]);
                    results.Add(0);
                }
                else if (i == period)
                {
                    results.Add(sum / period);
                }
                else
                {
                    sum -= GetAppliedPrice(stocks[i - period - 1]);
                    sum += GetAppliedPrice(stocks[i]);
                    results.Add(sum / period);
                }
            }
        }

        public override void Calculate(IList<StockInfo> stocks)
        {
            if (stocks == null || stocks.Count == 0)
                return;

            double sum = 0;
            for (int i = stocks.Count-1, j = 1; i >= 0; i--, j++)
            {
                var stock = stocks[i];
                if (j <= period)
                {
                    sum += GetAppliedPrice(stock);
                    stock.MA = sum / j;
                }
                else
                {
                    sum -= GetAppliedPrice(stocks[i + period]);
                    sum += GetAppliedPrice(stock);
                    stock.MA = sum / period;
                }
            }
        }
    }
}

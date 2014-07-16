using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockMonitor.Model;

namespace StockMonitor.Indicator
{
    public abstract class BaseIndicator
    {
        public enum ApplyTo
        {
            OPEN = 0,
            CLOSE = 1,
            HIGH = 2,
            LOW = 3,
            ADJ_CLOSE = 4,
            MEDIAN = 5
        }

        public ApplyTo PriceType { get; set; }

        public double GetAppliedPrice(StockInfo stock)
        {
            switch (PriceType)
            {
                case ApplyTo.OPEN:
                    return stock.Open;
                case ApplyTo.CLOSE:
                    return stock.Close;
                case ApplyTo.HIGH:
                    return stock.High;
                case ApplyTo.LOW:
                    return stock.Low;
                case ApplyTo.ADJ_CLOSE:
                    return stock.AdjClose;
                case ApplyTo.MEDIAN:
                    return (stock.High + stock.Low) / 2;
                default:
                    return stock.Close;
            }
        }

        public int GetPeriodAgoIndex(int current, int period, int size)
        {
            // if the total size is less than period
            // always return the oldest one index
            if (size <= period)
            {
                return size - 1;
            }

            return (current + period >= size) ? 
                size - 1 : current + period - 1;
        }

        public virtual void Calculate(IList<StockInfo> stocks) { }
    }
}

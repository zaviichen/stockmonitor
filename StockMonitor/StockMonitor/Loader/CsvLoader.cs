using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using StockMonitor.Model;

namespace StockMonitor.dataloader
{
    class CsvLoader
    {
        public List<StockInfo> LoadData(string symbol)
        {
            string path = string.Format("data/{0}.csv", symbol);
            var stocks = new List<StockInfo>();

            using (var reader = new StreamReader(path))
            {
                string data = reader.ReadToEnd();
                string[] rows = data.Trim().Split('\n');

                for (int i = 1; i < rows.Length; i++)
                {
                    var stock = StockInfo.Create(symbol, rows[i]);
                    if (stock != null)
                    {
                        stocks.Add(stock);
                    }
                }
            }
            return stocks;
        }
    }
}

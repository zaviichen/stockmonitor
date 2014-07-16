using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows;
using StockMonitor.Model;

namespace StockMonitor.dataloader
{
    class YahooLoader
    {
        public static readonly string URI = "http://ichart.finance.yahoo.com/table.csv?s={0}&c={1}";
        public static readonly int startOfYear = 1962;

        public List<StockInfo> LoadData(string symbol)
        {
            string uri = string.Format(URI, symbol, startOfYear);
            var stocks = new List<StockInfo>();

            try
            {
                using (var web = new WebClient())
                {
                    string data = web.DownloadString(uri);
                    string[] rows = data.Trim().Split('\n');

                    for (int i = 1; i < rows.Length; i++)
                    {
                        var cols = rows[i].Split(',');
                        var stock = StockInfo.Create(symbol, rows[i]);
                        if (stock != null)
                        {
                            stocks.Add(stock);
                        }
                    }

                    // save to csv file
                    using (var writer = new StreamWriter(string.Format("data/{0}.csv", symbol), false))
                    {
                        writer.Write(data);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return stocks;
        }
    }
}

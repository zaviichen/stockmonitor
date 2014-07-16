using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using StockMonitor.Model;

namespace StockMonitor.Loader
{
    class RssLoader
    {
        private static readonly string URI = "http://feeds.finance.yahoo.com/rss/2.0/headline?s={0}&region=US&lang=en-US";

        public List<RssInfo> LoadRSS(string symbol)
        {
            var rss = new List<RssInfo>();
            try
            {
                XElement root = XElement.Load(string.Format(URI, symbol));
                var items = from e in root.Descendants("item")
                            select new RssInfo
                            {
                                Title = e.Element("title").Value,
                                Description = e.Element("description").Value,
                                Link = e.Element("link").Value
                            };
                rss = items.ToList();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return rss;
        }
    }
}

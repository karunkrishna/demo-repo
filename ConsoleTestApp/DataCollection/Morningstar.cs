using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ConsoleTestApp.DataCollection
{
    
    class Morningstar
    {
        private string urlFinancials = "http://financials.morningstar.com/income-statement/is.html?t=DOW&region=usa&culture=en-US";
        private string urlKeyRatios = "http://financials.morningstar.com/ratios/r.html?t=DOW&region=usa&culture=en-US";

        public Morningstar()
        {
            this.FetchFinancials(urlFinancials);

        }
        public void FetchFinancials(string url)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlWeb hw = new HtmlWeb();
            doc = hw.Load(url);
            string content  = doc.ToString();
        }
    }
}

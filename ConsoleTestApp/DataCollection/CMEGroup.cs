using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace ConsoleTestApp.DataCollection
{
    class CMEGroup
    {

        private string urlFutureDates = "http://www.cmegroup.com/trading/equity-index/us-index/e-mini-sandp500_product_calendar_futures.html";
        private string urlOptionsDatesStandard = "http://www.cmegroup.com/trading/equity-index/us-index/e-mini-sandp500_product_calendar_options.html?optionProductId=138";
        private string urlOptionsDatesMontly = "http://www.cmegroup.com/trading/equity-index/us-index/e-mini-sandp500_product_calendar_options.html?optionProductId=136";
        private string urlOptionsDatesWeekly = "http://www.cmegroup.com/trading/equity-index/us-index/e-mini-sandp500_product_calendar_options.html?optionProductId=2915";
        List<string[]> matrix = new List<string[]>();
        private MySqlConnection dbConn = new MySqlConnection("server = localhost; database=algo;uid=root;password=Password1;");

        public CMEGroup()
        {
            this.FetchData(urlFutureDates);
            this.FetchData(urlOptionsDatesStandard);
            this.FetchData(urlOptionsDatesMontly);
            this.FetchData(urlOptionsDatesWeekly);
            this.SaveToDatabase(matrix);
            //Console.ReadKey();
        }

        private void FetchData(string url)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlWeb hw = new HtmlWeb();
            doc = hw.Load(url);



            HtmlNodeCollection record =
                //doc.DocumentNode.SelectNodes(".//*[@id='calendarFuturesProductTable1']//tbody//tr");
                doc.DocumentNode.SelectNodes(".//tbody//tr");
                
            foreach (var node in record)
            {
                string[] row = new string[3];
                MatchCollection data = Regex.Matches(node.InnerHtml, @"\n\t\t\t<td>\s*(.+?)</td>", RegexOptions.Singleline);

                row[0] = data[0].Groups[1].Value;
                string[] splitStartEndDate = Regex.Split(data[1].Groups[1].Value,"<br>");
                row[1] = DateTime.Parse(splitStartEndDate[0]).ToShortDateString();
                row[2] = DateTime.Parse(splitStartEndDate[1]).ToShortDateString();

                matrix.Add(row);
            }
        }

        private void SaveToDatabase(List<string[]> matrix)
        {


            foreach (var record in matrix)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbConn;
                dbConn.Open();
                cmd.CommandText = "Insert into settlementDate values (@symbol, @ticker,@startDate,@endDate)";
                cmd.Parameters.Add(new MySqlParameter("@symbol", "ES"));
                cmd.Parameters.Add(new MySqlParameter("@ticker", record[0]));
                cmd.Parameters.Add(new MySqlParameter("@startDate", record[1]));
                cmd.Parameters.Add(new MySqlParameter("@endDate", record[2]));
                cmd.ExecuteNonQuery();
                dbConn.Close();
            }



        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.IO;
using System.Net;
using MySql.Data.MySqlClient;
using Microsoft.Office.Interop.Excel;


namespace WindowsFormsApplication1
{

    class SecotorHoldingDownloader
    {
        private MySqlConnection dbConn = new MySqlConnection("server = localhost; database=algo;uid=root;password=Password1;");
        private string csvDownloadDir = @"C:\Users\Karunyan\Documents\Reports\_sectorData\";
        private string[] sectorSymbol = { "SPY","XLY", "XLP", "XLE", "XLFS", "XLF", "XLV", "XLI", "XLB", "XLRE", "XLK", "XLU" };

        public void DownloadYahooHistoricalData()
        {
            foreach(string symbol in sectorSymbol)
                this._downloadYahooHistoricalData(symbol);
        }

        public void RecordHoldingsData()
        {
            this._recordHoldingsData();
        }

        public void DownloadStateStreetHoldings()
        {
            foreach (string symbol in sectorSymbol)
                this._downloadStateStreetHoldings(symbol);
        }

        private void _downloadYahooHistoricalData(string symbol) // Download the Sector Historical Data 
        {
            string url = string.Format("http://ichart.yahoo.com/table.csv?s={0}&a=0&b=1&c=2016&d=11&e=24&f=2016", symbol);
            WebClient data = new WebClient();

            try
            {
                data.DownloadFile(url, String.Format("{0}{1}.csv", csvDownloadDir, symbol));
            }
            catch { }

        }

        private void _downloadStateStreetHoldings(string symbol)  // Download the Fund Holdings per Sector
        {
            string url = "https://www.spdrs.com/product/fund.seam?ticker=" + symbol;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlWeb hw = new HtmlWeb();
            doc = hw.Load(url);

            HtmlNodeCollection holdingURL = doc.DocumentNode.SelectNodes("//*[@id='FUND_TOP_HOLDINGS']//@href");
            string symbolURL = url.Substring(0, 21) + holdingURL[0].Attributes[0].Value;
            
            WebClient data = new WebClient();
            data.DownloadFile(symbolURL, string.Format("{0}{1}_Holdings.xls", csvDownloadDir, symbol));

        }

        private void _recordHoldingsData()  // Will only record the top 10 weighted stocks in the sector list
        {
            Dictionary<string, string[][]> top10SectorInstruments = new Dictionary<string, string[][]>();

            string[] fileNames = Directory.GetFiles(csvDownloadDir, "*_Holdings.xls");
            foreach (string file in fileNames)
            {
                //string path = Path.Combine(csvDownloadDir, file);

                Application excel = new Application();
                Workbook wb = excel.Workbooks.Open(file);
                Worksheet excelSheet = wb.ActiveSheet;

                string[][] matrix = new string[10][];

                for(int i=0;i<10;i++)
                {
                    //The matrix column header is set to: 
                    matrix[i] = new string[] { excelSheet.Cells[2, 2].Value.ToString(), excelSheet.Cells[5+i, 2].Value.ToString(), excelSheet.Cells[5 + i, 3].Value.ToString(),
                        excelSheet.Cells[5 + i, 5].Value.ToString(), excelSheet.Cells[3, 2].Value.ToString().Substring(6)};
                }

                top10SectorInstruments.Add(excelSheet.Cells[2, 2].Value.ToString(), matrix);
                wb.Close();

            }
            //Save the data to the databse using the dictionary currently in memory
            this._saveDataToDatabase(top10SectorInstruments);
        }

        private void _saveDataToDatabase( Dictionary<string,string[][]> dictData)
        {
            dbConn.Open();

            // Conduct a sanity check to insert the data based on the secotrSymbol and the lastUpdateTime 
            foreach (KeyValuePair<string, string[][]> key in dictData)
            {


                foreach(string[] element in key.Value)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = dbConn;
                    cmd.CommandText = "Insert into sectorData values (@sectorSymbol, @underlyingSymbol,@weight,@sharesHeld,@lastUpdatedTime)";
                    cmd.Parameters.Add(new MySqlParameter("@sectorSymbol", element[0].ToString()));
                    cmd.Parameters.Add(new MySqlParameter("@underlyingSymbol", element[1].ToString()));
                    cmd.Parameters.Add(new MySqlParameter("@weight", element[2].ToString()));
                    cmd.Parameters.Add(new MySqlParameter("@sharesHeld", element[3].ToString()));
                    cmd.Parameters.Add(new MySqlParameter("@lastUpdatedTime", element[4].ToString()));

                    cmd.ExecuteNonQuery();
                    
                }
                
            }
            dbConn.Close();

        }
        private void _recordETFData()
        {

        }
    }
}

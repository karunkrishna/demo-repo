using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using HtmlAgilityPack;
using System.Net;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace ConsoleTestApp.DataCollection
{
    class StateStreet
    {
        private MySqlConnection dbConn = new MySqlConnection("server = localhost; database=algo;uid=root;password=Password1;");
        private string csvDownloadDir = @"C:\Users\Karunyan\Documents\Reports\_sectorData\";
        //private string csvProcesseDir = @"C:\Users\Karunyan\Documents\Reports\_archieved\"; Work on moving archieved file

        private string[] sectorSymbol = { "SPY", "XLY", "XLP", "XLE", "XLFS", "XLF", "XLV", "XLI", "XLB", "XLRE", "XLK", "XLU" };
        //private string[] sectorSymbol = {"XLB" };   //Test Case
        private Dictionary<string, string[][]> dictData = new Dictionary<string, string[][]>();

        public void DownloadXLSFile()
        { 
            foreach(string symbol in sectorSymbol)
            { 
                string url = "https://www.spdrs.com/product/fund.seam?ticker=" + symbol;
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                HtmlWeb hw = new HtmlWeb();
                doc = hw.Load(url);

                    HtmlNodeCollection holdingURL = doc.DocumentNode.SelectNodes("//*[@id='FUND_TOP_HOLDINGS']//@href");
                string symbolURL = url.Substring(0, 21) + holdingURL[0].Attributes[0].Value;

                WebClient data = new WebClient();
                data.DownloadFile(symbolURL, string.Format("{0}{1}_Holdings_{2}.xls", csvDownloadDir, symbol,DateTime.Now.ToShortDateString()));
            }
            Console.WriteLine("State Street.cs - Fetched data for StateStreet website...");
            this.CleanXLSFile();
        }

        //private void CleanXLSFile()
        public void CleanXLSFile()
        {
            Dictionary<string, string[][]> underlyingList = new Dictionary<string, string[][]>();

            string[] fileNames = Directory.GetFiles(csvDownloadDir, "*_Holdings*");
            foreach (string file in fileNames)
            {

                Application excel = new Application();
                Workbook wb = excel.Workbooks.Open(file);
                Worksheet excelSheet = wb.ActiveSheet;

                string[][] data = new string[excelSheet.UsedRange.Row + excelSheet.UsedRange.Rows.Count - 15][]; // -15 represents the (4) header and (11) footer to be removed from document

                for (int i = 0; i < excelSheet.UsedRange.Row + excelSheet.UsedRange.Rows.Count - 15; i++)
                {
                    //The matrix column header is set to: 
                    data[i] = new string[] { excelSheet.Cells[2, 2].Value.ToString(), excelSheet.Cells[5+i, 2].Value.ToString(), excelSheet.Cells[5 + i, 3].Value.ToString(),
                        excelSheet.Cells[5 + i, 5].Value.ToString(), excelSheet.Cells[5 + i, 4].Value.ToString(),excelSheet.Cells[3, 2].Value.ToString().Substring(6)};

                }
                dictData.Add(excelSheet.Cells[2, 2].Value.ToString(),data);
                wb.Close();
                
            }
            Console.WriteLine("State Street.cs - Parsed all excel documents...");
            this.SaveDataToDatabase(this.GetRecordedDateList());
        }

        private List<string> GetRecordedDateList()
        {
            dbConn.Open();
            string query = "select concat(sectorSymbol,(concat('.', lastUpdatedTime))) from etfData group by concat(sectorSymbol, (concat('.', lastUpdatedTime)))";
            List<string> data = new List<string>();

            using (MySqlCommand command = new MySqlCommand(query, dbConn))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        data.Add(reader.GetString(0));
                    }
                }
            }
            dbConn.Close();
            Console.WriteLine("State Street.cs - Retreieved recorded date records for sanity check...");
            return data;
        }

        public void SaveDataToDatabase(List<string> checkDate)
        {
            dbConn.Open();

            // Conduct a sanity check to insert the data based on the secotrSymbol and the lastUpdateTime 
            foreach (KeyValuePair<string, string[][]> key in dictData)
            {
                    foreach (string[] element in key.Value)
                    {
                        if (!checkDate.Contains(element[0].ToString() + "." + element[5].ToString()))
                        {
                            MySqlCommand cmd = new MySqlCommand();
                            cmd.Connection = dbConn;
                            cmd.CommandText = "Insert into etfData values (@sectorSymbol, @underlyingSymbol,@weight,@sharesHeld,@sector,@lastUpdatedTime)";
                            cmd.Parameters.Add(new MySqlParameter("@sectorSymbol", element[0].ToString()));
                            cmd.Parameters.Add(new MySqlParameter("@underlyingSymbol", element[1].ToString()));
                            cmd.Parameters.Add(new MySqlParameter("@weight", element[2].ToString()));
                            cmd.Parameters.Add(new MySqlParameter("@sharesHeld", element[3].ToString()));
                            cmd.Parameters.Add(new MySqlParameter("@sector", element[4].ToString()));
                            cmd.Parameters.Add(new MySqlParameter("@lastUpdatedTime", element[5].ToString()));
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            dbConn.Close();
            Console.WriteLine("State Street.cs - Saved Data to the database...");
        }
    }
}

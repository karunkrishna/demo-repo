using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using MySql.Data.MySqlClient;
using HtmlAgilityPack;
using System.IO;
using System.Diagnostics;



namespace ConsoleTestApp.DataCollection
{
    class YahooFinance
    {
        private Dictionary<string, bool?> isValidURL = new Dictionary<string, bool?>();
        private MySqlConnection dbConn = new MySqlConnection("server = localhost; database=algo;uid=root;password=Password1;"); 
        private string csvDownloadDir = @"C:\Users\Karunyan\Documents\Reports\_sectorData\";

        public void RunURLChecker()
        {
            Stopwatch timer = Stopwatch.StartNew();
            this.GetUniqueTickerSymbol();
            Console.WriteLine("YahooFinance.cs - Checking if URL is valid...");
            foreach (KeyValuePair<string, bool?> element in isValidURL.ToArray())
            {
                this.ValidateTickerURL(element.Key);
            }
            Console.WriteLine("YahooFinance.cs - Downloading yahoo daily data on valid ticker symbols");
            this.DownloadDailyData();
            Console.WriteLine("YahooFinance.cs - Save data to database...");
            this.SaveDatatoDatabase();

            timer.Stop();
            TimeSpan timespan = timer.Elapsed;
            Console.WriteLine(timespan);
            Console.ReadKey();
        }
        private void ValidateTickerURL(string symbol)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlWeb hw = new HtmlWeb();
            doc = hw.Load(string.Format("https://ca.finance.yahoo.com/q?s={0}", symbol));

            HtmlNodeCollection isURLErrorNode = doc.DocumentNode.SelectNodes(".//div[contains(@class,'error')]");

            if (isURLErrorNode == null)
                isValidURL[symbol] = true;
            else
                isValidURL[symbol] = false;
        }
        private void DownloadDailyData()
        {
            foreach (KeyValuePair<string, bool?> element in isValidURL)
            {
                if (element.Value == true)
                {
                    string url = string.Format("http://ichart.yahoo.com/table.csv?s={0}&a=0&b=1&c=2010&d=11&e=31&f=2016", element.Key);
                    WebClient data = new WebClient();

                    try
                    {
                        data.DownloadFile(url, String.Format("{0}{1}_Daily_{2}.csv", csvDownloadDir, element.Key, DateTime.Now.ToShortDateString()));
                    }
                    catch { Console.WriteLine("Did not download the following symbol" + element.Key); }
                }
            }

        }   // TO DO: I need to only download data that is already not saved in the database. 
        private void GetUniqueTickerSymbol()
        {
            dbConn.Open();
            string query = "select distinct(underlyingsymbol) from etfdata union select distinct(sectorsymbol) from etfdata";
            //string query = "select distinct(underlyingsymbol)from etfdata where underlyingsymbol in ('DD', 'DOW', 'MON', '85749P9A')";
            using (MySqlCommand command = new MySqlCommand(query, dbConn))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        isValidURL.Add(reader.GetString(0), null);
                    }
                }
            }
            dbConn.Close();
            Console.WriteLine("YahooFinance.cs - Retreieved unique sybmols...");
        }
        private void SaveDatatoDatabase()
        {

            foreach (KeyValuePair<string, bool?> element in isValidURL)
            {
                if (element.Value == true)
                { 
                dbConn.Open();
                MySqlCommand createTable = new MySqlCommand("CREATE TABLE IF NOT EXISTS yahooDailyData (symbol varchar(20), datenum varchar(30), open varchar(20), high varchar(20), low varchar(20), close varchar(20), volume varchar(25), adjClose varchar(20));",dbConn);
                createTable.ExecuteNonQuery();

                string[] fileNames = Directory.GetFiles(csvDownloadDir, element.Key + "_Daily*");
                {

                        var sr = new StreamReader(fileNames[0]);

                        while (!sr.EndOfStream)
                        {
                            var line = sr.ReadLine();
                            var column = line.Split(',');

                                if (!column[0].Contains("Date"))
                                {
                                    MySqlCommand cmd = new MySqlCommand();
                                    cmd.Connection = dbConn;
                                    cmd.CommandText = "Insert into yahooDailyData values (@symbol, @datenum, @open,@high,@low,@close,@volume,@adjClose)";
                                    cmd.Parameters.Add(new MySqlParameter("@symbol", element.Key));
                                    cmd.Parameters.Add(new MySqlParameter("@datenum", column[0].ToString()));
                                    cmd.Parameters.Add(new MySqlParameter("@open", column[1].ToString()));
                                    cmd.Parameters.Add(new MySqlParameter("@high", column[2].ToString()));
                                    cmd.Parameters.Add(new MySqlParameter("@low", column[3].ToString()));
                                    cmd.Parameters.Add(new MySqlParameter("@close", column[4].ToString()));
                                    cmd.Parameters.Add(new MySqlParameter("@volume", column[5].ToString()));
                                    cmd.Parameters.Add(new MySqlParameter("@adjClose", column[6].ToString()));
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        
                    }
                }
                dbConn.Close();

            }
        }   // TO DO: I need to include sanity checks so that I don't save duplicat data for symbol and date.
        public void SaveSingleData(string filename)
        {
                    dbConn.Open();
                    MySqlCommand createTable = new MySqlCommand("CREATE TABLE IF NOT EXISTS yahooDailyData (symbol varchar(20), datenum varchar(30), open varchar(20), high varchar(20), low varchar(20), close varchar(20), volume varchar(25), adjClose varchar(20));", dbConn);
                    createTable.ExecuteNonQuery();

                    string[] fileNames = Directory.GetFiles(csvDownloadDir, filename+".csv");
                    {

                        var sr = new StreamReader(fileNames[0]);

                        while (!sr.EndOfStream)
                        {
                            var line = sr.ReadLine();
                            var column = line.Split(',');

                            if (!column[0].Contains("Date"))
                            {
                                MySqlCommand cmd = new MySqlCommand();
                                cmd.Connection = dbConn;
                                cmd.CommandText = "Insert into yahooDailyData values (@symbol, @datenum, @open,@high,@low,@close,@volume,@adjClose)";
                                cmd.Parameters.Add(new MySqlParameter("@symbol", filename));
                                cmd.Parameters.Add(new MySqlParameter("@datenum", column[0].ToString()));
                                cmd.Parameters.Add(new MySqlParameter("@open", column[1].ToString()));
                                cmd.Parameters.Add(new MySqlParameter("@high", column[2].ToString()));
                                cmd.Parameters.Add(new MySqlParameter("@low", column[3].ToString()));
                                cmd.Parameters.Add(new MySqlParameter("@close", column[4].ToString()));
                                cmd.Parameters.Add(new MySqlParameter("@volume", column[5].ToString()));
                                cmd.Parameters.Add(new MySqlParameter("@adjClose", column[6].ToString()));
                                cmd.ExecuteNonQuery();
                            }
                        }

                    }

                dbConn.Close();

            
        }   // Alot of this is duplicate code. There is probably a cleaner way to do this. 
    }
}

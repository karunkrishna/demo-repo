using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using MySql.Data.MySqlClient;


namespace ConsoleTestApp.DataCollection
{
    class CheckURLIsValid
    {
        private Dictionary<string, string> isValidURL = new Dictionary<string, string>();
        private MySqlConnection dbConn = new MySqlConnection("server = localhost; database=algo;uid=root;password=Password1;");
        public void RunURLChecker()
        {
            this.UniqueTickerSymbol();
            this.TryURL();
        }
        private void TryURL()
        {

            // using MyClient from linked post
            using (var client = new MyClient())
            {
                try
                {
                    foreach (KeyValuePair<string, string> symbol in isValidURL)
                    { 
                        string yahooFinaceTickerSearchString = string.Format("https://ca.finance.yahoo.com/q?s={0}", symbol);
                        client.HeadOnly = true;
                        client.AllowAutoRedirect = false;
                        // fine, no content downloaded
                        string s1 = client.DownloadString(yahooFinaceTickerSearchString);
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }

            }
        }

        private void UniqueTickerSymbol()
        {
            dbConn.Open();
            //string query = "select distinct(underlyingsymbol) from etfdata union select distinct(sectorsymbol) from etfdata";
            string query = "select distinct(underlyingsymbol)from etfdata where underlyingsymbol in ('DD', 'DOW', 'MON', '85749P9A')";
            using (MySqlCommand command = new MySqlCommand(query, dbConn))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        isValidURL.Add(reader.GetString(0),"");
                    }
                }
            }
            dbConn.Close();
            Console.WriteLine("CheckURLisValid.cs - Retreieved unique sybmols...");
        }
    }

    class MyClient : WebClient 
    {
        // This piece of code was taken from stackoverflow.com
        // http://stackoverflow.com/questions/924679/c-sharp-how-can-i-check-if-a-url-exists-is-valid
        // http://stackoverflow.com/questions/2538895/c-sharp-detect-page-redirect

        public bool HeadOnly { get; set; }
        public bool AllowAutoRedirect { get; set; }
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest req = base.GetWebRequest(address);
            if (HeadOnly && req.Method == "GET")
            {
                req.Method = "HEAD";
                
            }
            Console.WriteLine(address); // I added this code to check if it worked. 
            Console.WriteLine(req.ToString());
            return req;
        }
    }  //Copied code to check if URL is valid
}

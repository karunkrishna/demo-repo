using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Timers;



namespace ConsoleTestApp.DataCollection
{
    class LocalForexFactory
    {
        private MySqlConnection dbConn = new MySqlConnection("server = localhost; database=algo;uid=root;password=Password1;");
        private List<string> checkEventId = new List<string>();
        private DateTime checkLastUpdatedTime = new DateTime();
        Timer aTimer = new Timer();
        public void FetchLocalHTTPData()
        {


            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 5000;
            aTimer.Enabled = true;

           // this.ParseLocalHttpData(@"C:/Users/Karunyan/Documents/Reports/testLocalHTML/doing.html", 60816);
            //            this.ParseLocalHttpData(@"C:/Users/Karunyan/Documents/Reports/testLocalHTML/after.html", 64079);

        }
        private void ParseLocalHttpData(string url, int eventId)
        {

            //TRY CATCH FOR WHEN FILE OR URL DOES NOT EXITS. 
            string refreshedActual = "";
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlWeb hw = new HtmlWeb();
            doc = hw.Load(url);


            HtmlNodeCollection actualNode = doc.DocumentNode.SelectNodes(string.Format("//*[@id='flexBox_flex_calendar_mainCal']//*[contains(@data-eventid,{0})]//td[contains(@class,'actual')]",eventId));
            HtmlNodeCollection previousNode = doc.DocumentNode.SelectNodes(string.Format("//*[@id='flexBox_flex_calendar_mainCal']//*[contains(@data-eventid,{0})]//td[contains(@class,'previous')]", eventId));
            HtmlNodeCollection forecastNode = doc.DocumentNode.SelectNodes(string.Format("//*[@id='flexBox_flex_calendar_mainCal']//*[contains(@data-eventid,{0})]//td[contains(@class,'forecast')]", eventId));

            //string result = "";

            if (previousNode[0].InnerText !="" || forecastNode[0].InnerText != "")
            {
                //column[5] = actualNode[counter].InnerHtml;
                if (actualNode[0].InnerHtml.Contains("Request"))
                {
                    Console.WriteLine("Keep refreshing timer");
                    aTimer.Enabled = true;
                }
                else
                {
                    refreshedActual = actualNode[0].InnerText;
                    Console.WriteLine(refreshedActual + "   YES, Save to database...");
                    aTimer.Enabled = false;
                    SaveDataToDatabase(60816, refreshedActual);

                }
            }
            else
            {
                Console.WriteLine("This is not an updateable event...");
            }
        }

        private void SaveDataToDatabase(int id, string actual)
        {

            //TO DO: THIS NEEDS TO BE UPDATED. 


            string updateActualData = string.Format("update economiceventdataRefresh set actual = '{0}'where id = '{1}'", actual, id);

            dbConn.Open();

            MySqlCommand cmd = new MySqlCommand(updateActualData, dbConn);
            cmd.ExecuteNonQuery();

            dbConn.Close();

        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Hello World!");
            this.ParseLocalHttpData(@"C:/Users/Karunyan/Documents/Reports/testLocalHTML/doing.html", 60816);
        }
    }

}

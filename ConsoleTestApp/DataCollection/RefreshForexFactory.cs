using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;


namespace ConsoleTestApp.DataCollection
{
    class RefreshForexFactory
    {
        private MySqlConnection dbConn = new MySqlConnection("server = localhost; database=algo;uid=root;password=Password1;");
        private List<string> checkEventId = new List<string>();
        public void FetchHTTPData()
        {
            //TO DO: Get the eventid for forefactory items currently stored in the database
            //Get timestampe to generate HTTP request. 
            //Monitor for refresh tag, if refresh tag is gone...and data is present. Get Data, save to database and display toolbox
            
            Console.WriteLine("ForexFactory.cs - Saved data to database...");

        }
        private void ParseHTTPData(string url)
        {
            checkEventId = this.CheckRecordedEventId();
            string lastDate = "";
            string lastTime = "";

            List<string[]> totalList = new List<string[]>();
            Dictionary<string, string[]> data = new Dictionary<string, string[]>();
            string[] column = new string[8];

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlWeb hw = new HtmlWeb();
            doc = hw.Load(url);

            HtmlNodeCollection dataId = doc.DocumentNode.SelectNodes("//*[@id='flexBox_flex_calendar_mainCal']//tr[contains(@class,'calendar__row calendar_row')]");
            HtmlNodeCollection dateNode = doc.DocumentNode.SelectNodes("//*[@id='flexBox_flex_calendar_mainCal']//tr[contains(@class,'calendar__row calendar_row')]//td[contains(@class,'date')]");
            HtmlNodeCollection timeNode = doc.DocumentNode.SelectNodes("//*[@id='flexBox_flex_calendar_mainCal']//tr[contains(@class,'calendar__row calendar_row')]//td[contains(@class,'time')]");
            HtmlNodeCollection currencyNode = doc.DocumentNode.SelectNodes("//*[@id='flexBox_flex_calendar_mainCal']//tr[contains(@class,'calendar__row calendar_row')]//td[contains(@class,'currency')]");
            HtmlNodeCollection impactNode = doc.DocumentNode.SelectNodes("//*[@id='flexBox_flex_calendar_mainCal']//tr[contains(@class,'calendar__row calendar_row')]//td[contains(@class,'impact')]");
            HtmlNodeCollection eventNode = doc.DocumentNode.SelectNodes("//*[@id='flexBox_flex_calendar_mainCal']//tr[contains(@class,'calendar__row calendar_row')]//td[contains(@class,'event')]");
            HtmlNodeCollection actualNode = doc.DocumentNode.SelectNodes("//*[@id='flexBox_flex_calendar_mainCal']//tr[contains(@class,'calendar__row calendar_row')]//td[contains(@class,'actual')]");
            HtmlNodeCollection forecastNode = doc.DocumentNode.SelectNodes("//*[@id='flexBox_flex_calendar_mainCal']//tr[contains(@class,'calendar__row calendar_row')]//td[contains(@class,'forecast')]");
            HtmlNodeCollection previousNode = doc.DocumentNode.SelectNodes("//*[@id='flexBox_flex_calendar_mainCal']//tr[contains(@class,'calendar__row calendar_row')]//td[contains(@class,'previous')]");

            //string result = "";
            for (int counter = 0; counter < dateNode.Count; counter++)
            {

                string dataEventId = Regex.Matches(dataId[counter].OuterHtml, "\\d+")[0].ToString();

                lastDate = (dateNode[counter].InnerText.Length == 0) ? lastDate : DateTime.Parse((dateNode[counter].InnerText.Substring(3) + " " + url.Substring(url.Length - 4))).ToShortDateString();


                lastTime = (timeNode[counter].InnerText.Length == 0) ? lastTime : timeNode[counter].InnerText;
                lastTime = lastTime.Replace("am", " AM");
                lastTime = lastTime.Replace("pm", " PM");

                column[0] = lastDate;
                column[1] = lastTime;
                column[2] = (currencyNode[counter].InnerText.Length == 0) ? "" : currencyNode[counter].InnerText;

                if (impactNode[counter].InnerHtml.Contains("High"))
                    column[3] = "High";
                else if (impactNode[counter].InnerHtml.Contains("Medium"))
                    column[3] = "Medium";
                else if (impactNode[counter].InnerHtml.Contains("Low"))
                    column[3] = "Low";
                else if (impactNode[counter].InnerHtml.Contains("holiday"))
                    column[3] = "Holiday";
                else if (impactNode[counter].InnerText.Length == 0)
                {
                    column[3] = "";
                    column[1] = "";
                }
                else
                    column[3] = "Non-Economic";

                column[4] = eventNode[counter].InnerText;
                column[5] = actualNode[counter].InnerText;
                column[6] = forecastNode[counter].InnerText;
                column[7] = previousNode[counter].InnerText;
                dataEventId = (dataEventId.Length < 3) ? "" : dataEventId;

                if (!dataEventId.Equals(""))
                {
                    this.SaveDataToDatabase(dataEventId,column[0],column[1], column[2], column[3], column[4], column[5], column[6], column[7]);
                }
            }
            //this.SaveDataToDatabase(data,this.CheckRecordedEventId());
        }
        private List<string> CheckRecordedEventId()
        {
            dbConn.Open();
            string query = "select id from economicEventData";
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
            Console.WriteLine("ForexFactory.cs - Retreieved recorded eventId records for sanity check...");
            return data;
        }

        private void SaveDataToDatabase(string eventId, string dateNum, string timeEst, string currency, string impact, string eventName, string actual, string forecast, string previous)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = dbConn;
            dbConn.Open();

                if (!checkEventId.Contains(eventId))
                    {
                        cmd.CommandText = "Insert into economicEventData values (@eventId, @dateNum,@timeEst,@currency,@impact,@event,@actual,@forecast,@previous)";
                        cmd.Parameters.Add(new MySqlParameter("@eventId", eventId));
                        cmd.Parameters.Add(new MySqlParameter("@dateNum", dateNum));
                        cmd.Parameters.Add(new MySqlParameter("@timeEst", timeEst));
                        cmd.Parameters.Add(new MySqlParameter("@currency", currency));
                        cmd.Parameters.Add(new MySqlParameter("@impact", impact));
                        cmd.Parameters.Add(new MySqlParameter("@event", eventName));
                        cmd.Parameters.Add(new MySqlParameter("@actual", actual));
                        cmd.Parameters.Add(new MySqlParameter("@forecast", forecast));
                        cmd.Parameters.Add(new MySqlParameter("@previous", previous));
                    cmd.ExecuteNonQuery();
                    }

            dbConn.Close();
        }
    }
}

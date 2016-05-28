using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Net;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    class ForexFactoryCalendar
    {
        private MySqlConnection dbconn = new MySqlConnection("server=localhost;database=algo;uid=root;password=Password1;");
        public string FetchXPATHData()
        {

            dbconn.Open();
            string[] monthRange = { "jan" };//, "feb" ,"mar","apr","may","jun","jul","aug","sep","oct","nov","dec"};
            string[] yearRange = { "2016"};
            //string ffUrl = "http://www.forexfactory.com/calendar.php?month=may.2016";

            string returnString = "";

            foreach (string year in yearRange)
            {
                foreach (string month in monthRange)
                {
                    string ffUrl = "http://www.forexfactory.com/calendar.php?month=" + month + "." + year;
                    returnString += this.Fetch(ffUrl) + Environment.NewLine;
                }
            }
            dbconn.Close();
            return (returnString);

        }
        
        public string RefreshEcondomicData()
        {
            //string currentDate = DateTime.Now.ToString("MMM").ToLower() + DateTime.Now.Day.ToString() + "." + DateTime.Now.Year.ToString();


            string tryURL = "http://www.forexfactory.com/calendar.php?day=may24.2016";

            Dictionary<int, string[][]> refreshData = new Dictionary<int, string[][]>();
            string selectTriggeringEvent = "select id, actual, forecast from economicData";

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlWeb hw = new HtmlWeb();
            doc = hw.Load(tryURL);

            HtmlNodeCollection refreshForcastNode = doc.DocumentNode.SelectNodes("*[@id='flexBox_flex_calendar_mainCal']//tr[contains(@data-eventid,62482)][contains(@class,'calendar__row calendar_row')]//td[contains(@class,'forcast')]");
            HtmlNodeCollection refreshAcutalNode = doc.DocumentNode.SelectNodes("*[@id='flexBox_flex_calendar_mainCal']//tr[contains(@data-eventid,62482)][contains(@class,'calendar__row calendar_row')]//td[contains(@class,'forcast')]");

            
            //XPATH Get triggered eventId Actual Data
            ////*[@id='flexBox_flex_calendar_mainCal']//tr[contains(@data-eventid,62482)][contains(@class,"calendar__row calendar_row")]//td[contains(@class,"actual")]
            return "";
        }
        
        private string Fetch(string url)  // Fetches Forex Factory Calendar Data using XPATH Expression and saves the data to the Database 
        {
            //TO DO: I need to handle some error checking for duplicate data. right now it's being handled by the duplicate key in the database. I would like to get to a point where the cold handles it, because I woul like to implement so realtime update when news is about to be released. 
            {

            string lastDate = "";
            string lastTime = "";

            List<string[]> totalList = new List<string[]>();
            string[] stats = new string[9];

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

            string result = "";
            for (int counter = 0; counter < dateNode.Count; counter++)
            {

                string dataEventId = Regex.Matches(dataId[counter].OuterHtml, "\\d+")[0].ToString();

                lastDate = (dateNode[counter].InnerText.Length == 0) ? lastDate : DateTime.Parse((dateNode[counter].InnerText.Substring(3) + " " + url.Substring(url.Length - 4))).ToShortDateString();


                lastTime = (timeNode[counter].InnerText.Length == 0) ? lastTime : timeNode[counter].InnerText;
                lastTime = lastTime.Replace("am"," AM");
                lastTime = lastTime.Replace ("pm", " PM");

                    stats[0] = lastDate;
                    stats[1] = lastTime;
                    stats[2] = (currencyNode[counter].InnerText.Length == 0) ? "" : currencyNode[counter].InnerText;

                    if (impactNode[counter].InnerHtml.Contains("High"))
                        stats[3] = "High";
                    else if (impactNode[counter].InnerHtml.Contains("Medium"))
                        stats[3] = "Medium";
                    else if (impactNode[counter].InnerHtml.Contains("Low"))
                        stats[3] = "Low";
                    else if (impactNode[counter].InnerHtml.Contains("holiday"))
                        stats[3] = "Holiday";
                    else if (impactNode[counter].InnerText.Length == 0)
                    {
                        stats[3] = "";
                        stats[1] = "";
                    }
                    else
                        stats[3] = "Non-Economic";

                    stats[4] = eventNode[counter].InnerText;
                    stats[5] = actualNode[counter].InnerText;
                    stats[6] = forecastNode[counter].InnerText;
                    stats[7] = previousNode[counter].InnerText;
                    stats[8] = (dataEventId.Length < 3) ? "" : dataEventId;

                    if (stats[8] != "") //Don't insert non-event days to the database. 
                    {
                        string insertStatement = String.Format("insert into economicData values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');",
                                    stats[8], stats[0], stats[1], stats[2], stats[3], stats[4],
                                    stats[5], stats[6], stats[7]);
                        try
                        {

                            MySqlCommand insertData = new MySqlCommand(insertStatement, dbconn);
                            insertData.ExecuteNonQuery();
                        }
                        catch
                        {
                            // Create eventlog for possible dublicate or error excetion. 
                        }
                        result += String.Format("{0}  {1}  {2}  {3} {4}  {5}  {6}  {7}  {8}  {9}", stats[0], stats[1], stats[2], stats[3], stats[4], stats[5], stats[6], stats[7], stats[8], Environment.NewLine);
                    }
            }
            return result;

        }
        }
    }
}

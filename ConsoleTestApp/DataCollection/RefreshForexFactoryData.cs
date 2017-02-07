using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;


namespace ConsoleTestApp.DataCollection
{
    class RefreshForexFactoryData
    {
        private int id;
        private DateTime eventTime = new DateTime();

        private System.Timers.Timer executeFetchEvent = new System.Timers.Timer();
        private System.Timers.Timer LoopTillCompleted = new System.Timers.Timer();
        private MySqlConnection dbConn = new MySqlConnection("server = localhost; database=algo;uid=root;password=Password1;");

        public RefreshForexFactoryData(int ID, DateTime EventTime)
        {
            // This class responsible for monitoring and updating the database value in realtime on a per event basis. 
            // TO DO: Currently the id and date is hard code. But in Time I hope to update to behave in realtime. 
            // TO DO: Tooltip and also error handling when internet connection is diconnected. 


            this.id = ID;
            this.eventTime = EventTime;

            //TO DO: Use current system time
            //DateTime currentTime = DateTime.Now;

            //DateTime currentTime = new DateTime(2016, 06, 22, 9, 59, 40);
            DateTime currentTime = DateTime.Now;

            //TO DO error handle when time < 0
            TimeSpan timeToEvent = eventTime - currentTime;

            executeFetchEvent.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            executeFetchEvent.Interval = (timeToEvent.TotalMilliseconds > 0) ? timeToEvent.TotalMilliseconds : 10;
            executeFetchEvent.Enabled = true;
            Console.WriteLine(string.Format("[{0}] {1}   wait ({2})", this.id,this.eventTime, timeToEvent));
        }


        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            // TO DO: Stop loop if Internet is disconnected or if Loop exceeds 10 minutes. 
            Console.WriteLine(string.Format("Begin fetching update for {0}  @  {1}",id,eventTime));

            executeFetchEvent.Enabled = false;
            LoopTillCompleted.Interval = 10;
            LoopTillCompleted.Enabled = true;
            LoopTillCompleted.Elapsed += new ElapsedEventHandler(OnRefreshEvent);


        }

        private void ParseLocalHttpData(int eventId)
        {
            string refreshedActual = "";
            //string url = @"C:/Users/Karunyan/Documents/Reports/testLocalHTML/doing.html";
            string url = "http://www.forexfactory.com/calendar.php?day=today";

            try
            {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                HtmlWeb hw = new HtmlWeb();
                doc = hw.Load(url);

                HtmlNodeCollection actualNode =
                    doc.DocumentNode.SelectNodes(
                        string.Format(
                            "//*[@id='flexBox_flex_calendar_mainCal']//*[contains(@data-eventid,{0})]//td[contains(@class,'actual')]",
                            eventId));
                HtmlNodeCollection previousNode =
                    doc.DocumentNode.SelectNodes(
                        string.Format(
                            "//*[@id='flexBox_flex_calendar_mainCal']//*[contains(@data-eventid,{0})]//td[contains(@class,'previous')]",
                            eventId));
                HtmlNodeCollection forecastNode =
                    doc.DocumentNode.SelectNodes(
                        string.Format(
                            "//*[@id='flexBox_flex_calendar_mainCal']//*[contains(@data-eventid,{0})]//td[contains(@class,'forecast')]",
                            eventId));

                if (previousNode[0].InnerText != "" || forecastNode[0].InnerText != "")
                {
                    if (actualNode[0].InnerHtml.Contains("Request"))
                    {
                        Console.WriteLine(string.Format("{0} Request icon still present, continue fetching...", id));
                        //aTimer.Enabled = true;
                    }
                    else
                    {
                        refreshedActual = actualNode[0].InnerText;
                        Console.WriteLine(string.Format("{0} Actual: {1}", id, refreshedActual));
                        LoopTillCompleted.Enabled = false;
                        SaveDataToDatabase(id, refreshedActual);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection to url lost...");
            }
        }

        private void OnRefreshEvent(object source, ElapsedEventArgs e)
        {
            LoopTillCompleted.Interval = 5000;
            this.ParseLocalHttpData(id);
        }

        private void SaveDataToDatabase(int id, string actual)
        {
            string updateActualData = string.Format("update economiceventdata set actual = '{0}'where id = '{1}'", actual, id);

            dbConn.Open();
            MySqlCommand cmd = new MySqlCommand(updateActualData, dbConn);
            cmd.ExecuteNonQuery();
            dbConn.Close();

            //TO DO: I might need to send the data back to the managing program to ballon tip in one sitting. 
            /*
            var notification = new NotifyIcon()
            {
                Visible = true,
                
                Icon = System.Drawing.SystemIcons.Information,
                BalloonTipText = "Existing Home Sales  	5.53M	5.53M"

            };

            notification.ShowBalloonTip(1);
            */
        }

    }
}

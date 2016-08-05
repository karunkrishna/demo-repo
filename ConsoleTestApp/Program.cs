using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTestApp.DataCollection;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Globalization;
//using System.Threading;
//using UITimer = System.Timers.Timer;
using System.Timers;
    //using System.Timers.ElapsedEventHandler;

namespace ConsoleTestApp
{
    class Program
    {
        //private List<int> currentDayEventId = new List<int>();  
         
        private static MySqlConnection dbConn = new MySqlConnection("server = localhost; database=algo;uid=root;password=Password1;");
        private Timer executeFetchEvent = new Timer();


        static void Main(string[] args)
        {
            Dictionary<int, DateTime> currentEventandTime = new Dictionary<int, DateTime>();
            Dictionary<int, string> isIssueCurrentEventandTime = new Dictionary<int, string>();
            string queryEventId = "select id, concat(concat(datenum,' '),timeEst) from ( select * from economiceventdataRefresh where forecast <> '' or previous <> '') xd where xd.datenum = '2016-06-22'";

            dbConn.Open();
            using (MySqlCommand command = new MySqlCommand(queryEventId, dbConn))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        DateTime convertedDate = new DateTime();

                        if (DateTime.TryParse(reader.GetString(1), out convertedDate))
                        {
                            currentEventandTime.Add(reader.GetInt32(0), convertedDate);
                        }
                        else
                        {
                            //Test out this case please....
                            isIssueCurrentEventandTime.Add(reader.GetInt32(0),reader.GetString(1));
                        }
                    }
                }
            }
            dbConn.Close();

            //createTimerEvent();
            DateTime eventTime = new DateTime(2016, 06, 22, 10, 00, 00);

            //http://stackoverflow.com/questions/314008/programatically-using-a-string-as-object-name-when-instantiating-an-object

            foreach (KeyValuePair<int, DateTime> id in currentEventandTime)
            {

                new RefreshForexFactoryData(id.Key, id.Value);
            }

            //RefreshForexFactoryData eventA = new RefreshForexFactoryData(60816, eventTime);
            //RefreshForexFactoryData eventB = new RefreshForexFactoryData(62333, eventTime);

            Console.ReadLine();
        }

        #region Test cases for various HTML Parsing Programs

        // System.Collections.Generic.KeyValuePair<int, System.DateTime>


        //LocalForexFactory csvLocalForexFactory = new LocalForexFactory();
        //csvLocalForexFactory.FetchLocalHTTPData();

        //ForexFactory csvForexFactory = new ForexFactory();
        //csvForexFactory.FetchHTTPData();


        //CommitmentOfTraders csv = new CommitmentOfTraders();
        //StateStreet csvStateStreet = new StateStreet();




        //YahooFinance checkUrl = new YahooFinance();
        //EconomicEventHeatmap getHeatmap = new EconomicEventHeatmap();

        //EMailAlert sendEmail = new EMailAlert();

        //Morningstar getFinacials = new Morningstar();

        //Zacks date = new Zacks();
        //CMEGroup settlement = new CMEGroup();
        // YahooFinance vix = new YahooFinance();
        // vix.SaveSingleData("^VIX");

        // KeyAlerts getData = new KeyAlerts();

        //Load historical data
        //csv.CsvFetchCommitmentofTraders();
        //csvStateStreet.DownloadXLSFile();
        //csvForexFactory.FetchHTTPData();

        //Sunday secheduler to collect data
        //csv.HttpFetchCommitmentOfTraders();

        //Time sensitive scheduler to collect data
        //

        //Download Equitities data
        //checkUrl.RunURLChecker();


        #endregion

        private void createTimerEvent()
        {
            // I WILL BE SETTING UP A SCHEDULE TASK BASED OF ELAPSED TIME HERE. 
            // I HOPE THAT IT DOES NOT GET MESSY. LETS DO IT FOR ONE CASE, AND THEN EXPAND TO MULTIPLE THREADS. 

            //hardcoded data: 
            Dictionary<int, DateTime> testEventDetails = new Dictionary<int, DateTime>();
            int testId = 60816;
            DateTime testDateTime = new DateTime(2016, 06, 22, 10, 00, 00);
            testEventDetails.Add(testId, testDateTime);

            DateTime currentTime = DateTime.Now;
            TimeSpan timeToEvent = testDateTime - currentTime;
            double totalMsToEvent = timeToEvent.TotalMilliseconds;

            //Now I have this elapsed time, I need a way to trigger the event, and actually do the call request to the HTML page. 
            //Create a unix comand to quickly convert before and after page. 


            executeFetchEvent.Interval = totalMsToEvent;
            executeFetchEvent.Elapsed += new ElapsedEventHandler(OnTimedEvent);

        }
        private  void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("We have went about fetching the data");
            //this.ParseLocalHttpData(@"C:/Users/Karunyan/Documents/Reports/testLocalHTML/doing.html", 60816);
            executeFetchEvent.Enabled = false;
        }

    }

}


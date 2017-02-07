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
            //This will download the data from the website. 

            LinearRegression test = new LinearRegression();

            Console.ReadKey();

        }

        public static void GetInfoAndSendData()
        {
            try
            {
                ForexFactory csvForexFactory = new ForexFactory();
                csvForexFactory.FetchHTTPData();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not download upcoming events. Connection to internet is not available...");
            }

            //Then I need to run the automatic refresher. 

            Dictionary<int, DateTime> currentEventandTime = new Dictionary<int, DateTime>();
            Dictionary<int, string> isIssueCurrentEventandTime = new Dictionary<int, string>();
            //string queryEventId = "select id, concat(concat(datenum,' '),timeEst) from ( select * from economiceventdata where forecast <> '' or previous <> '') xd where xd.datenum = '2016-06-22'";
            string queryEventId = string.Format("select id, concat(concat(datenum,' '),timeEst) from ( select * from economiceventdata where forecast <> '' or previous <> '') xd where xd.datenum = '{0}'", DateTime.Now.ToShortDateString());

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
                            isIssueCurrentEventandTime.Add(reader.GetInt32(0), reader.GetString(1));
                        }
                    }
                }
            }
            dbConn.Close();

            Console.WriteLine("List of events to monitor");
            Console.WriteLine("------------------------------");
            foreach (KeyValuePair<int, DateTime> id in currentEventandTime)
            {

                new RefreshForexFactoryData(id.Key, id.Value);
            }

            Console.WriteLine("List of events that are not monitored");
            Console.WriteLine("------------------------------");

            foreach (KeyValuePair<int, string> id in isIssueCurrentEventandTime)
            {

                Console.WriteLine(string.Format("[{0}] {1}", id.Key, id.Value));
            }

            EMailAlert send = new EMailAlert();
            send.SendEmail();

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

    }

}


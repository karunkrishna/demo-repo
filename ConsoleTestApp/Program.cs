using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTestApp.DataCollection;
using MySql.Data.MySqlClient;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {

            //ForexFactory csvForexFactory = new ForexFactory();
            //csvForexFactory.FetchHTTPData();


            //CommitmentOfTraders csv = new CommitmentOfTraders();
            //StateStreet csvStateStreet = new StateStreet();
            LocalForexFactory csvLocalForexFactory = new LocalForexFactory();
            csvLocalForexFactory.FetchLocalHTTPData();



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



            Console.ReadLine();
        }
    }
}

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
            //CommitmentOfTraders csv = new CommitmentOfTraders();
            //StateStreet csvStateStreet = new StateStreet();
            //ForexFactory csvForexFactory = new ForexFactory();
            //YahooFinance checkUrl = new YahooFinance();
            //EconomicEventHeatmap getHeatmap = new EconomicEventHeatmap();
            Zacks date = new Zacks();



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

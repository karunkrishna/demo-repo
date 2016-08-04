using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ConsoleTestApp
{
    class KeyAlerts
    {
        private List<string[]> listEventData = new List<string[]>();
        private List<string[]> listReleaseData = new List<string[]>();

        private MySqlConnection dbConn = new MySqlConnection("server = localhost; database=algo;uid=root;password=Password1;");

        public KeyAlerts()
        {
            this.GetEconomicData();
            this.GetEarningRelease();
        }

        private void GetEconomicData()
        {
            dbConn.Open();
            DateTime startDate = new DateTime(2016, 02, 14);
            DateTime endDate = new DateTime(2016, 02, 17);

            string query = string.Format("select* from economiceventdata where dateNum between '{0}' and '{1}' order by " +
                                          "concat(concat(dateNum, ' '), timeEst) desc", startDate, endDate);

            using (MySqlCommand command = new MySqlCommand(query, dbConn))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        string[] record = new string[8];
                        record[0] = reader.GetString(0);
                        record[1] = reader.GetString(1);
                        record[2] = reader.GetString(2);
                        record[3] = reader.GetString(3);
                        record[4] = reader.GetString(4);
                        record[5] = reader.GetString(5);
                        record[6] = reader.GetString(6);
                        record[7] = reader.GetString(7);

                        listEventData.Add(record);

                    }
                }
            }

            Console.WriteLine("Upcoming USD High and Medium Events");
            foreach (var s in listEventData)
            {
                if (s[2].Contains("AM") || s[2].Contains("PM")) // I need to modify this so that It will show holiday for future event. 
                {
                    if (s[3].Contains("USD"))
                        if (s[4].Contains("High") || s[4].Contains("Medium"))
                            Console.WriteLine(string.Format("{0}  {1}  {2}  {3}  {4}  {5}", DateTime.Parse(s[1] + " " + s[2]).ToString("yyyy-MM-dd HH:mm"), s[3], s[4], s[5], s[6], s[7]));
                }
            }


            Console.WriteLine(Environment.NewLine + "Upcoming USD and EUR Holidays");
            foreach (var s in listEventData)
            {
                if (s[4].Contains("Holiday") && (s[3].Contains("USD") || s[3].Contains("EUR")))
                {
                   Console.WriteLine(string.Format("{0}  {1}  {2}  {3}  {4}  {5}", s[1] + " " + s[2], s[3], s[4], s[5], s[6], s[7]));
                }
            }
            dbConn.Close();
        }

        private void GetEarningRelease()
        {
            dbConn.Open();
            DateTime startDate = new DateTime(2016, 06, 14);
            DateTime endDate = new DateTime(2016, 08, 17);

            string query = string.Format("select x1.datenum,x1.symbol,x1.description,x1.releasetime,x1.estimate,x1.reported,x1.suprise,x1.changeper as chang,s1.weight from zacksEarnings x1, etfData s1 where x1.symbol = s1.underlyingSymbol and s1.sectorSymbol = 'SPY' and x1.datenum between '{0}' and '{1}' and s1.weight > 0.70 order by x1.datenum asc", startDate, endDate);


            Console.WriteLine(Environment.NewLine + "Earnings Release of High Weigted Symbols");
            using (MySqlCommand command = new MySqlCommand(query, dbConn))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        string[] record = new string[9];
                        record[0] = reader.GetString(0);
                        record[1] = reader.GetString(1);
                        record[2] = reader.GetString(2);
                        record[3] = reader.GetString(3);
                        record[4] = reader.GetString(4);
                        record[5] = reader.GetString(5);
                        record[6] = reader.GetString(6);
                        record[7] = reader.GetString(7);
                        record[8] = reader.GetString(8);

                        listReleaseData.Add(record);

                    }
                }
            }

            foreach (var s in listReleaseData)
            {
                    Console.WriteLine(string.Format("{0}  {1}  {2}  {3}  {4}  {5}  {6}  {7}   {8}", s[0], s[1], s[2], s[3], s[4], s[5], s[6], s[7],s[8]));
            }

            dbConn.Close();
        }
    }
}

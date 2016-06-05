using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ConsoleTestApp
{
    public class EconomicEventHeatmap
    {
        //I just had this ideas to extra the ninjatrader chart details use a dictionary for they key as timestamp will be unique. Then I can has out some volatility based on earnings release, etc. 
       private MySqlConnection dbConnection = new MySqlConnection("server=localhost;database=algo;uid=root;password=Password1;");
       private Dictionary<string, List<string[]>> economicEvents = new Dictionary<string, List<string[]>>();

        public EconomicEventHeatmap()
        {
            dbConnection.Open();
            string[] date = { "2016-06-02", "2016-06-01" };

            foreach (var d in date)
            {
                string query = string.Format("select concat(concat(dateNum,' '),timeEst), currency, impact, event, actual,forecast from economiceventdata where datenum = '{0}'",d);
                List<string[]> recordMatrix = new List<string[]>();
               

                using (MySqlCommand cmd = new MySqlCommand(query, dbConnection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(0).Contains("AM") || reader.GetString(0).Contains("PM"))
                            {
                                string[] record = new string[6];
                                DateTime estModifiedTime =(TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Parse(reader.GetString(0))) == true) ? DateTime.Parse(reader.GetString(0)).AddHours(1) : DateTime.Parse(reader.GetString(0));

                                record[0] = estModifiedTime.ToString();
                                record[1] = reader.GetString(1);
                                record[2] = reader.GetString(2);
                                record[3] = reader.GetString(3);
                                record[4] = reader.GetString(4);
                                record[5] = reader.GetString(5);

                                recordMatrix.Add(record);
                            }

                        }
                    }
                    economicEvents.Add(d, recordMatrix);
                }
            }

        }
    }

}
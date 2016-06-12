using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;

namespace ConsoleTestApp
{
    class Zacks
    {
        private int lookbackDays = 148;
        private DateTime date = new DateTime(2016, 08, 06);

        private List<DateTime> fetchDates = new List<DateTime>();
        Dictionary<DateTime, List<string[]>> zackEarningsStat = new Dictionary<DateTime, List<string[]>>();
        private MySqlConnection dbConn = new MySqlConnection("server = localhost; database=algo;uid=root;password=Password1;");

        public Zacks()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            for (int i = 0; i < lookbackDays; i++)
                fetchDates.Add(date.AddDays(+1 * i));

            foreach (var d in fetchDates)
            {
                zackEarningsStat.Add(d,this.FetchData(this.GetUnixTime(d.ToShortDateString())));

            }

            foreach (KeyValuePair<DateTime, List<string[]>> pair in zackEarningsStat)
            {
                foreach (string[] s in pair.Value)
                {
                   // Console.WriteLine(string.Format("{0} {1} {2} {3} {4} {5} {6} {7}",pair.Key.ToShortDateString(), s[0], s[1], s[2], s[3], s[4],
                     //   s[5], s[6]));

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = dbConn;
                    dbConn.Open();
                    cmd.CommandText = "Insert into zacksEarnings values (@dateNum,@symbol,@desc,@time,@estimate,@reported,@suprise,@change)";
                    cmd.Parameters.Add(new MySqlParameter("@dateNum", pair.Key.ToShortDateString()));
                    cmd.Parameters.Add(new MySqlParameter("@symbol", s[0]));
                    cmd.Parameters.Add(new MySqlParameter("@desc", s[1]));
                    cmd.Parameters.Add(new MySqlParameter("@time", s[2]));
                    cmd.Parameters.Add(new MySqlParameter("@estimate", s[3]));
                    cmd.Parameters.Add(new MySqlParameter("@reported", s[4]));
                    cmd.Parameters.Add(new MySqlParameter("@suprise", s[5]));
                    cmd.Parameters.Add(new MySqlParameter("@change", s[6]));
                    cmd.ExecuteNonQuery();
                    dbConn.Close();
                }
            }
            timer.Stop();
            Console.WriteLine(timer.Elapsed);
            
        }

        private string GetUnixTime(string date)
        {
            int year = DateTime.Parse((date)).Year;
            int month = DateTime.Parse(date).Month;
            int day = DateTime.Parse(date).Day;

            var dateTime = new DateTime(year, month, day, 9, 0, 0, DateTimeKind.Utc);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var unixDateTime = (dateTime.ToLocalTime() - epoch).TotalSeconds;

            return Convert.ToString(unixDateTime);
        }
        private List<string[]> FetchData(string unixTime)
        {
            string url = string.Format("https://www.zacks.com/includes/classes/z2_class_calendarfunctions_data.php?calltype=eventscal&date={0}&type=1",unixTime);
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            HttpWebResponse reposne = (HttpWebResponse) request.GetResponse();

            StreamReader sr = new StreamReader(reposne.GetResponseStream());
            string page = sr.ReadToEnd();
            string[] parsedData = page.Split('[');
            
            List<string[]> earningsMatrix = new List<string[]>();


            for (int i = 2; i < parsedData.Count(); i++)
            {
                bool recordData = true;
                string[] earningsDetail = new string[7];

                MatchCollection symbol = Regex.Matches(parsedData[i], @"symbol(.+?)</span>", RegexOptions.Singleline);
                MatchCollection description = Regex.Matches(parsedData[i], @"hotspot(.+?)</span>", RegexOptions.Singleline);
                MatchCollection content = Regex.Matches(parsedData[i], "</span>\", (.+?)]", RegexOptions.Singleline);

                foreach (Match match in symbol)
                {
                    StringBuilder sb = new StringBuilder(match.Groups[0].Value);
                    string data = sb
                        .Replace("symbol", string.Empty)
                        .Replace(@"\", string.Empty)
                        .Replace(@"/", string.Empty)
                        .Replace(">", string.Empty)
                        .Replace("<", string.Empty)
                        .Replace("span", string.Empty)
                        .Replace(@"""", string.Empty)
                        .ToString();

                    earningsDetail[0] = data;
                }
                foreach (Match match in description)
                {
                    StringBuilder sb = new StringBuilder(match.Groups[0].Value);
                    string data = sb
                        .Replace("hotspot", string.Empty)
                        .Replace(@"\", string.Empty)
                        .Replace(@"/", string.Empty)
                        .Replace(">", string.Empty)
                        .Replace("<", string.Empty)
                        .Replace("span", string.Empty)
                        .Replace(@"""", string.Empty)
                        .ToString();

                    earningsDetail[1] = data;
                }
                foreach (Match match in content)
                {
                    string[] data = match.Groups[0].Value.Split(',');


                    for (int j = 1; j < data.Count(); j++)
                    {
                        StringBuilder sb = new StringBuilder(data[j]);
                        string text = sb
                            .Replace(@"\", string.Empty)
                            .Replace(@"/", string.Empty)
                            .Replace(">", string.Empty)
                            .Replace("<", string.Empty)
                            .Replace("span", string.Empty)
                            .Replace("div", string.Empty)
                            .Replace(@"""", string.Empty)
                            .Replace("]", string.Empty)
                            .Replace(@"--", string.Empty)
                            .Replace("down", string.Empty)
                            .Replace("class", string.Empty)
                            .Replace(@"up", string.Empty)
                            .Replace(@"=", string.Empty)
                            .ToString().Trim();

                        try
                        {
                            earningsDetail[1 + j] = text;
                            //There are penny stock in the report, which change by 1,200% which is causing the parsed 
                            //data to fail. I've just skipped them.
                        }
                        catch (Exception)
                        {
                            recordData = false;
                        }
                        
                    }
                }
                if (recordData =true)
                    earningsMatrix.Add(earningsDetail);
            }
            return earningsMatrix;
        }
        public void GetEarnings()
        {

            #region FireBug and some hacking information
            // I am having a problem with javascript on the Zacks website. 
            // I am going to use PhantomJs and CasperJs to scrape. 
            //https://www.youtube.com/watch?v=Cqic-ZKPFyk
            //I would still like to conduct most of the code using C#. But I would like to learn how to 
            //trigger the js event, load the phase, scape the data into a string, send it to C# for parsing 
            // and collection. 


            //It looks like I found a way to avoid all the phantommethods. 

            //looks like data goes back to 1400994000 Which begins on may/25/2014. 
            //I wonder if it is a just because we are currently on May/29th/2016 (1464498000).
            // I was able to get these request using firebug inspection. When you click on the button, 
            //the console in firebug prints something.   

            //here are some of the calls
            //https://www.zacks.com/includes/classes/z2_class_calendarfunctions_data.php?calltype=weeklycal
            //https://www.zacks.com/includes/classes/z2_class_calendarfunctions_data.php?calltype=eventscal&date=1464498000&type=5

            // the datevalue and the type is very important 5= divident, and 1 = earnings
            //https://www.zacks.com/includes/classes/z2_class_calendarfunctions_data.php?calltype=eventscal&date=1401253200&type=5
            // XAPTH Statement //tr[contains(@class,"odd") or contains(@class,"even")]  
            //Epoch time converter: 1463288400
            #endregion



        }
    }
}

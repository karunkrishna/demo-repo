using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Data;
using HtmlAgilityPack;


namespace WindowsFormsApplication1
{
    class FetchFromForexFactory
    {

        private string insertEconData;
        private bool isFirstEntry = true;

        private DataTable RecordEntry()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Timestamp");
            dt.Columns.Add("Currency");
            dt.Columns.Add("Impact");
            dt.Columns.Add("EventId");
            dt.Columns.Add("Actual");
            dt.Columns.Add("Forcast");
            dt.Columns.Add("Previous");
            dt.AcceptChanges();

            return dt;

        }

        public string FetchDataUsingHTMLAgility()
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlWeb hw = new HtmlWeb();
            doc = hw.Load("http://www.forexfactory.com/calendar.php?day=today");

            //HtmlNode ourNode = doc.DocumentNode.SelectNodes("//div 
            return doc.ToString();
        }

        public void FetchData()
        {
            if (EventLog.SourceExists("TheCoffeeShopTrader"))
            {

            }
            else
            {
                System.Diagnostics.EventLog.CreateEventSource("TheCoffeeShopTrader", "TheCoffeeShopTrader");
            }
            

            string currentDate = DateTime.Now.ToString("MMM").ToLower() + DateTime.Now.Day.ToString() + "." + DateTime.Now.Year.ToString();
            string url = "http://www.forexfactory.com/calendar.php?day=" + currentDate;


            insertEconData = "";
            isFirstEntry = true;
            string parseDateRef = "";
            string parseTimeRef = "";

            List<string> listDate = new List<string>();
            List<string> listTime = new List<string>();
            List<string> listCurrency = new List<string>();
            List<string> listImpact = new List<string>();
            List<string> listEvent = new List<string>();
            List<string> listActual = new List<string>();
            List<string> listForcast = new List<string>();
            List<string> listPrevious = new List<string>();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StreamReader websr = new StreamReader(response.GetResponseStream());
            string sourceCode = websr.ReadToEnd();

            int startIndex = sourceCode.IndexOf("calendar__row");
            int endIndex = sourceCode.LastIndexOf("calendar__row");
            sourceCode = sourceCode.Substring(startIndex, endIndex - startIndex);

            MatchCollection parseDate = Regex.Matches(sourceCode, @"calendar__cell calendar__date date"">\s*(.+?)</td>", RegexOptions.Singleline);
            MatchCollection parseTime = Regex.Matches(sourceCode, @"calendar__cell calendar__time time"">\s*(.+?)</td>", RegexOptions.Singleline);
            MatchCollection parseCurrency = Regex.Matches(sourceCode, @"calendar__cell calendar__currency currency"">s*(.+?)</td>", RegexOptions.Singleline);
            MatchCollection parseImpact = Regex.Matches(sourceCode, @"calendar__cell calendar__impact impact calendar__impact calendar__impact--\s*(.+?)</td>", RegexOptions.Singleline);
            MatchCollection parseEvent = Regex.Matches(sourceCode, @"td class=""calendar__cell calendar__event event""> <div> <span class=""calendar__event-title"">\s*(.+?)</span> </div> </td>", RegexOptions.Singleline);
            MatchCollection parseActual = Regex.Matches(sourceCode, @"td class=""calendar__cell calendar__actual actual"">\s*(.+?)</td>", RegexOptions.Singleline);
            MatchCollection parseForcast = Regex.Matches(sourceCode, @"td class=""calendar__cell calendar__forecast forecast"">\s*(.+?)</td>", RegexOptions.Singleline);
            MatchCollection parsePrevious = Regex.Matches(sourceCode, @"td class=""calendar__cell calendar__previous previous"">\s*(.+?)</td>", RegexOptions.Singleline);
            //MatchCollection parseNoEventDate = Regex.Matches(sourceCode, @"data-eventid=""\s*(.+?)""\s*(.+?) data-touchable> <td class=""calendar__cell calendar__date date"">", RegexOptions.Singleline);


            foreach (Match m in parseDate)
            {
                MatchCollection DateOnly = Regex.Matches(m.Groups[1].Value, @"<span>\s*(.+?)</span>", RegexOptions.Singleline);

                foreach (Match extract in DateOnly)
                    parseDateRef = extract.Groups[1].Value + " " + url.Substring(url.Length - 4);

                listDate.Add(parseDateRef);
            }

            foreach (Match m in parseTime)
            {
                if (m.Groups[1].Value.Length < 50) // when there is additional tag compoent becase time is blank
                    parseTimeRef = m.Groups[1].Value;

                parseTimeRef = parseTimeRef.Replace("am", " AM");
                parseTimeRef = parseTimeRef.Replace("pm", " PM");

                listTime.Add(parseTimeRef);
            }

            foreach (Match m in parseCurrency)
                listCurrency.Add(m.Groups[1].Value);


            foreach (Match m in parseImpact)
            {
                MatchCollection ImpactOnly = Regex.Matches(m.Groups[1].Value, @"lass=""\s*(.+?)""></span>", RegexOptions.Singleline);
                foreach (Match extract in ImpactOnly)
                    listImpact.Add(extract.Groups[1].Value);
            }


            foreach (Match m in parseEvent)
                listEvent.Add(m.Groups[1].Value);

            foreach (Match m in parseActual)
            {
                if (m.Groups[1].Value.Length < 25)
                {
                    listActual.Add(m.Groups[1].Value);
                }
                else if (m.Groups[1].Value.Length < 50)
                {
                    MatchCollection ActualOnly = Regex.Matches(m.Groups[1].Value, @">\s*(.+?)</span>", RegexOptions.Singleline);
                    foreach (Match extract in ActualOnly)
                        listActual.Add(extract.Groups[1].Value);
                }
                else if (m.Groups[1].Value.Length == 61)
                {
                    listActual.Add("");
                }
            }

            foreach (Match m in parseForcast)
            {

                if (m.Groups[1].Value.Length < 50) // when there is additional tag compoent becase forcast numbers is blank
                    listForcast.Add(m.Groups[1].Value);
                else
                    listForcast.Add("");
            }

            foreach (Match m in parsePrevious)
            {
                if (m.Groups[1].Value.Length < 25)
                    listPrevious.Add(m.Groups[1].Value);
                else if (m.Groups[1].Value.Length == 55)
                    listPrevious.Add("");
                else if (m.Groups[1].Value.Length > 50) // when previous data is revised better, worse on ""
                {
                    MatchCollection ActualOnly = Regex.Matches(m.Groups[1].Value, @">\s*(.+?)</span>", RegexOptions.Singleline);
                    foreach (Match extract in ActualOnly)
                        listPrevious.Add(extract.Groups[1].Value);
                }
            }



            for (int i = 0; i <= listDate.Count - 1; i++)
            {
                if (listCurrency[i] == "USD" && (listImpact[i] == "high" || listImpact[i] == "holiday") && !listEvent[i].Contains("Crude Oil"))
                {
                    DateTime convertTextToTime;
                    if (listTime[i].Contains("AM") || listTime[i].Contains("PM"))
                    {
                        convertTextToTime = DateTime.Parse(listTime[i]);
                        listTime[i] = convertTextToTime.ToString("HH:mm");
                    }

                    if (isFirstEntry)
                    {
                        insertEconData += String.Format("{1}    {4}{8}",
                        listDate[i], listTime[i], listCurrency[i], listImpact[i], listEvent[i], listActual[i], listForcast[i], listPrevious[i], Environment.NewLine);
                        isFirstEntry = false;
                        
                    }
                    else
                    {
                        if (listTime[i] == listTime[i - 1])
                        {
                            insertEconData += String.Format("{1}    {4}{8}",
                        listDate[i], "         ", listCurrency[i], listImpact[i], listEvent[i], listActual[i], listForcast[i], listPrevious[i], Environment.NewLine);
                        }
                        else
                        {
                            insertEconData += String.Format("{1}    {4}{8}",
                                listDate[i], listTime[i], listCurrency[i], listImpact[i], listEvent[i], listActual[i], listForcast[i], listPrevious[i], Environment.NewLine);
                        }
                    }
                }
            }
            if (EventLog.SourceExists("TheCoffeeShopTrader"))
            {
                EventLog log = new EventLog("TheCoffeeShopTrader");
                log.Source = "TheCoffeeShopTrader";

                log.WriteEntry(insertEconData, EventLogEntryType.Information);
            }
        }
    }
}

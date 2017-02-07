using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Mail;
using MySql.Data.MySqlClient;


namespace ConsoleTestApp
{
    class EMailAlert
    {
        //karun.xmail@gmail.com
        //xmail1234
        private string returnResult = "";
        private static MySqlConnection dbConn = new MySqlConnection("server = localhost; database=algo;uid=root;password=Password1;");
        public void SendEmail()
        {
            Console.WriteLine("Send weekly economic event report");
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("karun.xmail@gmail.com", "xmail1234");

            GetThisWeekInEvents();
            MailMessage mm = new MailMessage("donotreply@domain.com", "karun.xmail@gmail.com", "This Week In Events", returnResult);
            //mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.IsBodyHtml = false;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            try
            {
                client.Send(mm);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not send email due to internet connection...");
            }
            


        }

        private void GetThisWeekInEvents()
        {
            Console.WriteLine(DateTime.Now);
            DateTime startOfWeekDateTime = DateTime.Now.StartOfWeek(DayOfWeek.Sunday);
            DateTime endOfWeekDateTime = startOfWeekDateTime.AddDays(7);


            Console.WriteLine(string.Format("Week start and end date: {0} | {1}",startOfWeekDateTime,endOfWeekDateTime));

            string queryEventId = string.Format("select * from economiceventdata where datenum >= '{0}' and datenum < '{1}' and currency='USD' and impact in ('Medium','High','Holiday') order by datenum asc",startOfWeekDateTime,endOfWeekDateTime);
            

            dbConn.Open();
            using (MySqlCommand command = new MySqlCommand(queryEventId, dbConn))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        returnResult = returnResult+ String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}", reader.GetString(1), reader.GetString(2),
                            reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6),
                            reader.GetString(7), reader.GetString(8), Environment.NewLine);
                    }
                }
            }
            dbConn.Close();
        }

    }
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }
    }
}

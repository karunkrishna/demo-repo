using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using MySql.Data.MySqlClient;

namespace ConsoleTestApp.DataCollection
{
    class CommitmentOfTraders
    {
        private MySqlConnection dbConn = new MySqlConnection("server = localhost; database=algo;uid=root;password=Password1;");

        public void FetchCsvData()
        {
            string[] fileEntries = Directory.GetFiles(@"C:\Users\Karunyan\Documents\Reports\_commitmentOfTraders");
            foreach (string fileName in fileEntries)
            {
                StreamReader sr = new StreamReader(fileName);

                dbConn.Open();
                List<string> checkDate = this.GetRecordedDateList();
                string data = sr.ReadLine();
                while (data != null)
                {
                    this.SaveDataToDatabase(data, checkDate);
                    data = sr.ReadLine();
                }
                dbConn.Close();
                Console.WriteLine("TXT Data collected");
            }
        }
        public void FetchHttpData()
        {

            string url = "http://www.cftc.gov/dea/newcot/FinFutWk.txt";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream());

            dbConn.Open();
            List<string> checkDate = this.GetRecordedDateList();
            string data = sr.ReadLine();
            while (data != null)
            {
                this.SaveDataToDatabase(data, checkDate);
                data = sr.ReadLine();
            }
            dbConn.Close();
            Console.WriteLine("HTTP Data collected");


        }
        private List<string> GetRecordedDateList()
        {
            string query = "select distinct(lastupdatedTime) from CommitmentOfTradersData;";
            List<string> data = new List<string>();

            using (MySqlCommand command = new MySqlCommand(query, dbConn))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        data.Add(reader.GetString(0));
                    }
                }
            }

            return data;
        }
        private void SaveDataToDatabase(string data, List<string> checkDate)
        {

            string[] parchedData = data.Split(',');
            if (!checkDate.Contains(parchedData[2].ToString()))
            {
                if (!parchedData[0].Contains("Market_and_Exchange_Names"))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = dbConn;
                    cmd.CommandText = "INSERT INTO commitmentoftradersdata values(@Description, @Code, @Multiplier, @Exchange, @OpenInterest, " +
                        "@DI_Long, @DI_Short, @DI_Spreading, @AS_Long, @AS_Short, @As_Spreading, @LF_Long, @LF_Short, @LF_Spreading, @OT_Long, " +
                        "@OT_Short, @OT_Spreading, @NON_Long, @NON_Short, @Chng_DI_Long, @Chng_DI_Short, @Chng_DI_Spreading, @Chng_AS_Long, " +
                        "@Chng_AS_Short, @Chng_As_Spreading, @Chng_LF_Long, @Chng_LF_Short, @Chng_LF_Spreading, @Chng_OT_Long, @Chng_OT_Short, " +
                        "@Chng_OT_Spreading, @Chng_NON_Long, @Chng_NON_Short, @OIpct_DI_Long, @OIpct_DI_Short, @OIpct_DI_Spreading, @OIpct_AS_Long, " +
                        "@OIpct_AS_Short, @OIpct_As_Spreading, @OIpct_LF_Long, @OIpct_LF_Short, @OIpct_LF_Spreading, @OIpct_OT_Long, @OIpct_OT_Short, " +
                        "@OIpct_OT_Spreading, @OIpct_NON_Long, @OIpct_NON_Short, @TotalTraders, @trds_DI_Long, @trds_DI_Short, @trds_DI_Spreading, " +
                        "@trds_AS_Long, @trds_AS_Short, @trds_As_Spreading, @trds_LF_Long, @trds_LF_Short, @trds_LF_Spreading, @trds_OT_Long, " +
                        "@trds_OT_Short, @trds_OT_Spreading,@LastUpdatedTime);";

                    cmd.Parameters.Add(new MySqlParameter("@Description", parchedData[0].ToString().Replace('"', ' ').Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@Code", parchedData[3].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@Multiplier", parchedData[81].ToString().Trim().Replace('"', ' ').Trim()
                        + ((parchedData[82].ToString().Contains(")") ? parchedData[82].ToString().Trim().Replace('"', ' ').Trim() : ""))));
                    cmd.Parameters.Add(new MySqlParameter("@Exchange", parchedData[4].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OpenInterest", parchedData[7].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@DI_Long", parchedData[8].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@DI_Short", parchedData[9].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@DI_Spreading", parchedData[10].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@AS_Long", parchedData[11].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@AS_Short", parchedData[12].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@As_Spreading", parchedData[13].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@LF_Long", parchedData[14].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@LF_Short", parchedData[15].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@LF_Spreading", parchedData[16].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OT_Long", parchedData[17].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OT_Short", parchedData[18].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OT_Spreading", parchedData[19].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@NON_Long", parchedData[22].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@NON_Short", parchedData[23].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@Chng_DI_Long", parchedData[25].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@Chng_DI_Short", parchedData[26].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@Chng_DI_Spreading", parchedData[27].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@Chng_AS_Long", parchedData[28].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@Chng_AS_Short", parchedData[29].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@Chng_As_Spreading", parchedData[30].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@Chng_LF_Long", parchedData[31].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@Chng_LF_Short", parchedData[32].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@Chng_LF_Spreading", parchedData[33].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@Chng_OT_Long", parchedData[34].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@Chng_OT_Short", parchedData[35].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@Chng_OT_Spreading", parchedData[36].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@Chng_NON_Long", parchedData[39].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@Chng_NON_Short", parchedData[40].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OIpct_DI_Long", parchedData[42].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OIpct_DI_Short", parchedData[43].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OIpct_DI_Spreading", parchedData[44].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OIpct_AS_Long", parchedData[45].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OIpct_AS_Short", parchedData[46].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OIpct_As_Spreading", parchedData[47].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OIpct_LF_Long", parchedData[48].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OIpct_LF_Short", parchedData[49].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OIpct_LF_Spreading", parchedData[50].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OIpct_OT_Long", parchedData[51].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OIpct_OT_Short", parchedData[52].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OIpct_OT_Spreading", parchedData[53].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OIpct_NON_Long", parchedData[56].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@OIpct_NON_Short", parchedData[57].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@TotalTraders", parchedData[58].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@trds_DI_Long", parchedData[59].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@trds_DI_Short", parchedData[60].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@trds_DI_Spreading", parchedData[61].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@trds_AS_Long", parchedData[62].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@trds_AS_Short", parchedData[63].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@trds_As_Spreading", parchedData[64].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@trds_LF_Long", parchedData[65].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@trds_LF_Short", parchedData[66].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@trds_LF_Spreading", parchedData[67].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@trds_OT_Long", parchedData[68].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@trds_OT_Short", parchedData[69].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@trds_OT_Spreading", parchedData[70].ToString().Trim()));
                    cmd.Parameters.Add(new MySqlParameter("@LastUpdatedTime", parchedData[2].ToString().Trim()));

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

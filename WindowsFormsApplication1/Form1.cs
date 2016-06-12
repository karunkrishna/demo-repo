using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {


        private MySqlConnection dbconn = new MySqlConnection("server=localhost;database=algo;uid=root;password=Password1;");
        private List<CummOffsetData> records = new List<CummOffsetData>();
        public Form1()
        {

            dbconn.Open();
            string queryTradeTable = "select tradedate,high,low,open,close from  kDailyPNL_cumm;";

            using (MySqlCommand command = new MySqlCommand(queryTradeTable, dbconn))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                while (reader.Read())
                {
                    CummOffsetData sw = new CummOffsetData(reader.GetString(0),reader.GetDouble(1),
                        reader.GetDouble(2),reader.GetDouble(3),reader.GetDouble(4));
                    records.Add(sw);
                }
            }

            InitializeComponent();
            
            tabControl1.TabPages[0].Text = "Trade Summary";
            tabControl1.TabPages[1].Text = "Economic Data";
            button1.Text = "Fetch Data";
            //tabControl1.TabPages[1]


            CreateCandleStick();
            CreateTradeSummary();
            CreateEconData();

            dbconn.Close();
        }

        private void CreateCandleStick()
        {

            Series price = new Series("price"); // <<== make sure to name the series "price"
            Series.Series.Add(price);

            // Set series chart type
            Series.Series["price"].ChartType = SeriesChartType.Candlestick;

            // Set the style of the open-close marks
            //Series.Series["price"]["OpenCloseStyle"] = "Triangle";

            // Show both open and close marks
            //Series.Series["price"]["ShowOpenClose"] = "Both";

            // Set point width
            Series.Series["price"]["PointWidth"] = "0.5";

            // Set colors bars
            Series.Series["price"]["PriceUpColor"] = "White"; // <<== use text indexer for series
            Series.Series["price"]["PriceDownColor"] = "black"; // <<== use text indexer for series
            Series.Series["price"].Color = Color.Black;
            Series.Series["price"].BorderColor = Color.Black;
            Series.Series["price"].BorderWidth = 1;
            Series.Series["price"].IsVisibleInLegend = false;

            Series.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineWidth=1;
            Series.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineWidth = 1;
            Series.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            Series.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            Series.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            Series.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor =Color.Gray;
            Series.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.Gray;


            /*for (int i = 0; i < k.Count; i++)
            {
                // adding date and high
                Series.Series["price"].Points.AddXY(i, k[i].Hoog);
                // adding low
                Series.Series["price"].Points[i].YValues[1] = k[i].Laag;
                //adding open
                Series.Series["price"].Points[i].YValues[2] = k[i].PrijsOpen;
                // adding close
                Series.Series["price"].Points[i].YValues[3] = k[i].PrijsGesloten;
            }*/

            for (int i = 0; i < records.Count; i++)
            {
                // adding date and high
                Series.Series["price"].Points.AddXY(i, records[i].high);
                // adding low
                Series.Series["price"].Points[i].YValues[1] = records[i].low;
                //adding open
                Series.Series["price"].Points[i].YValues[2] = records[i].open;
                // adding close
                Series.Series["price"].Points[i].YValues[3] = records[i].close;
            }
        }
        private void CreateTradeSummary()
        {
            MySqlCommand cmd = new MySqlCommand("select tradedate, trades, wingross+lossgross, wintrades, wingross, wingross/wintrades, losstrades, lossgross, lossgross/losstrades from kDailyPNL_cumm", dbconn);
            using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
            {
                using (DataTable dt = new DataTable())
                {
                    sda.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }

            dataGridView1.RowHeadersVisible = false; 
            dataGridView1.AllowUserToAddRows = false;
            
            dataGridView1.Columns[0].Width = 85;
            dataGridView1.Columns[0].HeaderText = "Trade Date";
            //dataGridView1.Columns[0].DefaultCellStyle.Format = ("DD/MM/YYYY");


            dataGridView1.Columns[1].Width = 50;
            dataGridView1.Columns[1].HeaderText = "#";
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridView1.Columns[2].Width = 90;
            dataGridView1.Columns[2].HeaderText = "Daily PNL";
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[2].DefaultCellStyle.Format = "#,0.00";

            dataGridView1.Columns[3].Width = 50;
            dataGridView1.Columns[3].HeaderText = "# Win";
            dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridView1.Columns[4].Width = 90;
            dataGridView1.Columns[4].HeaderText = "Gross Win";
            dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[4].DefaultCellStyle.Format = "#,0.00";

            dataGridView1.Columns[5].Width = 90;
            dataGridView1.Columns[5].HeaderText = "Avg. Win";
            dataGridView1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[5].DefaultCellStyle.Format = "#,0.00";

            dataGridView1.Columns[6].Width = 50;
            dataGridView1.Columns[6].HeaderText = "# Loss";
            dataGridView1.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridView1.Columns[7].Width = 90;
            dataGridView1.Columns[7].HeaderText = "Gross Loss";
            dataGridView1.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[7].DefaultCellStyle.Format = "#,0.00";

            dataGridView1.Columns[8].Width = 90;
            dataGridView1.Columns[8].HeaderText = "Avg. Loss";
            dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[8].DefaultCellStyle.Format = "#,0.00";

           

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToDouble(row.Cells[3].Value) > 0)
                {
                    row.DefaultCellStyle.BackColor = Color.MediumSpringGreen;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
                }

            }

        }

        private void CreateEconData()
        {
            MySqlCommand cmd = new MySqlCommand("select estDateTime,currency,impact,eventItem from dstecondata where estDatetime > '2016-04-01' and currency = 'USD' and impact ='H'", dbconn);
            using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
            {
                using (DataTable dt = new DataTable())
                {
                    sda.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
            }


        }


        private void dataGridView_CellFormating(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (Convert.ToDouble(row.Cells[3].Value) > 0)
                {
                    row.DefaultCellStyle.BackColor = Color.MediumSpringGreen;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ForexFactoryCalendar fetchData = new ForexFactoryCalendar();
            textBox1.Text = fetchData.FetchXPATHData();

            
            //FetchFromForexFactory fetchData = new FetchFromForexFactory();
            //textBox1.Text = fetchData.FetchDataUsingHTMLAgility();
            //fetchData.FetchData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //SecotorHoldingDownloader sectorData = new SecotorHoldingDownloader();
            //yahooDownload.DownloadYahooHistoricalData();
            //sectorData.DownloadStateStreetHoldings();
           // sectorData.RecordHoldingsData();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
    class CummOffsetData
    {
        public string date;
        public double high;
        public double low;
        public double open;
        public double close;

        public CummOffsetData(string d, double h, double l, double o, double c) { date = d; high = h; low = l; open = o; close = c; }

    }
}

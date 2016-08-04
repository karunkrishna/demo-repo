using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestApp.DataCollection
{
    class Finviz
    {
        private string urlMinute = "http://finviz.com/futures_charts.ashx?t=INDICES&p=m5";
        private string urlHour = "http://finviz.com/futures_charts.ashx?t=INDICES&p=h1";
        private string urlDaily = "http://finviz.com/futures_charts.ashx?t=INDICES&p=d1";
        private string urlWeekly = "http://finviz.com/futures_charts.ashx?t=INDICES&p=w1";
        
        public Finviz()
        {
            //http://finviz.com/fut_chart.ashx?t=ES&p=m5&s=m&rev=636014517695569229
            //http://finviz.com/image.ashx?sp500&rev=636015036927043359
            //http://finviz.com/fut_chart.ashx?t=ES&p=h1&s=m&rev=636016299088310239
            //http://finviz.com/fut_chart.ashx?t=ES&p=d1&s=m&rev=636016299667189406
            //http://finviz.com/fut_chart.ashx?t=ES&p=w1&s=m&rev=636016300445995815
            //http://finviz.com/fut_chart.ashx?t=ES&p=m1&s=m&rev=636016301389905223

        }

        public void GetImages()
        {
            string urlImage = ".//*[@id='futures_charts']//table//tbody//tr//td//a//img";
        }
    }
}

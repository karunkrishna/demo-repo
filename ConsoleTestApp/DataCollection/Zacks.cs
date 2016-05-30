using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestApp.DataCollection
{
    class Zacks
    {
        public void GetEarnings()
        {
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

        }
    }
}

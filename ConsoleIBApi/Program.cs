using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBApi;

namespace ConsoleIBApi
{
    class Program
    {
        static void Main(string[] args)
        {

            #region TWSAPI Testing
            /*
            Console.WriteLine(" Press any key to call Realtime data code");
            Console.ReadKey();
            RealtimeData callRealTime = new RealtimeData();
            Console.ReadKey();
            */

            /*
            Console.WriteLine(" Press any key to call Historical data code");
            Console.ReadKey();
            HistoricalData callHistorical = new HistoricalData();
            Console.ReadKey();
            */

            /*
            Console.WriteLine(" Press any key to call Submit Order data code");
            Console.ReadKey();
            SubmitOrder callHistorical = new SubmitOrder();
            */
            #endregion

            var video = new video()
            {
                Title = "Video 1"
            };

            var videoEncover = new VideoEncoder();  //publisher demo
            var mailService = new Mailservice();  // Subscriber


            videoEncover.VideoEncoded += mailService.OnVideoEncoded;   //subscribing to event. 

            videoEncover.Encode(video);
            Console.ReadKey();
        }
    }
    public class Mailservice
    {
        //Keep in mind the event signiture that we aggreed above based on the delegate. 
        //We are now going to define the event Handler


        public void OnVideoEncoded(object source, EventArgs e)
        {
            Console.WriteLine("Sending mail...  ");
        }
    }
}

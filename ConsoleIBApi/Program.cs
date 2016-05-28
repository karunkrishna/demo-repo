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

            Console.WriteLine(" Press any key to call Submit Order data code");
            Console.ReadKey();
            SubmitOrder callHistorical = new SubmitOrder();
            Console.ReadKey();
        }
    }
}

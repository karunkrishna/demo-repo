using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBApi;

namespace ConsoleIBApi
{
    class RealtimeData
    {
        public RealtimeData()
        {
            Console.WriteLine("Start the TWS Api Connection");

            EWrapperImpl ibClient = new EWrapperImpl();
            ibClient.ClientSocket.eConnect("127.0.0.1", 7496, 1);

            Contract contract = new Contract()
            {
                Symbol = "IBM",
                SecType = "STK",
                Exchange = "SMART",
                Currency = "USD"
            };

            List<TagValue> mktDataOptions = new List<TagValue>();

            Console.WriteLine("QuoteRequest for made" + contract.Symbol);

            ibClient.ClientSocket.reqMktData(1, contract, "", false, mktDataOptions);
            Console.ReadKey(); // Key main thread active so that the reqMktData tread can remain open. 

            Console.WriteLine("QuoteCancel for " + contract.Symbol);

            ibClient.ClientSocket.cancelMktData(1);

            ibClient.ClientSocket.eDisconnect();

        }
    }
}

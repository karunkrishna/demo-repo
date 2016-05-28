using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBApi;

namespace ConsoleIBApi
{
    class HistoricalData
    {
        public HistoricalData()
        {
            string strEndDate = "20160528 16:00:00";
            string strDuration = "1 M";
            string strBarSize = "1 Day";
            string strWhatToShow = "TRADES";

            EWrapperImpl ibClient = new EWrapperImpl();
            ibClient.ClientSocket.eConnect("127.0.0.1", 7496, 1);

            Contract contract = new Contract()
            {
                Symbol = "IBM",
                SecType = "STK",
                Exchange = "SMART",
                Currency = "USD"
            };

            List<TagValue> historicalDataOptions = new List<TagValue>();

            ibClient.ClientSocket.reqHistoricalData(1, contract, strEndDate, strDuration, strBarSize, strWhatToShow, 1, 1, historicalDataOptions);
            

            Console.ReadKey();
            ibClient.ClientSocket.eDisconnect();
        }

    }
}

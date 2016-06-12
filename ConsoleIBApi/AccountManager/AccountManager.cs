using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBApi;

namespace ConsoleIBApi
{
    class AccountManager
    {
        private bool accountSummaryRequestActive = false;

        public AccountManager()
        {
            Console.WriteLine("Connecting to TWS Api to Fetch Account Details");

            EWrapperImpl ibClient = new EWrapperImpl();
            ibClient.ClientSocket.eConnect("127.0.0.1", 7496, 1);
            ibClient.ClientSocket.reqAccountUpdates(true,"U4193925");
           


        }
    }
}

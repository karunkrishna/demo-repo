using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBApi;


namespace ConsoleIBApi
{
    class SubmitOrder
    {
        public SubmitOrder()
        {
            int iOrderId = 3011;
            int giOrderId;

            EWrapperImpl ibClient = new EWrapperImpl();
            ibClient.ClientSocket.eConnect("127.0.01", 7496, 1);

            Contract contract = new Contract()
            {
                Symbol = "IBM",
                SecType = "STK",
                Exchange = "SMART",
                Currency = "USD"
            };

            /*
            Order orderInfo = new Order()
            {
                OrderId = iOrderId,
                Action = "BUY",
                OrderType = "LMT",
                TotalQuantity = 100,
                LmtPrice = 150.00,
                Tif = "DAY"
            };*/

            Order limitBuy = new Order()
            {
                OrderId = iOrderId+1,
                Action = "BUY",
                OrderType = "LMT",
                TotalQuantity = 100,
                LmtPrice = 150.00,
                Tif = "DAY"
            };

            Order limitSell = new Order()
            {
                OrderId = iOrderId+2,
                Action = "SELL",
                OrderType = "LMT",
                TotalQuantity = 100,
                LmtPrice = 160.00,
                Tif = "DAY"
            };

            Console.WriteLine("Ready to place order. Press a Key");
            Console.ReadKey();

            ibClient.ClientSocket.placeOrder(iOrderId + 1,contract, limitBuy);
            ibClient.ClientSocket.placeOrder(iOrderId + 2, contract, limitSell);

            Console.WriteLine(Environment.NewLine + "Cancel order by pressing any key...");
            Console.ReadKey();

            ibClient.ClientSocket.cancelOrder(iOrderId+1);
            ibClient.ClientSocket.cancelOrder(iOrderId + 2);

            ibClient.ClientSocket.eDisconnect();






        }
    }
}

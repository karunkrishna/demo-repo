
05/28/2016 12:10 PM
I am following this tutorial. To get a working understanding of event-base design and also implementing a software that can execute orders againts Interactive Brokres. Figures crossed, that I can make this work and this is worth the pain points

You can use the API User Guide form this location:
https://www.interactivebrokers.com/en/software/api/api.htm

TO DO: I need to do this section, because I think it will conceptually show me how to process the ticker data. However there is no realtime data right now, as its sunday. And I want to move on to the next section. 


http://holowczak.com/ib-api-socket-csharp-realtime/14/

I am following the tutorial from this location
http://holowczak.com/ib-api-socket-csharp-realtime/5/


Property		Description
conId 				Contract identifier for the financial instrument. Use a different contract ID for each data request. 
symbol 				Stock symbol or underlying symbol for Options or Futures
secType 			Type of instrument: Stock=STK, Option=OPT, Future=FUT, etc. 
expiry 				For Options or Futures: The expiration data in the format YYYYMM 
strike 				For Options: The Options Strike Price  
right 				For Options: The Options “PUT” or “CALL” rights 
multiplier			The contract multiplier for Futures or Options 
exchange  			The destination of order or request. “SMART” = IB smart order router 
primaryExchange		The primary listing exchange where the instrument trades. NYSE, NASDAQ, AMEX, BATS, ARCA, PHLX etc. 
currency			The currency of the exchange USD or GBP or CAD or EUR, etc. 
localSymbol 		The local exchange symbol of the instrument 
includeExpired 		Include expired futures or options contracts in the request 
secIdType 			The type of security identifier such as CUSIP, ISIN, SEDOL or RIC
secId 				The identifier of the type specified in secIdType. For example AAPL.O 

In each of the examples, before a data request is made or before an order is submitted, an object of class Contract will be created and its attributes will be populated with appropriate values used to identify the financial instrument. For example, to access market data for Microsoft stock, set the attributes:


conID = 0; 	// Contract ID
secType = "STK";	// Security type is a stock (STK) 
symbol = "MSFT";	// Microsoft stock symbol 
exchange = "SMART"; 	// Use IB’s Smart Order router to get the prices 
currency = "USD"; 	// USD Currency 
To access a June 2014 $35 Call option on Microsoft set the attributes:

conID = 0; 	// Contract ID
secType = "OPT"; 	// Security type is an Option (OPT)  
symbol = "MSFT"; 	// Google’s stock symbol 
expiry = "20140620"; 	// June 2014 Expiry 
strike = 35; 	// $35.00 strike price 
right = "CALL"; 	// Call option 
multiplier = "100"; 	// multiplier 100 shares per contract for options
exchange = "SMART"; 	// Use IB’s Smart Order router to get the prices 
currency = "USD"; 	// USD Currency 
To access a June 2017 Crude Oil Futures contract set the attributes:

conID = 0; 	// Contract ID
secType = "FUT"; 	// Security type is an Future (FUT)  
symbol = "CL"; 	// Crude Oil underlying symbol (CL) 
expiry = "201706"; 	// June 2017 Expiry 
exchange = "SMART"; 	// Use IB’s Smart Order router to get the prices 
To access a foreign exchange quote such as Euro/USD:

conID = 0; 	// Contract ID
secType = "CASH"; 	// Security type is Cash / FX 
symbol = "EUR"; 	// Euro underlying (base currency) symbol (EUR/USD quote) 
exchange = "IDEALPRO"; 	// Use the IDEALPRO FX data source  
currency = "USD"; 	// Quoted currency is USD  
Note that when reading currency quotes, consider the “Base” currency and the “quoted” currency.

http://holowczak.com/ib-api-socket-csharp-realtime/11/
The final bit of code to add will kick off the subscription to the security specified in the Contract object. The parameters for the reqMktData method are:

tickerId			A unique id to represent this request. Since the program only makes one request, the number 1 is used in this example.
contract			The contract object specifying the financial instrument. This was already specified previously in the program.
genericTickList		A string representing special tick values. There will be no custom tick values in this example so an empty string can be used.
snapshot			When snapshot is true the program will obtain only the latest price tick. When snapshot is false, the program will obtain a stream of market data until the program ends or the request is canceled.


http://holowczak.com/ib-api-socket-csharp-realtime/12/
Interactive Brokers sends back the tick prices and sizes in two different messages. In these implementations, the value of tickerId will match the same tickerId that was used in the call to reqMktData. For the tickPrice messages, the value of field will be one of:

1 = bid 
2 = ask
4 = last
6 = high
7 = low
9 = close
For the tickSize messages, the value of field will be one of:

0 = bid size
3 = ask size
5 = last size
8 = volume



05/28/2016 1:16 PM
Learning to extract historical data form IB TWS APi. 
There is an issue here as well. It looks like IB is stingy with their historical data. 
Also, looks like I don't have permission on my demo account. It looks like I keep hitting roadblocks. 
I wonder if its more important to deal with account information than anything else for IB connection. 
Well I'll continue to go throught the examples until we hit on all the use cases. 

Still hungry and hopeful. 

http://holowczak.com/ib-api-socket-csharp-historical/10/

Each data bar that is sent back in response to the historical data request triggers a call to the historicalData event handler. Note that the parameters of the method include:

reqId		The unique ID of the request
date		The date and time of the data bar
open		The opening price at the start of the data bar
high		The highest price during the data bar
low			The lowest price during the data bar
close		The last price reported at the end of the data bar
volume		The total volume of shares or contracts during the bar
count		The number of trades (or quotes) during the bar
WAP			The Volume weighted average price during the bar
hasGaps		If true, indicates that data may be missing from the bar


TO DO: This is important to do . 
http://holowczak.com/ib-api-socket-csharp-historical/14/

Exercise 1: Saving historical data to a file

Modify the historical data program to write the retrieved data to a local file in addition to displaying the data in the console.

Add the System.IO namespace at the top of the code with the “using” clause:
using System.IO;

In the historicalData method, declare a StreamWriter using File.AppendText and then write out the OutputString variable to the streamwriter. Do this right after the Console.WriteLine.
// Declare a stream writer to append data to a file
using (StreamWriter sw = File.AppendText("mydata.txt"))
// write the data to the file
sw.write(OutputString);
// Flush the output to disk
sw.Flush();

Note that the resulting file (mydata.txt) will be saved in the Debug folder within the project’s folder. For example, under the following folder:
\Projects\IB_History_Data_CS\IB_History_Data_CS\bin\Debug


05/28/2016 2:09 PM
This will be a great example, however, I fear that it will be fruitless if I can't get the demo data and realtime tick data sorted out. 

Talk to technical team and revert back. 
http://holowczak.com/ib-api-vcsharp-realtimebars/15/
http://holowczak.com/ib-api-vcsharp-market-depth/



05/28/2016 2:19 PM
Order Creating details.

Next we will need to create an Order object and then populate that Order object with the necessary parameters for the order. Interactive Brokers provides a very large range of parameters for Orders including the common Limit, Market and Stop Loss order types. A comprehensive treatment of all of the different order types and parameters is beyond the scope of this tutorial.

http://holowczak.com/ib-api-socket-csharp-console-submit-orders/11/

http://holowczak.com/ib-api-socket-csharp-console-submit-orders/14/

You need to handle the global orderId for orders placed via C#. But will that conflict with Ninjatrader and manually placing orders via TWS?

Exercise 1: Generate Unique Order Ids

Every order that is submitted must have a unique OrderID. The API provides a method call and event handler to generate a new, unique OrderID. To add this functionality:

Declare an integer named giOrderId
In the EWrapperImpl.cs file assign the most recent OrderId in the NextOrderId to giOrderId
When calling the placeOrder method from the main program use this global giOrderId

05/28/2016 2:57 PM
There are two items that I ignored. This will be for a later time as I still have alot of work to do in regards to algo trading itself. 

Both topics revolve around Options 
http://holowczak.com/ib-api-socket-csharp-console-contract-details/
http://holowczak.com/ib-api-socket-csharp-console-pricing-options/


05/28/2016 3:06 PM
Lets work on the Account details section now. 
I believe this will be a heavy topic to go-over as risk management relies heavily on this topic. 

CashBalance – account cash balance
DayTradesRemaining – number of day trades left
EquityWithLoanValue – equity with Loan Value
InitMarginReq – current initial margin requirement
MaintMarginReq – current maintenance margin
NetLiquidation – net liquidation value
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IBApi;
using System.Diagnostics;
using System.Net.Http;

namespace ConsoleIBApi
{
    class Program
    {
        [ThreadStatic]
        public static int _field;  // You need to add the threadstatic attribute to mark the variable


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

            #region Tutorials

            #region EventHandling Testing
            //var video = new video()
            //{
            //    Title = "Video 1"

            //};
            //var videoEncover = new VideoEncoder();  //publisher demo
            //var mailService = new Mailservice();  // Subscriber


            //videoEncover.VideoEncoded += mailService.OnVideoEncoded;   //subscribing to event. 

            //videoEncover.Encode(video);
            //Console.ReadKey();
            #endregion

            #region Multithreading VoidMethod (Threadstart)
            /*
            
            Thread t = new Thread(new ThreadStart(ThreadMethod));
            t.IsBackground = true;
            //The thread we created where is a foreground thread. 
            //To se it to backgorund thread you need to mark it as background. 
            //But if you main thread finishes, it might not output the thread values because it hand't 
            // time to execute. 

            t.Start();

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("Main thread: Do some work");
                Thread.Sleep(0);

            }

           // t.Join(); //Specifized not to close the app once it gets to the end of Main() class. And to continue
                // The ThreadMethod until it too is completed. 

            //This is what I needed in my earlier program to execute the ErrorHandling ourside the main class It think. 
            //t.join waits for the thread that we created to finish before it continues forward. So I guess we can use it
            //required datea before we contineu forward. 

            // The t.join comes in handy when dealing with isBackground threads. To make sure it finishes running before
            // the main app closes. 



            Console.WriteLine("AFTER");

    */
            #endregion

            #region Mulithreading ParameterizedTreadStart()

            //Thread t = new Thread(new ParameterizedThreadStart(ParameterizedThreadMethod));
            //t.Start(15); // This is where we pass on our parameter. As you see it has an overload. 
            //t.Join();

            #endregion

            #region Mulithreading stopping a tread with Lamda expression

            //bool stopped = false;

            //Thread t = new Thread(new ThreadStart(() =>
            //    {
            //        while (!stopped)
            //        {
            //            Console.WriteLine("Running....");
            //            Thread.Sleep(1000);

            //        }
            //    }));

            //t.Start();

            //Console.WriteLine("Press any key to stop thread...");
            //Console.ReadKey();
            //stopped = true;

            #endregion

            #region Multithreading using Static Variables

            //Thread t1 = new Thread(new ThreadStart(() =>
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        _field++;
            //        Console.WriteLine("Thread A: {0}", _field);

            //    }
            //}));
            //t1.Start();
            //Thread t2 = new Thread(new ThreadStart(() =>
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        _field++;
            //        Console.WriteLine("Thread B: {0}", _field);


            //    }
            //}));
            //t2.Start();


            #endregion

            #region Multithreading Queing work to the thead pool

            ////The problem with using the Threadpool class is that we can't manage our class well. Why?
            ////Because we can't tell when the tread has finished on when the tread has returned a value. 

            //ThreadPool.QueueUserWorkItem((s) =>
            //{
            //    Console.WriteLine("Working on a thread from the threadpool");

            //});



            //Console.WriteLine();



            #endregion

            #region Multithreading starting a task example of using aciton. 


            //Task t = Task.Run(action: ThreadMethodTask);
            //t.Wait();

            #endregion

            #region Multithreading starting a task  that returns a generic type and continuation

            //Task<int> t = Task.Run(() =>
            //{
            //    return 42;
            //}).ContinueWith((i) =>
            //{
            //    return i.Result*2;
            //});

            //t = t.ContinueWith((i) =>   // You have to reassign the task to the task again to call it if you are
            ////gonna call it again. If you don't it wont run. 
            //{
            //    return i.Result*2;
            //});
            //Console.WriteLine(t.Result);

            #endregion

            #region Multithreading starting a task  that returns a generic type and continuation OnlyOnFault

            //Task<int> t = Task.Run(() =>
            //{
            //    //throw new Exception();
            //    return 42;
            //});

            //t.ContinueWith((i) =>
            //{
            //    Console.WriteLine("Faulted");
            //},TaskContinuationOptions.OnlyOnFaulted);

            //t.ContinueWith((i) =>
            //{
            //    Console.WriteLine("Completed");
            //}, TaskContinuationOptions.OnlyOnRanToCompletion);

            //Console.WriteLine(t.Result);

            #endregion

            #region Using Task.WaitAll

            // Task[] tasks = new Task[3];

            // tasks[0] = Task.Run(() =>
            // {
            //     Thread.Sleep(1000);
            //     Console.WriteLine("1");
            //     return 1;
            // });
            // tasks[1] = Task.Run(() =>
            // {
            //     Thread.Sleep(2000);
            //     Console.WriteLine("2");
            //     return 2;

            // });
            // tasks[2] = Task.Run(() =>
            // {
            //     Thread.Sleep(1000);
            //     Console.WriteLine("3");
            //     return 3;

            // });

            //// tasks[0].Wait(); //Wait for task 1 to run. 
            // Task.WaitAll(tasks);



            #endregion

            #region Using Task.waitAny and a code bloack for removing array item. 
            //    //When you have an array of task and it continunes if any one of the task completes. 
            //    Task<int>[] tasks = new Task<int>[3];
            //tasks[0] = Task.Run(() =>
            //{
            //    Thread.Sleep(2000);
            //    return 1;
            //});
            //tasks[1] = Task.Run(() =>
            //{
            //    Thread.Sleep(1000);
            //    return 2;
            //});
            //tasks[2] = Task.Run(() =>
            //{
            //    Thread.Sleep(3000);
            //    return 3;
            //});

            //while (tasks.Length > 0)
            //{
            //    int i = Task.WaitAny(tasks);
            //    Task<int> completedTask = tasks[i];
            //    Console.WriteLine( "removed "+ completedTask.Result);
            //    var temp = tasks.ToList();  // This block of code allows of reasy removable of array. 
            //    temp.RemoveAt(i);
            //    tasks = temp.ToArray();
            //}



            #endregion

            #region Using Paralle.For and Paralle.Foreach

            //Stopwatch test = new Stopwatch();
            //test.Start();

            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine(i);
            //    Thread.Sleep(1000);

            //}
            //test.Stop();
            //Console.WriteLine(test.Elapsed);
            //#endregion

            ////You just can't predict the order check it. 
            //Stopwatch test2 = new Stopwatch();
            //test2.Start();
            //Parallel.For(0, 10, (i) =>
            //{
            //    Console.WriteLine(i);
            //    Thread.Sleep(1000);
            //});
            //test2.Stop();
            //Console.WriteLine(test2.Elapsed);


            //int[] myArray = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            //Stopwatch test3 = new Stopwatch();
            //test3.Start();
            //foreach (var i in myArray)
            //{
            //    Console.WriteLine(i);
            //    Thread.Sleep(1000);

            //}
            //test3.Stop();
            //Console.WriteLine(test3.Elapsed);


            //Stopwatch test4 = new Stopwatch();
            //test4.Start();
            //Parallel.ForEach(myArray, (i) =>
            //{
            //    Console.WriteLine(i);
            //    Thread.Sleep(1000);

            //});

            //test4.Stop();
            //Console.WriteLine(test4.Elapsed);
            #endregion

            #region Using Async and await code

            //string result = DownloadContent().Result;
            //Console.WriteLine(result);

            #endregion

            #endregion



            Console.ReadKey();
        }

        public static async Task<string> DownloadContent()
        {
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("http://www.microsoft.com");
                return result;
            }
        }

        public static void ThreadMethodTask()
        {
            for (int i = 0; i < 100; i++)
            {
                Console.Write('*');
            }

        }

        public static void ThreadMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("ThreadProcess {0}",i);
                Thread.Sleep(1000);
            }
        }

        public static void ParameterizedThreadMethod(object o)
        {
            for (int i = 0; i < (int)o; i++)
            {
                Console.WriteLine("ThreadProc: {0}",i);

            }
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

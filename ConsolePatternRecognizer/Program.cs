using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsolePatternRecognizer
{

    class Program
    {
        static List<double> tickList = new List<double>();
        static Queue<double> tickQueue = new Queue<double>();
         

        public EventHandler<EventArgs> SampleCompleted;

        static void Main(string[] args)
        {
            SamplePattern runSamplePattern = new SamplePattern();
            TickData runPublisher = new TickData();

            runPublisher.TickUpdated += OnTickUpdated;

            Thread runTickThread = new Thread(runPublisher.PublishTickData);
            runTickThread.Start();

            Console.ReadKey();
        }

        private  static void OnTickUpdated(object source, TickEventArgs args)
        {
           Console.WriteLine(args.Close);

            tickQueue.Enqueue(args.Close);

            if (tickQueue.Count >= 10)
            {
                Console.WriteLine("We hit sample size");


                Thread runThread = new Thread(() =>
                {
                    foreach (var data in tickList)
                    {
                        Console.WriteLine("Sample set, do some work: "  + data);
                    }

                });

                runThread.Start();
                if (runThread.ThreadState == ThreadState.Stopped)
                {
                    tickList.RemoveRange(0, 10);

                }

            }

        }
        #region Creating an event when sampleSize hits 10
        //protected virtual void OnSampleCompleted(EventArgs e)
        //{
        //    if (SampleCompleted != null)
        //        SampleCompleted(this,e);
        //}
        #endregion 
    }
    class Data
    {
        public double Close { get; set; }
    }
}

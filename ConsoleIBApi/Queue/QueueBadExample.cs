using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleIBApi
{

    internal class QueueBadExample
    {
        private static Queue<int> numbers = new Queue<int>();
        private static Random rand = new Random();
        private const int NumThreads = 3;
        private static int[] sums = new int[NumThreads];

        public QueueBadExample()
        {
            Stopwatch runTimer = new Stopwatch();
            runTimer.Start();
            
            var producdingThread = new Thread(ProduceNumbers);
            producdingThread.Start();

            Thread[] threads = new Thread[NumThreads];

            for (int i = 0; i < NumThreads; i++)
            {
                threads[i] = new Thread(SumNumbers);
                threads[i].Start(i);
            }
            for (int i = 0; i < NumThreads; i++)
                threads[i].Join();
            int totalSum = 0;
            for (int i = 0; i < NumThreads; i++)
            {
                totalSum += sums[i];

            }
            Console.WriteLine("Done adding. Total is {0}",totalSum);
            runTimer.Stop();
            Console.WriteLine(runTimer.Elapsed);
            
            //this.ProduceNumbers();
            //this.SumNumbers();
        }
        private  void ProduceNumbers()
        {
            for (int i = 0; i < 10; i++)
            {
                int numToEnqueue = rand.Next(10);
                lock (numbers)
                    numbers.Enqueue(numToEnqueue);

                Thread.Sleep(rand.Next(1000));
                Console.WriteLine("Producing thread adding {0} to the queue.", numToEnqueue);
            }
        }

        private  void SumNumbers(object threadNumber)
        {
            DateTime startTime = DateTime.Now;
            int mySum = 0;
            while ((DateTime.Now - startTime).Seconds < 10)
            {
                lock (numbers)
                {
                    if (numbers.Count != 0)
                    {
                        int numtoSum = numbers.Dequeue();
                        mySum += numtoSum;
                        Console.WriteLine("Consuming thread adding {0} to its toal sum making {1} " +
                                          "for the thread total.", numtoSum, mySum);

                    }
                }
            }
            sums[(int) threadNumber] = mySum;
        }
    }
}

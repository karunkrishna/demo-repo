using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePatternRecognizer
{
    class SamplePattern
    {
        public SamplePattern()
        {
            Console.WriteLine("Pattern Recognize program has started");
            TickData tickData = new TickData();
            tickData.TickUpdated += OnTickUpdated;
        }   //TO DO: Create a second thread that will listen to the tick data event to process...right now I don't think this program is active to listen to the event. 

        private void OnTickUpdated(object source, EventArgs args)
        {
            Console.WriteLine("Hello motherfucker you did it");
        }
    }
}

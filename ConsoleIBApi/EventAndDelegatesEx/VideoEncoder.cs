using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleIBApi
{
    class VideoEncoder
    {
        /// <summary>
        ///  What we are going to do is publish an event when the encoder finishes it's job...
        ///  represented by the Sleep Thread
        ///  https://www.youtube.com/watch?v=jQgwEsJISy0
        ///  
        /// There are three things we need to do:
        /// 1- Define a delegate
        /// 2- Define an event based on that delegate
        /// 3 - Raise the event
        /// 
        /// The convention for the parameters of the event is 
        /// object 'source' the class that this sending/publish the event
        /// 
        /// EventArgs is any addition 'data' we want to send in the event. 
        /// Also the method name itself has a convention for example use the Classname appened with EventHandler
        /// Also notice the tense of the event defintion VideoEncoded vs VideoEncoding
        /// 
        /// Convention is to use Protected virtual void...and start with On"""
        /// 
        /// Take a look at the VideoEventArgs for passing on additional information from publisher to the subscriber. 
        /// </summary>
        /// <param name="video"></param>
        /// 

        
        public class VideoEventArgs : EventArgs
        {
            public video Video { get; set; }
        }
        public delegate void VideoEncodedEventHandler(object source, EventArgs args); // Step 1
        public event VideoEncodedEventHandler VideoEncoded;  // Step2

        public void Encode(video video)
        {
            Console.WriteLine("Encoding video...");
            Thread.Sleep(3000);
            Console.WriteLine("Fin.");

            OnVideoEncoded();  // Calling the notifier (publishing method)
        }

        protected virtual void OnVideoEncoded() // Step3
        {
            //the responsibility for notific the subscriber is coded here.
            if (VideoEncoded != null)  // Someone has to subsribe to it...take a look at the progra.cs to see how to subscribe to event
                VideoEncoded(this, EventArgs.Empty); //if you don't want to send any event args. 

        }

        

    }
}

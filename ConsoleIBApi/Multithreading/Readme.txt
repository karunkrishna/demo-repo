There are three ways to intialize a tread. 

The first is to use the thread class. 

You have to declare delegats to start a thread
1. ThreadStart (Which works on void method)
2. ParametereizedThreadStart (When you want to pass in objects with the thread runs to kep it move it on.)
	It takes in a parameter of object. and we need to use some casting to type to use our object. 

Now its important to learn how to stop a thread before it finishes. 
	He uses lanmda expression in this case....to quikcly create a method within the call itself. 
	That is pretty cool. https://youtu.be/RhLLxew4-TY?t=1502


When you call a thread, it is local. Which means that any variable made in that thread, will only be availave in that threa.d 
Any thread that access a global variable, will access that global variable and any thread that accesses that global variable will be acting 
on data that might be acted on this global variable that might not be your desired outcome. 

But if you mark those global variables as staict any method that access that [static] global variable will have 
its on copy of the variable. 

When a thread is run, it is destoryed when it completes. 
However, there is a way that we can recycle them. And that is thread pooling. 

Threadpool (Dont use, ecause you cant manage it well)
You can use ThreadStart which is unmanaged. But this means you can completely crasht he sever if someone kicks up 
too many threads. A thread pool limits this, and is a bit more clean in terms of managing threads and memory. 
Threadpools you can not monitor to see when a thread has completed or when a thread has returned a value. 


Task (is a managed threadpool )
 You can return value form the method using.Result
 You can use continuation. It is when you can continue to do a [new] task once the prior tax completes. 


 Parrallel.For and Parallel.Foreach
 Itteration of the For and foreach don't need to run sequentially. 
 It could increase the completion of the loop quicker. 


 There is another concept of writting async code. 

 This is a great website 
 http://www.albahari.com/threading/

 When you use locker to prevent a thread from overright a variable used by other threads. 
 It is called thread-safe. 

 Shared data is the primary cause of complexity and obscure errors in multithreading. Although often essential, it pays to keep it as simple as possible.

 Thread.Sleep(0) relinquishes the thread’s current time slice immediately, voluntarily handing over the CPU to other threads. Framework 4.0’s new Thread.Yield() method does the same thing — except that it relinquishes only to threads running on the same processor.

Sleep(0) or Yield is occasionally useful in production code for advanced performance tweaks. It’s also an excellent diagnostic tool for helping to uncover thread safety issues: if inserting Thread.Yield() anywhere in your code makes or breaks the program, you almost certainly have a bug.

Threading’s Uses and Misuses
Multithreading has many uses; here are the most common:

Maintaining a responsive user interface
By running time-consuming tasks on a parallel “worker” thread, the main UI thread is free to continue processing keyboard and mouse events.
Making efficient use of an otherwise blocked CPU
Multithreading is useful when a thread is awaiting a response from another computer or piece of hardware. While one thread is blocked while performing the task, other threads can take advantage of the otherwise unburdened computer.
Parallel programming
Code that performs intensive calculations can execute faster on multicore or multiprocessor computers if the workload is shared among multiple threads in a “divide-and-conquer” strategy (see Part 5).
Speculative execution
On multicore machines, you can sometimes improve performance by predicting something that might need to be done, and then doing it ahead of time. LINQPad uses this technique to speed up the creation of new queries. A variation is to run a number of different algorithms in parallel that all solve the same task. Whichever one finishes first “wins” — this is effective when you can’t know ahead of time which algorithm will execute fastest.
Allowing requests to be processed simultaneously
On a server, client requests can arrive concurrently and so need to be handled in parallel (the .NET Framework creates threads for this automatically if you use ASP.NET, WCF, Web Services, or Remoting). This can also be useful on a client (e.g., handling peer-to-peer networking — or even multiple requests from the user).
With technologies such as ASP.NET and WCF, you may be unaware that multithreading is even taking place — unless you access shared data (perhaps via static fields) without appropriate locking, running afoul of thread safety.

Threads also come with strings attached. The biggest is that multithreading can increase complexity. Having lots of threads does not in and of itself create much complexity; it’s the interaction between threads (typically via shared data) that does. This applies whether or not the interaction is intentional, and can cause long development cycles and an ongoing susceptibility to intermittent and nonreproducible bugs. For this reason, it pays to keep interaction to a minimum, and to stick to simple and proven designs wherever possible. This article focuses largely on dealing with just these complexities; remove the interaction and there’s much less to say!

A good strategy is to encapsulate multithreading logic into reusable classes that can be independently examined and tested. The Framework itself offers many higher-level threading constructs, which we cover later. [Using fitnesse model to automate component testing]
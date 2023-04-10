/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            const int initialState = 10;

            OptionA();
            OptionB();

            Console.ReadLine();

            void OptionA()
            {
                Thread rootThread = ThreadWithState.StartThread(initialState);
                Console.WriteLine("Option A: start");
                rootThread.Join();
                Console.WriteLine("Option A: finish\n");
            }

            void OptionB()
            {
                // this implementation uses separate semaphores for each thread to support such recursive waiting behavior (just like using Thread.Join()):
                // start (1) -> start (2) -> ... -> start (n) -> finish (n) -> finish (n-1) -> ... -> finish (1)
                // where (n) is thread id
                SemaphoreThread rootThread = SemaphoreThread.StartThread(initialState);
                Console.WriteLine("Option B: start");
                rootThread.Wait();
                Console.WriteLine("Option B: finish\n");

                // this implementation uses common static Semaphore for all threads with the following flow:
                // start (1) -> finish (1) -> start (2) -> finish (2) -> ... -> start (n) -> finish (n)
                Console.WriteLine("Option B: simplified");
                SimpleSemaphoreThread.StartThread(initialState);
            }
        }
    }

    class ThreadWithState
    {
        private int _state;

        private ThreadWithState(int state)
        {
            _state = state;
        }

        public void RunRecursive()
        {
            int newState = Interlocked.Decrement(ref _state);
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"Thread {currentThreadId} with state {newState} started.");

            if (newState != 0)
            {
                Thread childThread = StartThread(newState);
                childThread.Join();
            }

            Console.WriteLine($"Thread {currentThreadId} finished.");
        }

        public static Thread StartThread(int state)
        {
            ThreadWithState threadWithState = new ThreadWithState(state);
            Thread thread = new Thread(new ThreadStart(threadWithState.RunRecursive));
            thread.Start();
            return thread;
        }
    }

    public class SemaphoreThread
    {
        private int _state;
        private readonly Semaphore _semaphore;

        public SemaphoreThread(int state)
        {
            _state = state;
            _semaphore = new Semaphore(0, 1);
        }

        public void RunRecursive()
        {
            int newState = Interlocked.Decrement(ref _state);
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;

            Console.WriteLine($"Thread {currentThreadId} with state {newState} started.");

            if (newState != 0)
            {
                SemaphoreThread childThread = StartThread(newState);
                childThread.Wait();
            }

            Console.WriteLine($"Thread {currentThreadId} finished.");
            _semaphore.Release();
        }

        public void Wait()
        {
            _semaphore.WaitOne();
        }

        public static SemaphoreThread StartThread(int state)
        {
            SemaphoreThread threadWithSemaphor = new SemaphoreThread(state);
            ThreadPool.QueueUserWorkItem(_ => threadWithSemaphor.RunRecursive());
            return threadWithSemaphor;
        }
    }

    public class SimpleSemaphoreThread
    {
        public readonly static Semaphore Semaphore;

        private int _state;

        public SimpleSemaphoreThread(int state)
        {
            _state = state;
        }

        static SimpleSemaphoreThread()
        {
            Semaphore = new Semaphore(1, 1);
        }

        public void RunRecursive()
        {
            Semaphore.WaitOne();
            int newState = Interlocked.Decrement(ref _state);
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;

            Console.WriteLine($"Thread {currentThreadId} with state {newState} started.");

            if (newState != 0)
            {
                StartThread(newState);
            }

            Console.WriteLine($"Thread {currentThreadId} finished.");
            Semaphore.Release();
        }

        public static SimpleSemaphoreThread StartThread(int state)
        {
            SimpleSemaphoreThread threadWithSemaphor = new SimpleSemaphoreThread(state);
            ThreadPool.QueueUserWorkItem(_ => threadWithSemaphor.RunRecursive());
            return threadWithSemaphor;
        }
    }
}

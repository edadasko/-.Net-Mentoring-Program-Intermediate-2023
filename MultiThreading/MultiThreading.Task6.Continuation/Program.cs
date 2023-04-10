/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            Task mainTask = new Task(() =>
            {
                Console.WriteLine($"Main task {CurrentThreadId()}");

                Console.WriteLine("Press 'e' to throw Exception");
                Console.WriteLine("      'c' to cancel task");
                Console.WriteLine("or any other key for success");

                char inputChar = Console.ReadKey().KeyChar;
                Console.WriteLine();

                switch (inputChar)
                {
                    case 'e': throw new Exception();
                    case 'c':
                        cancellationTokenSource.Cancel();
                        break;
                };

                cancellationToken.ThrowIfCancellationRequested();

            }, cancellationToken);

            mainTask.ContinueWith(_ =>
            {
                Console.WriteLine(
                    $"a) It happens regardless of the result of the parent task\n" +
                    $"{CurrentThreadId()}");

            }, TaskContinuationOptions.None);

            mainTask.ContinueWith(_ =>
            {
                Console.WriteLine(
                    $"b) It happens when the parent task finished without success\n" +
                    $"{CurrentThreadId()}");

            }, TaskContinuationOptions.NotOnRanToCompletion);

            mainTask.ContinueWith(_ =>
            {
                Console.WriteLine(
                    $"c) Parent thread is reused for this continuation on fail.\n" +
                    $"{CurrentThreadId()}");

            }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            mainTask.ContinueWith(_ =>
            {
                Console.WriteLine(
                    $"d) It happens on cancellation and executed outside of the thread pool.\n" +
                    $"{CurrentThreadId()}" +
                    $"Executed outside the thread pool: {!Thread.CurrentThread.IsThreadPoolThread}\n");

            }, TaskContinuationOptions.LongRunning | TaskContinuationOptions.OnlyOnCanceled);

            mainTask.Start();

            try
            {
                mainTask.Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();

            string CurrentThreadId() => $"Current thread id: {Thread.CurrentThread.ManagedThreadId}\n";
        }
    }
}

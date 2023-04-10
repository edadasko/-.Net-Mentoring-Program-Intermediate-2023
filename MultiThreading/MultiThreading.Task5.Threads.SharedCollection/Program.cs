/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            const int timeout = 1000;

            ConcurrentStack<int> stack = new ConcurrentStack<int>();

            AutoResetEvent producerEvent = new AutoResetEvent(false);
            AutoResetEvent consumerEvent = new AutoResetEvent(false);

            Task producer = new Task(Adding);
            Task consumer = new Task(Printing);

            producer.Start();
            consumer.Start();

            Console.ReadLine();

            void Adding()
            {
                const int elementsCount = 10;
                for (int i = 0; i < elementsCount; i++)
                {
                    stack.Push(i);
                    producerEvent.Set();

                    if (!consumerEvent.WaitOne(timeout))
                    {
                        break;
                    }
                }
            }

            void Printing()
            {
                while (true)
                {
                    if (!producerEvent.WaitOne(timeout))
                    {
                        break;
                    }

                    Console.WriteLine(string.Join(" ", stack));
                    consumerEvent.Set();
                }
            }
        }
    }
}

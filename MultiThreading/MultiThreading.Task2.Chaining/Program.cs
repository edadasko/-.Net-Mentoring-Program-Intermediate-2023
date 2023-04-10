/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Task chainOfTasks = CreateChainOfFourTasks();
            chainOfTasks.Start();

            Console.ReadLine();
        }

        static Task CreateChainOfFourTasks()
        {
            const int arraySize = 10;
            const int minValue = 0;
            const int maxValue = 100;

            Task<int[]> task1 = new Task<int[]>(() =>
                DoOperationWithPrint(
                    "First Task, Random array: ",
                    () => CreateRandomArray(arraySize),
                    PrintArray));

            Task<int[]> task2 = task1.ContinueWith(task =>
                DoOperationWithPrint(
                    "Second Task, array multiplied by random integer: ",
                    () => MultiplyArray(task.Result),
                    PrintArray));

            Task<int[]> task3 = task2.ContinueWith(task =>
                DoOperationWithPrint(
                    "Third Task, sorted array in ASC order: ",
                    () => SortArray(task.Result), 
                    PrintArray));

            Task<double> task4 = task3.ContinueWith(task =>
                DoOperationWithPrint(
                    "Fourth Task, the average value: ", 
                    () => Enumerable.Average(task.Result),
                    Console.WriteLine));

            return task1;

            int[] CreateRandomArray(int size)
            {
                Random random = new Random();

                return Enumerable.Range(0, size)
                    .Select(_ => random.Next(minValue, maxValue))
                    .ToArray();
            }

            int[] MultiplyArray(int[] array)
            {
                int randomInt = new Random().Next(minValue, maxValue);
                return array.Select(x => x * randomInt).ToArray();
            }

            int[] SortArray(int[] array)
            {
                Array.Sort(array);
                return array;
            }
        }

        static TOutput DoOperationWithPrint<TOutput>(string capture, Func<TOutput> operation, Action<TOutput> printAction)
        {     
            Console.WriteLine(capture);
            TOutput result = operation();
            printAction(result);
            return result;
        }

        static void PrintArray<T>(T[] array) =>
            Console.WriteLine(string.Join(" ", array));
    }
}

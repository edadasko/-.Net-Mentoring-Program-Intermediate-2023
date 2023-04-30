using Messaging.Core;
using Messaging.Core.Interfaces;
using Messaging.ProcessingService.Interfaces;

namespace Messaging.ProcessingService
{
    internal class Program
    {
        private const string FolderPath = "ResultFolder";

        static void Main(string[] args)
        {
            IMessageProcessor messageProcessor = new MessageProcessor(FolderPath);

            messageProcessor.PartProcessingStarted += (_, message) =>
                Console.WriteLine($"Processing of {message.PartNum}/{message.PartsCount} part of {message.Name} started.");

            messageProcessor.PartProcessingCompleted += (_, message) =>
                Console.WriteLine($"Processing of {message.PartNum}/{message.PartsCount} part of {message.Name} completed.");

            messageProcessor.FileProcessingCompleted += (_, message) =>
                Console.WriteLine($"Processing of {message.Name} completed.");

            IMessageConsumer consumer = new RabbitMqMessageConsumer(messageProcessor.ProcessMessageAsync);

            EventWaitHandle waitHandle = new AutoResetEvent(false);
            Task.Run(() => consumer.Listen(waitHandle));

            Console.ReadLine();
            waitHandle.Set();
        }
    }
}
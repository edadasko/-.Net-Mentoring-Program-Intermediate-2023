using Messaging.DataCaptureService.Interfaces;

namespace Messaging.DataCaptureService
{
    internal class Program
    {
        private const string FolderPath = "InitialFolder";

        static void Main(string[] args)
        {
            IFolderListener folderListener = new FolderListener(FolderPath);

            folderListener.FileIsAdded += (_, fileName) =>
                Console.WriteLine($"{fileName} is added to the folder.");

            folderListener.PartIsSended += (_, message) =>
                Console.WriteLine($"{message.PartNum}/{message.PartsCount} part of {message.Name} is sended.");

            folderListener.FileIsSended += (_, fileName) =>
                Console.WriteLine($"{fileName} is completely sended.");

            EventWaitHandle waitHandle = new AutoResetEvent(false);
            Task.Run(() => folderListener.Run(waitHandle));

            Console.ReadLine();
            waitHandle.Set();
        }
    }
}
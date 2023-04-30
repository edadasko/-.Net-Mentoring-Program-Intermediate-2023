using Messaging.Common.Models;
using Messaging.Common.Serializators;
using Messaging.DataCaptureService.Interfaces;
using Messaging.Producers;

namespace Messaging.DataCaptureService
{
    public class FolderListener : IFolderListener
    {
        private static readonly IMessageProducer _messageProducer = new RabbitMqMessageProducer();
        private static readonly IFileMessageBuilder _fileMessageFormatter = new FileMessageBuilder();
        private static readonly IBinarySerializator<FileMessage> _binarySerializator = new BinarySerializator<FileMessage>();

        private readonly string _folderPath;

        public event EventHandler<string> FileIsAdded;
        public event EventHandler<FileMessage> PartIsSended;
        public event EventHandler<string> FileIsSended;

        public FolderListener(string folderPath)
        {
            _folderPath = folderPath;
            Directory.CreateDirectory(_folderPath);
        }

        public void Run(EventWaitHandle waitingHandle)
        {
            using var watcher = new FileSystemWatcher(_folderPath);

            watcher.EnableRaisingEvents = true;

            watcher.Created += async (s, e) => await HandleCreatedFileAsync(e);

            waitingHandle.WaitOne();
        }

        private async Task HandleCreatedFileAsync(FileSystemEventArgs fileSystemEventArgs)
        {
            while (IsFileLocked(fileSystemEventArgs.FullPath))
            {
                await Task.Delay(1000);
            }

            using FileStream fileStream = new FileStream(fileSystemEventArgs.FullPath, FileMode.Open);
            FileIsAdded?.Invoke(this, fileStream.Name);

            await foreach (var message in _fileMessageFormatter.GenerateMessagesAsync(fileStream))
            {
                byte[] bytes = _binarySerializator.Serialize(message);
                _messageProducer.SendMessage(bytes);
                PartIsSended?.Invoke(this, message);
            }

            FileIsSended?.Invoke(this, fileStream.Name);
        }

        private static bool IsFileLocked(string path)
        {
            FileStream stream = null;

            try
            {
                stream = File.Open(path, FileMode.Open);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                stream?.Close();
            }

            return false;
        }
    }
}

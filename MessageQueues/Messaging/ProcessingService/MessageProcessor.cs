using Messaging.Common.Models;
using Messaging.Common.Serializators;
using Messaging.ProcessingService.Interfaces;

namespace Messaging.ProcessingService
{
    public class MessageProcessor : IMessageProcessor
    {
        private readonly string _saveFolder;
        private static readonly IBinarySerializator<FileMessage> binaryFormatter = new BinarySerializator<FileMessage>();
        private static readonly Dictionary<string, ResultFile> _files = new Dictionary<string, ResultFile>();

        public event EventHandler<FileMessage> PartProcessingStarted;
        public event EventHandler<FileMessage> PartProcessingCompleted;
        public event EventHandler<FileMessage> FileProcessingCompleted;

        public MessageProcessor(string saveFolder)
        {
            _saveFolder = saveFolder;
            Directory.CreateDirectory(_saveFolder);
        }

        public async Task ProcessMessageAsync(byte[] message)
        {
            FileMessage fileMessage = binaryFormatter.Deserialize(message);

            PartProcessingStarted?.Invoke(this, fileMessage);

            await WriteMessageAsync(fileMessage);

            PartProcessingCompleted?.Invoke(this, fileMessage);

            if (fileMessage.PartNum == fileMessage.PartsCount)
            {
                _files[fileMessage.Name].FileStream.Flush();
                _files[fileMessage.Name].FileStream.Close();

                FileProcessingCompleted?.Invoke(this, fileMessage);
            }
        }

        private async Task WriteMessageAsync(FileMessage fileMessage)
        {
            if (fileMessage.PartNum == 1)
            {
                var processedFileName = $"{Guid.NewGuid()}.{Path.GetExtension(fileMessage.Name)}";
                _files[fileMessage.Name] = new ResultFile()
                {
                    LastProcessedPart = fileMessage.PartNum,
                    FileStream = File.Create(Path.Combine(_saveFolder, processedFileName)),
                    SemaphoreSlim = new SemaphoreSlim(1),
                };

                _files[fileMessage.Name].SemaphoreSlim.Wait();
                await _files[fileMessage.Name].FileStream.WriteAsync(fileMessage.Data);
                _files[fileMessage.Name].SemaphoreSlim.Release();
            }
            else
            {
                _files[fileMessage.Name].SemaphoreSlim.Wait();

                _files[fileMessage.Name].LastProcessedPart = fileMessage.PartNum;
                await _files[fileMessage.Name].FileStream.WriteAsync(fileMessage.Data);

                _files[fileMessage.Name].SemaphoreSlim.Release();
            }
        }

        private class ResultFile
        {
            public long LastProcessedPart { get; set; }

            public FileStream FileStream { get; set; }

            public SemaphoreSlim SemaphoreSlim { get; set; }
        }
    }
}

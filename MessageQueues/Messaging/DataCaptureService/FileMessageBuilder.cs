using Messaging.Common.Models;
using Messaging.Common.Serializators;
using Messaging.DataCaptureService.Interfaces;

namespace Messaging.DataCaptureService
{
    public class FileMessageBuilder : IFileMessageBuilder
    {
        private const int MaxMessageSizeInBytes = 4096;
        private static readonly IBinarySerializator<FileMessage> _binarySerializator = new BinarySerializator<FileMessage>();

        public async IAsyncEnumerable<FileMessage> GenerateMessagesAsync(FileStream fileStream)
        {
            var fileSizeInBytes = fileStream.Length;
            var emptyMessageSize = EmptyMessageSize(fileStream.Name);

            var partSizeInBytes = MaxMessageSizeInBytes - emptyMessageSize;
            var partsCount = fileSizeInBytes / partSizeInBytes;

            if (fileSizeInBytes % partSizeInBytes > 0)
            {
                partsCount++;
            }

            int currentPart = 1;
            byte[] filePart = new byte[partSizeInBytes];
            while (await fileStream.ReadAsync(filePart.AsMemory(0, partSizeInBytes)) != 0)
            {
                var message = new FileMessage(fileStream.Name, currentPart, partsCount, filePart);
                yield return message;
                currentPart++;
            }
        }

        private static int EmptyMessageSize(string fileName)
        {
            var emptyMessage = new FileMessage(fileName, 1, 1, Array.Empty<byte>());
            return _binarySerializator.Serialize(emptyMessage).Length;
        }
    }
}

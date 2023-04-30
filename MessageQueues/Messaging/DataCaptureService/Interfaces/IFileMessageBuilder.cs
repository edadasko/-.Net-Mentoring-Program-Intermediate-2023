using Messaging.Common.Models;

namespace Messaging.DataCaptureService.Interfaces
{
    public interface IFileMessageBuilder
    {
        IAsyncEnumerable<FileMessage> GenerateMessagesAsync(FileStream fileStream);
    }
}

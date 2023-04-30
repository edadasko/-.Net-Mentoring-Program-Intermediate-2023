using Messaging.Models;

namespace Messaging.ProcessingService.Interfaces
{
    public interface IMessageProcessor
    {
        Task ProcessMessageAsync(byte[] message);

        event EventHandler<FileMessage> PartProcessingStarted;
        event EventHandler<FileMessage> PartProcessingCompleted;
        event EventHandler<FileMessage> FileProcessingCompleted;
    }
}

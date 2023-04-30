using Messaging.Models;

namespace Messaging.DataCaptureService.Interfaces
{
    public interface IFolderListener
    {
        void Run(EventWaitHandle waitingHandle);

        public event EventHandler<string> FileIsAdded;
        public event EventHandler<FileMessage> PartIsSended;
        public event EventHandler<string> FileIsSended;
    }
}

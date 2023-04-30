namespace Messaging.Core.Interfaces
{
    public interface IMessageConsumer
    {
        void Listen(EventWaitHandle waitingHandle);
    }
}

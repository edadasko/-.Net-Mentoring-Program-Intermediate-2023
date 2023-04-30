namespace Messaging.Consumers
{
    public interface IMessageConsumer
    {
        void Listen(EventWaitHandle waitingHandle);
    }
}

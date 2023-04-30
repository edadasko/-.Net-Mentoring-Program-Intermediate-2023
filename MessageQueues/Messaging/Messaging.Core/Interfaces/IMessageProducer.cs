namespace Messaging.Core.Interfaces
{
    public interface IMessageProducer
    {
        void SendMessage(byte[] message);
    }
}

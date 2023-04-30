namespace Messaging.Producers
{
    public interface IMessageProducer
    {
        void SendMessage(byte[] message);
    }
}

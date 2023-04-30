namespace Messaging.Common.Serializators
{
    public interface IBinarySerializator<T>
    {
        byte[] Serialize(T message);

        T Deserialize(byte[] message);
    }
}

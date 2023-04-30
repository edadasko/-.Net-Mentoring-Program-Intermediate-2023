namespace Messaging.Serialization.Interfaces
{
    public interface IBinarySerializator<T>
    {
        byte[] Serialize(T message);

        T Deserialize(byte[] message);
    }
}

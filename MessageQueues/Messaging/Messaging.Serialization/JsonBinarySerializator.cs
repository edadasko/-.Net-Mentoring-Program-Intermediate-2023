using Messaging.Serialization.Interfaces;
using System.Text.Json;

namespace Messaging.Serialization
{
    public class JsonBinarySerializator<T> : IBinarySerializator<T>
    {
        public byte[] Serialize(T message)
        {
            return JsonSerializer.SerializeToUtf8Bytes(message);
        }

        public T Deserialize(byte[] message)
        {
            return JsonSerializer.Deserialize<T>(message);
        }
    }
}

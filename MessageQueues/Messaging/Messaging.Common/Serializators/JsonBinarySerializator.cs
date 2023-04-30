using System.Text.Json;

namespace Messaging.Common.Serializators
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

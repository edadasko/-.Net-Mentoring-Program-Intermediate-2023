using System.Runtime.Serialization.Formatters.Binary;

namespace Messaging.Common.Serializators
{
    public class BinarySerializator<T> : IBinarySerializator<T>
    {
        private static readonly BinaryFormatter binaryFormatter = new BinaryFormatter();

        public byte[] Serialize(T message)
        {
            using var ms = new MemoryStream();
#pragma warning disable SYSLIB0011
            binaryFormatter.Serialize(ms, message);
#pragma warning restore SYSLIB0011
            return ms.ToArray();
        }

        public T Deserialize(byte[] message)
        {
            using var memoryStream = new MemoryStream();
            memoryStream.Write(message, 0, message.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);
#pragma warning disable SYSLIB0011 
            return (T)binaryFormatter.Deserialize(memoryStream);
#pragma warning restore SYSLIB0011
        }
    }
}

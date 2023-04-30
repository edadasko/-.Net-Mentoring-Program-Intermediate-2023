namespace Messaging.Models
{
    [Serializable]
    public class FileMessage
    {
        public FileMessage(string name, long partNum, long partsCount, byte[] data)
        {
            Name = name;
            PartNum = partNum;
            PartsCount = partsCount;
            Data = data;
        }

        public string Name { get; set; }

        public long PartNum { get; set; }

        public long PartsCount { get; set; }

        public byte[] Data { get; set; }
    }
}
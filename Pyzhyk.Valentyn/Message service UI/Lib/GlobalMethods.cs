using System.IO;
using System.Xml.Serialization;

namespace Lib
{
    public static class GlobalMethods
    {
        private static readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof (Message));

        public static byte[] SerializeMessageToBytes(Message message)
        {
            var memoryStream = new MemoryStream();
            XmlSerializer.Serialize(memoryStream, message);

            return memoryStream.GetBuffer();
        }

        public static Message DeserializeBytesToMessage(byte[] buffer)
        {
            var memoryStream = new MemoryStream(buffer);

            return (Message) XmlSerializer.Deserialize(memoryStream);
        }

        public static void SaveToXml(Message message)
        {
            using (var fileStrem = new FileStream("Message.xml", FileMode.OpenOrCreate))
            {
                XmlSerializer.Serialize(fileStrem, message);
                fileStrem.Flush();
            }
        }
    }
}
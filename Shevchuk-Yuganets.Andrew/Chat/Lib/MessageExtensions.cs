using System.IO;
using System.Xml.Serialization;

namespace Lib
{
	public static class MessageExtensions
	{
		private static readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof (Message));

		public static byte[] SerializeToBytes(this Message message)
		{
			var memoryStream = new MemoryStream();
			XmlSerializer.Serialize(memoryStream, message);

			return memoryStream.GetBuffer();
		}

		public static Message BytesDeserializeToMessage(this Message message, byte[] buffer)
		{
			var memoryStream = new MemoryStream(buffer);

			return (Message) XmlSerializer.Deserialize(memoryStream);
		}
	}
}
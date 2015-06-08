using System;
using System.IO;
using System.Xml.Serialization;

namespace Lib
{
	[Serializable]
	public class Message
	{
		private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(Message));

		public string Name { get; set; }
		public string Text { get; set; }
		public DateTimeOffset Time { get; set; }
		public bool IsNewClient { get; set; }

		public byte[] Serialize()
		{
			var memoryStream = new MemoryStream();
			_xmlSerializer.Serialize(memoryStream, this);

			return memoryStream.GetBuffer();
		}

		public Message Deserialize(byte[] buffer)
		{
			var memoryStream = new MemoryStream(buffer);

			return (Message)_xmlSerializer.Deserialize(memoryStream);
		}
	}
}

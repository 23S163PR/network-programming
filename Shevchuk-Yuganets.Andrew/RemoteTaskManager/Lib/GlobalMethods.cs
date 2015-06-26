using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace Lib
{
	public static class GlobalMethods
	{
		private static readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof (ObservableCollection<ProcessModel>));

		public static byte[] SerializeMessageToBytes(ObservableCollection<ProcessModel> message)
		{
			var memoryStream = new MemoryStream();
			XmlSerializer.Serialize(memoryStream, message);

			return memoryStream.GetBuffer();
		}

		public static ObservableCollection<ProcessModel> DeserializeBytesToMessage(byte[] buffer)
		{
			var memoryStream = new MemoryStream(buffer);

			return (ObservableCollection<ProcessModel>) XmlSerializer.Deserialize(memoryStream);
		}
	}
}
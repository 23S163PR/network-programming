using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lib
{
	public static class GlobalMethods
	{
		private static readonly BinaryFormatter BinaryFormatter = new BinaryFormatter();
		private static MemoryStream _memoryStream = default(MemoryStream);

		public static byte[] SerializeMessageToBytes(ObservableCollection<ProcessModel> message)
		{
			//var memoryStream = new MemoryStream();
			//XmlSerializer.Serialize(memoryStream, message);

			//return memoryStream.GetBuffer();

			using (_memoryStream = new MemoryStream())
			{
				BinaryFormatter.Serialize(_memoryStream, message);
			}

			return _memoryStream.GetBuffer();
		}

		public static ObservableCollection<ProcessModel> DeserializeBytesToMessage(byte[] buffer)
		{
			//var memoryStream = new MemoryStream(buffer);

			//return (ObservableCollection<ProcessModel>) XmlSerializer.Deserialize(memoryStream);

			_memoryStream = new MemoryStream(buffer);
			return (ObservableCollection<ProcessModel>) BinaryFormatter.Deserialize(_memoryStream);
		}
	}
}
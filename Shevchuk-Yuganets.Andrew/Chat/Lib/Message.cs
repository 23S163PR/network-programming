using System;
using System.IO;
using System.Xml.Serialization;

namespace Lib
{
	[Serializable]
	public class Message
	{
		public string Name { get; set; }
		public string Text { get; set; }
		public DateTimeOffset Time { get; set; }
		public bool IsNewClient { get; set; }
	}
}

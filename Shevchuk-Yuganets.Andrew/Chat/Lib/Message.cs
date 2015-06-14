using System;
using System.Xml.Serialization;

namespace Lib
{
	[Serializable]
	[XmlType(AnonymousType = true)]
	[XmlRoot(Namespace = "", IsNullable = false)]
	public class Message
	{
		[XmlElement("Avatar")]
		public byte[] Avatar { get; set; }

		[XmlElement("Name")]
		public string Name { get; set; }

		[XmlElement("Text")]
		public string Text { get; set; }

		[XmlElement("Time")]
		public DateTime Time { get; set; }
	}
}
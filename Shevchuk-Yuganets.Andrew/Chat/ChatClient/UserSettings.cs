using System;
using System.Xml.Serialization;

namespace ChatClient
{
	[Serializable]
	[XmlType(AnonymousType = true)]
	[XmlRoot(Namespace = "", IsNullable = false)]
	public class UserSettings
	{
		[XmlElement("UserName")]
		public string UserName { get; set; }

		[XmlElement("UserAvatarPath")]
		public string UserAvatarPath { get; set; }
	}
}
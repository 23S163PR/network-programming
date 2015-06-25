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

        [XmlElement("Login")]
        public string Login { get; set; }

        [XmlElement("MessageText")]
        public string MessageText { get; set; }

        [XmlElement("Time")]
        public DateTime Time { get; set; }
    }
}
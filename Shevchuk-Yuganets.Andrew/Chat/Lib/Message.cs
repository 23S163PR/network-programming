using System;

namespace Lib
{
	[Serializable]
	public class Message
	{
		public byte[] Avatar { get; set; }

		public string Name { get; set; }

		public string Text { get; set; }

		public DateTime Time { get; set; }
	}
}
using System;

namespace Lib
{
	[Serializable]
    public class Message
	{
	    public string Text { get; set; }
		public DateTimeOffset Time { get; set; }
	}
}

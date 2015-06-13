using System;

namespace ChatClient
{
	[Serializable]
	public class Config
	{
		public string ServerIp { get; set; }

		public ushort ServerPort { get; set; }

		public string UserName { get; set; }

		public string UserAvatarPath { get; set; }
	}
}

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServerServiceLogConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			const int port = 1339;
			var sender = new IPEndPoint(IPAddress.Any, 0);
			var client = new UdpClient(port);

			while (true)
			{
				var bytes = client.Receive(ref sender);
				var logMessage = Encoding.ASCII.GetString(bytes);

				Console.WriteLine(logMessage);
			}
		}
	}
}

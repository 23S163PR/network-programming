using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServerServiceLogConsole
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			const int port = 1339;
			var sender = new IPEndPoint(IPAddress.Any, 0);
			var client = new UdpClient(port);

			while (true)
			{
				try
				{
					var bytes = client.Receive(ref sender);
					var logMessage = Encoding.ASCII.GetString(bytes);

					Console.WriteLine(logMessage);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}
	}
}
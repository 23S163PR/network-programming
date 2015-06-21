using System;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using log4net;
using Lib;

namespace ChatServerService
{
	public class Client
	{
		private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private TcpClient _clientSocket;

		public void StartClient(TcpClient inputClientSocket)
		{
			_clientSocket = inputClientSocket;

			var chatThread = new Thread(DoChat);
			chatThread.Start();
		}

		private void DoChat()
		{
			while (true)
			{
				try
				{
					var buffer = new byte[NetworkSettings.MaxMessageSizeInBytes];
					var networkStream = _clientSocket.GetStream();
					networkStream.Read(buffer, 0, _clientSocket.ReceiveBufferSize);

					var message = GlobalMethods.DeserializeBytesToMessage(buffer);

					// Console.WriteLine("From client - {0}: {1}", message.Name, message.Text);

					logger.Info(string.Format("From client - {0}: {1}", message.Name, message.Text));

					ChatServerService.Broadcast(message);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
		}
	}
}
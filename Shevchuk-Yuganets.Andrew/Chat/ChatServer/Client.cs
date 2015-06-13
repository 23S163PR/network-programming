using System;
using System.Net.Sockets;
using System.Threading;
using Lib;

namespace ChatServer
{
	public class Client
	{
		private TcpClient _clientSocket;

		public void StartClient(TcpClient inputClientSocket)
		{
			_clientSocket = inputClientSocket;
			var chatThread = new Thread(DoChat);
			chatThread.Start();
		}

		private void DoChat()
		{
			var buffer = new byte[GlobalConfig.MaxMessageSizeInBytes];

			while (true)
			{
				try
				{
					var networkStream = _clientSocket.GetStream();
					networkStream.Read(buffer, 0, _clientSocket.ReceiveBufferSize);

					var message = new Message();
					message.BytesDeserializeToMessage(buffer);

					Console.WriteLine("From client - {0}: {1}", message.Name, message.Text);

					Program.Broadcast(message);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
		}
	}
}
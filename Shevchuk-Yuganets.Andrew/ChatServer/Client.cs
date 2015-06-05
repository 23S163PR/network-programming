using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatServer
{
	public class Client
	{
		private Hashtable clientsList;
		private TcpClient clientSocket;
		private string clNo;

		public void startClient(TcpClient inClientSocket, string clineNo, Hashtable cList)
		{
			clientSocket = inClientSocket;
			clNo = clineNo;
			clientsList = cList;
			var ctThread = new Thread(doChat);
			ctThread.Start();
		}

		private void doChat()
		{
			var requestCount = 0;
			var bytesFrom = new byte[10025];
			string dataFromClient = null;
			byte[] sendBytes = null;
			string serverResponse = null;
			string rCount = null;
			requestCount = 0;

			while ((true))
			{
				try
				{
					requestCount = requestCount + 1;
					var networkStream = clientSocket.GetStream();
					networkStream.Read(bytesFrom, 0, clientSocket.ReceiveBufferSize);
					dataFromClient = Encoding.ASCII.GetString(bytesFrom);
					dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
					Console.WriteLine("From client - " + clNo + " : " + dataFromClient);
					rCount = Convert.ToString(requestCount);

					Program.Broadcast(dataFromClient, clNo, true);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}
		}
	}
}

using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatServer
{
	internal class Program
	{
		private const int Port = 8888;
		private const int MaxMessageSizeInBytes = 10024;
		public static Hashtable clientsList = new Hashtable();

		private static void Main(string[] args)
		{ 
			var serverSocket = new TcpListener(IPAddress.Any, Port);
			var clientSocket = default(TcpClient);
			var counter = 0;

			serverSocket.Start();
			Console.WriteLine("Chat Server Started ....");
			counter = 0;
			while ((true))
			{
				counter += 1;
				clientSocket = serverSocket.AcceptTcpClient();

				var bytesFrom = new byte[MaxMessageSizeInBytes];
				string dataFromClient = null;

				var networkStream = clientSocket.GetStream();
				networkStream.Read(bytesFrom, 0, clientSocket.ReceiveBufferSize);
				dataFromClient = Encoding.ASCII.GetString(bytesFrom);
				dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$", StringComparison.Ordinal));

				clientsList.Add(dataFromClient, clientSocket);

				Broadcast(dataFromClient + " Joined ", dataFromClient, false);

				Console.WriteLine(dataFromClient + " Joined chat room ");
				var client = new Client();
				client.startClient(clientSocket, dataFromClient, clientsList);
			}

			clientSocket.Close();
			serverSocket.Stop();
			Console.WriteLine("exit");
			Console.ReadLine();
		}

		public static void Broadcast(string msg, string uName, bool flag)
		{
			foreach (DictionaryEntry Item in clientsList)
			{
				// TcpClient broadcastSocket;
				var broadcastSocket = (TcpClient) Item.Value;
				var broadcastStream = broadcastSocket.GetStream();
				byte[] broadcastBytes = null;

				if (flag)
				{
					broadcastBytes = Encoding.ASCII.GetBytes(uName + " says : " + msg);
				}
				else
				{
					broadcastBytes = Encoding.ASCII.GetBytes(msg);
				}

				broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
				broadcastStream.Flush();
			}
		}
	}
}
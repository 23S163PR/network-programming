using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using Lib;

namespace ChatServer
{
	internal class Program
	{
		private const int ServerPort = 8888;
		private const int MaxMessageSizeInBytes = 10024;
		public static Hashtable ClientsList = new Hashtable();

		private static void Main(string[] args)
		{ 
			var serverSocket = new TcpListener(IPAddress.Any, ServerPort);
			// var clientSocket = default(TcpClient);

			serverSocket.Start();
			Console.WriteLine("Chat Server Started....");

			while (true)
			{
				var clientSocket = serverSocket.AcceptTcpClient();

				var buffer = new byte[MaxMessageSizeInBytes];

				var networkStream = clientSocket.GetStream();
				networkStream.Read(buffer, 0, clientSocket.ReceiveBufferSize);

				var message = new Message().BytesDeserializeToMessage(buffer);

				ClientsList.Add(message, clientSocket);

				Broadcast(message);

				Console.WriteLine("{0} - Joined Chat", message.Name);

				var client = new Client();
				client.StartClient(clientSocket);
			}

			// -- Unreachable code, can be commented out -- //
			//clientSocket.Close();
			//serverSocket.Stop();
			//Console.WriteLine("exit");
			//Console.ReadLine();
		}

		public static void Broadcast(Message message)
		{
			foreach (DictionaryEntry item in ClientsList)
			{
				var broadcastSocket = (TcpClient) item.Value;
				var broadcastStream = broadcastSocket.GetStream();

				var byteData = message.SerializeToBytes();
				broadcastStream.Write(byteData, 0, byteData.Length);
				broadcastStream.Flush();
			}
		}
	}
}
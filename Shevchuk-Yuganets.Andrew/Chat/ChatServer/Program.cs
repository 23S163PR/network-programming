using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using Lib;

namespace ChatServer
{
	internal class Program
	{
		public static Hashtable ClientsList = new Hashtable();

		private static void Main(string[] args)
		{
			var serverSocket = new TcpListener(IPAddress.Any, NetworkSettings.ServerPort);
			var clientSocket = default(TcpClient);
			var networkStream = default(NetworkStream);

			serverSocket.Start();
			Console.WriteLine("Chat Server Started....");

			try
			{
				while (true)
				{
					clientSocket = serverSocket.AcceptTcpClient();

					var buffer = new byte[NetworkSettings.MaxMessageSizeInBytes];

					networkStream = clientSocket.GetStream();
					networkStream.Read(buffer, 0, clientSocket.ReceiveBufferSize);

					var message = GlobalMethods.DeserializeBytesToMessage(buffer);

					ClientsList.Add(message, clientSocket);

					Console.WriteLine("{0}", message.Text);

					var client = new Client();
					client.StartClient(clientSocket);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);

				serverSocket.Stop();

				if (clientSocket != null)
					clientSocket.Close();

				if (networkStream != null)
					networkStream.Dispose();

				Console.WriteLine("exit");
				Console.ReadLine();
			}
		}

		public static void Broadcast(Message message)
		{
			foreach (DictionaryEntry item in ClientsList)
			{
				var broadcastSocket = (TcpClient) item.Value;
				var broadcastStream = broadcastSocket.GetStream();

				var bytes = GlobalMethods.SerializeMessageToBytes(message);
				broadcastStream.Write(bytes, 0, bytes.Length);
				broadcastStream.Flush();
			}
		}
	}
}
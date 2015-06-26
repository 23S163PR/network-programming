using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using Lib;

namespace RemoteTaskManagerConsoleServer
{
	internal class Program
	{
		public static Hashtable ClientsList = new Hashtable();
		private static readonly TcpListener _serverSocket = new TcpListener(IPAddress.Any, NetworkSettings.ServerPort);
		private static TcpClient _clientSocket = default(TcpClient);
		private static NetworkStream _networkStream = default(NetworkStream);

		private static void Main(string[] args)
		{
			while (true)
			{
				try
				{
					_serverSocket.Start();
					_clientSocket = _serverSocket.AcceptTcpClient();

					//var buffer = new byte[NetworkSettings.MaxMessageSizeInBytes];

					////Task.Run(() =>
					////{
					////	_networkStream = _clientSocket.GetStream();
					////	_networkStream.Read(buffer, 0, _clientSocket.ReceiveBufferSize);


					////	//var message = GlobalMethods.DeserializeBytesToMessage(buffer);

					////	// ClientsList.Add(message, _clientSocket);


					////	// var client = new Client();
					////	// client.StartClient(_clientSocket);
					////});

					//ClientsList.Add(message, _clientSocket);

					Console.WriteLine("connected!");

					Broadcast(WmiManager.GetProcessList());

					_clientSocket.Close();
					_serverSocket.Stop();
				}
				catch
				{
					_serverSocket.Stop();
					_clientSocket.Close();
				}
			}
		}

		public static void Broadcast(ObservableCollection<ProcessModel> wmiProcessList)
		{
			foreach (DictionaryEntry item in ClientsList)
			{
				var broadcastSocket = (TcpClient) item.Value;
				var broadcastStream = broadcastSocket.GetStream();

				var bytes = GlobalMethods.SerializeMessageToBytes(wmiProcessList);
				broadcastStream.Write(bytes, 0, bytes.Length);
				broadcastStream.Flush();
			}
		}
	}
}
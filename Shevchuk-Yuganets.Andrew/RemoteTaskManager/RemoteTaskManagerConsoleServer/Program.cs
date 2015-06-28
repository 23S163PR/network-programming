using System;
using System.Net;
using System.Net.Sockets;
using Lib;

namespace RemoteTaskManagerConsoleServer
{
	internal class Program
	{
		private static readonly TcpListener _serverSocket = new TcpListener(IPAddress.Any, NetworkSettings.ServerPort);
		private static TcpClient _clientSocket = default(TcpClient);

		private static void Main(string[] args)
		{
			while (true)
			{
				try
				{
					_serverSocket.Start();
					_clientSocket = _serverSocket.AcceptTcpClient();

					Console.WriteLine("connected!");

					var broadcastStream = _clientSocket.GetStream();

					var bytes = GlobalMethods.SerializeMessageToBytes(WmiManager.GetProcessList());
					broadcastStream.Write(bytes, 0, bytes.Length);
					broadcastStream.Flush();
					broadcastStream.Dispose();

					_serverSocket.Stop();
					_clientSocket.Close();
				}
				catch
				{
					_serverSocket.Stop();
					_clientSocket.Close();
				}
			}
		}
	}
}
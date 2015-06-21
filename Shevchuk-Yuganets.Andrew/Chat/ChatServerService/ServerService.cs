using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using log4net;
using Lib;

namespace ChatServerService
{
	public partial class ChatServerService : ServiceBase
	{
		private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static Hashtable ClientsList = new Hashtable();
		private readonly TcpListener _serverSocket = new TcpListener(IPAddress.Any, NetworkSettings.ServerPort);
		private TcpClient _clientSocket = default(TcpClient);
		private NetworkStream _networkStream = default(NetworkStream);

		public ChatServerService()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			Task.Run(() =>
			{
				_serverSocket.Start();
				try
				{
					while (true)
					{
						_clientSocket = _serverSocket.AcceptTcpClient();

						var buffer = new byte[NetworkSettings.MaxMessageSizeInBytes];

						Task.Run(() =>
						{
							_networkStream = _clientSocket.GetStream();
							_networkStream.Read(buffer, 0, _clientSocket.ReceiveBufferSize);

							var message = GlobalMethods.DeserializeBytesToMessage(buffer);

							ClientsList.Add(message, _clientSocket);

							//Console.WriteLine("{0}", message.Text);

							logger.Info(string.Format("{0}", message.Text));

							var client = new Client();
							client.StartClient(_clientSocket);
						});
					}
				}
				catch
				{
					_serverSocket.Stop();

					if (_clientSocket != null)
						_clientSocket.Close();

					if (_networkStream != null)
						_networkStream.Dispose();

					//Console.WriteLine("exit");
					//Console.ReadLine();
				}
			});
		}

		protected override void OnStop()
		{
			_serverSocket.Stop();

			if (_clientSocket != null)
				_clientSocket.Close();

			if (_networkStream != null)
				_networkStream.Dispose();
		}

		public static void Broadcast(Message message)
		{
			foreach (DictionaryEntry item in ClientsList)
			{
				var broadcastSocket = (TcpClient)item.Value;
				var broadcastStream = broadcastSocket.GetStream();

				var bytes = GlobalMethods.SerializeMessageToBytes(message);
				broadcastStream.Write(bytes, 0, bytes.Length);
				broadcastStream.Flush();
			}
		}
	}
}
using System.Collections;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Threading.Tasks;
using Lib;

namespace RemoteTaskManagerService
{
	public partial class RemoteTaskManagerService : ServiceBase
	{
		public static Hashtable ClientsList = new Hashtable();
		private readonly NetworkStream _networkStream = default(NetworkStream);
		private readonly TcpListener _serverSocket = new TcpListener(IPAddress.Any, NetworkSettings.ServerPort);
		private TcpClient _clientSocket = default(TcpClient);

		public RemoteTaskManagerService()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			Task.Run(() =>
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

		public static void Broadcast(ObservableCollection<ProcessModel> wmiProcessList)
		{
			foreach (DictionaryEntry item in ClientsList)
			{
				var broadcastSocket = (TcpClient) item.Value;
				var broadcastStream = broadcastSocket.GetStream();

				var bytes = GlobalMethods.SerializeListToBytes(wmiProcessList);
				broadcastStream.Write(bytes, 0, bytes.Length);
				broadcastStream.Flush();
			}
		}
	}
}
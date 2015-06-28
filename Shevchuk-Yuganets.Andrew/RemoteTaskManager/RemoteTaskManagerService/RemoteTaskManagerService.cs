using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Threading.Tasks;
using Lib;

namespace RemoteTaskManagerService
{
	public partial class RemoteTaskManagerService : ServiceBase
	{
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
			});
		}

		protected override void OnStop()
		{
			_serverSocket.Stop();
			_clientSocket.Close();
		}
	}
}
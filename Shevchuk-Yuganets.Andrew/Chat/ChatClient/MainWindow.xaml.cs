using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Lib;

namespace ChatClient
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly TcpClient _clientSocket = new TcpClient();
		private NetworkStream _serverStream = default(NetworkStream);

		public MainWindow()
		{
			ConnectToServer();

			InitializeComponent();
			DataContext = this;
		}

		public ObservableCollection<Message> MessageList { get; } = new ObservableCollection<Message>();

		public void ConnectToServer()
		{
			if (_clientSocket.Connected)
				return;

			try
			{
				_clientSocket.Connect(NetworkSettings.ServerIp, NetworkSettings.ServerPort);

				_serverStream = _clientSocket.GetStream();

				var bytes = GlobalMethods.SerializeMessageToBytes(new Message
				{
					Text = string.Format("{0} - Joined Chat", ConfigurationManager.AppSettings["UserName"])
				});

				_serverStream.Write(bytes, 0, bytes.Length);
				_serverStream.Flush();

				var chatThread = new Thread(GetMessage);
				chatThread.Start();
			}
			catch
			{
				if (_clientSocket != null)
					_clientSocket.Close();

				if (_serverStream != null)
					_serverStream.Dispose();

				MessageBox.Show("Can't connect");
				Application.Current.Shutdown();
			}
		}

		private void GetMessage()
		{
			while (true)
			{
				try
				{
					_serverStream = _clientSocket.GetStream();
					var buffer = new byte[NetworkSettings.MaxMessageSizeInBytes];

					_serverStream.Read(buffer, 0, _clientSocket.ReceiveBufferSize);

					var message = GlobalMethods.DeserializeBytesToMessage(buffer);

					ShowMessage(message);
				}
				catch
				{
					if (_clientSocket != null)
						_clientSocket.Close();

					if (_serverStream != null)
						_serverStream.Dispose();

					MessageBox.Show("Connection Lost");
					Application.Current.Shutdown();
				}
			}
		}

		private void ShowMessage(Message message)
		{
			ChatItemsControl.CheckAppendMessage(MessageList, message);
		}

		private void SendMessageButton_OnClick(object sender, EventArgs e)
		{
			SendMessage();
		}

		private void SendMessage()
		{
			if (!_clientSocket.Connected)
				return;

			var message = default(Message);
			try
			{
				message = new Message
				{
					Avatar = File.ReadAllBytes(ConfigurationManager.AppSettings["UserAvatarPath"]),
					Name = ConfigurationManager.AppSettings["UserName"],
					Text = MessageTextBox.Text,
					Time = DateTime.Now
				};
			}
			catch
			{
				message = new Message
				{
					Avatar = new byte[0],
					Name = ConfigurationManager.AppSettings["UserName"],
					Text = MessageTextBox.Text,
					Time = DateTime.Now
				};
			}

			var bytes = GlobalMethods.SerializeMessageToBytes(message);

			_serverStream.Write(bytes, 0, bytes.Length);
			_serverStream.Flush();

			MessageTextBox.Clear();
		}

		private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				SendMessage();
			}
		}
	}
}
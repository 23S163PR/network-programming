using System;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Lib;

namespace ChatClient
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string ServerIp = "127.0.0.1";
		private const int ServerPort = 8888;
		//private const string ServerIp = "192.168.1.99";
		//private const int ServerPort = 1337;
		private const int MaxMessageSizeInBytes = 10024;
		private readonly TcpClient _clientSocket = new TcpClient();
		private NetworkStream _serverStream = default(NetworkStream);

		public MainWindow()
		{
			InitializeComponent();
		}

		private void SendMessageButton_OnClick(object sender, EventArgs e)
		{
			SendMessage();
		}

		private void ConnectButton_OnClick(object sender, EventArgs e)
		{
			if (_clientSocket.Connected)
				return;

			ShowMessage("Conected to Chat Server ...");
			_clientSocket.Connect(ServerIp, ServerPort);
			_serverStream = _clientSocket.GetStream();

			SendMessage();

			var ctThread = new Thread(GetMessage);
			ctThread.Start();
		}

		private void GetMessage()
		{
			while (true)
			{
				_serverStream = _clientSocket.GetStream();
				var buffer = new byte[MaxMessageSizeInBytes];
				_serverStream.Read(buffer, 0, _clientSocket.ReceiveBufferSize);

				var message = new Message().Deserialize(buffer);

				// TODO: create new control
				if (message.IsNewClient)
					ShowMessage(string.Format("{0} - Joined Chat", message.Name));
				else
					ShowMessage(string.Format("{0} says: {1}", message.Name, message.Text));
			}
		}

		// TODO: neew rewrite this method in future
		private void ShowMessage(string message)
		{
			ChatTextBox.CheckAppendText(Environment.NewLine + " >> " + message);
		}

		private void SendMessage()
		{
			if (NameTextBox.Text.Length == 0 && _clientSocket.Connected == false)
				return;

			var message = new Message
			{
				Name = NameTextBox.Text,
				Text = MessageTextBox.Text,
				Time = DateTimeOffset.Now
			};

			// TODO:
			_serverStream.Write(message.Serialize(), 0, message.Serialize().Length);
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

	// TODO: neew rewrite this extension method in future
	public static class TextBoxExtensions
	{
		public static void CheckAppendText(this TextBoxBase textBox, string msg, bool waitUntilReturn = false)
		{
			Action append = () => textBox.AppendText(msg);
			if (textBox.CheckAccess())
			{
				append();
			}
			else if (waitUntilReturn)
			{
				textBox.Dispatcher.Invoke(append);
			}
			else
			{
				textBox.Dispatcher.BeginInvoke(append);
			}
		}
	}
}
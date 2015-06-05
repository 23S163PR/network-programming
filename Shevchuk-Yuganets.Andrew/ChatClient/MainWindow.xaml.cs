using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ChatClient
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string Ip = "127.0.0.1";
		private const int Port = 8888;
		private const int MaxMessageSizeInBytes = 10024;
		private readonly TcpClient clientSocket = new TcpClient();
		private string readData;
		private NetworkStream serverStream = default(NetworkStream);

		public MainWindow()
		{
			InitializeComponent();
		}

		private void SendMessageButton_OnClick(object sender, EventArgs e)
		{
			var outStream = Encoding.ASCII.GetBytes(MessageTextBox.Text + "$");
			serverStream.Write(outStream, 0, outStream.Length);
			serverStream.Flush();
		}

		private void ConnectButton_OnClick(object sender, EventArgs e)
		{
			readData = "Conected to Chat Server ...";
			ShowMessage();
			clientSocket.Connect(Ip, Port);
			serverStream = clientSocket.GetStream();

			var outStream = Encoding.ASCII.GetBytes(NameTextBox.Text + "$");
			serverStream.Write(outStream, 0, outStream.Length);
			serverStream.Flush();

			var ctThread = new Thread(GetMessage);
			ctThread.Start();
		}

		private void GetMessage()
		{
			while (true)
			{
				serverStream = clientSocket.GetStream();
				var buffSize = 0;
				var inStream = new byte[MaxMessageSizeInBytes];
				buffSize = clientSocket.ReceiveBufferSize;
				serverStream.Read(inStream, 0, buffSize);
				var returndata = Encoding.ASCII.GetString(inStream);
				readData = "" + returndata;
				ShowMessage();
			}
		}

		private void ShowMessage()
		{
			ChatTextBox.CheckAppendText(Environment.NewLine + " >> " + readData);
		}
	}

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
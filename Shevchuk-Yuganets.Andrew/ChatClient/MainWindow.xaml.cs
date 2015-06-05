using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ChatClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		TcpClient clientSocket = new TcpClient();
		NetworkStream serverStream = default(NetworkStream);
		string readData = null;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void SendMessageButton_OnClick(object sender, EventArgs e)
		{
			byte[] outStream = Encoding.ASCII.GetBytes(MessageTextBox.Text + "$");
			serverStream.Write(outStream, 0, outStream.Length);
			serverStream.Flush();
		}

		private void ConnectButton_OnClick(object sender, EventArgs e)
		{
			readData = "Conected to Chat Server ...";
			ShowMessage();
			clientSocket.Connect("127.0.0.1", 8888);
			serverStream = clientSocket.GetStream();

			byte[] outStream = Encoding.ASCII.GetBytes(NameTextBox.Text + "$");
			serverStream.Write(outStream, 0, outStream.Length);
			serverStream.Flush();

			Thread ctThread = new Thread(GetMessage);
			ctThread.Start();
		}

		private void GetMessage()
		{
			while (true)
			{
				serverStream = clientSocket.GetStream();
				int buffSize = 0;
				byte[] inStream = new byte[10025];
				buffSize = clientSocket.ReceiveBufferSize;
				serverStream.Read(inStream, 0, buffSize);
				string returndata = Encoding.ASCII.GetString(inStream);
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

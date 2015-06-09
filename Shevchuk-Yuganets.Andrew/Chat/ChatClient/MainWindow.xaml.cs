using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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
			DataContext = this;
		}

		public ObservableCollection<Message> MessageList { get; } = new ObservableCollection<Message>();

		private void SendMessageButton_OnClick(object sender, EventArgs e)
		{
			SendMessage();
		}

		private void ConnectButton_OnClick(object sender, EventArgs e)
		{
			if (_clientSocket.Connected)
				return;

			try
			{
				_clientSocket.Connect(ServerIp, ServerPort);

				ShowMessage(new Message
				{
					Text = "Conected to Chat Server ...",
					Time = DateTime.Now
				});

				_serverStream = _clientSocket.GetStream();

				var chatThread = new Thread(GetMessage);
				chatThread.Start();
			}
			catch
			{
				_clientSocket.Close();
                MessageBox.Show("Can't connect");
			}
		}

		private void GetMessage()
		{
			try
			{
				while (true)
				{
					_serverStream = _clientSocket.GetStream();
					var buffer = new byte[MaxMessageSizeInBytes];
					_serverStream.Read(buffer, 0, _clientSocket.ReceiveBufferSize);

					var message = new Message().BytesDeserializeToMessage(buffer);

					ShowMessage(message);
				}
			}
			catch
			{
				_clientSocket.Close();
				MessageBox.Show("Connection Lost");
			}
		}

		private void ShowMessage(Message message)
		{
			ChatItemsControl.CheckAppendMessage(MessageList, message);
		}

		private void SendMessage()
		{
			if (!_clientSocket.Connected)
				return;

			var message = new Message
			{
				Name = NameTextBox.Text,
				Text = MessageTextBox.Text,
				Time = DateTime.Now
			};

			var byteData = message.SerializeToBytes();
			_serverStream.Write(byteData, 0, byteData.Length);
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

	public static class ItemControlExtensions
	{
		public static void CheckAppendMessage(this ItemsControl control, ObservableCollection<Message> list, Message message,
			bool waitUntilReturn = false)
		{
			Action append = () => list.Add(message);
			if (control.CheckAccess())
			{
				append();
			}
			else if (waitUntilReturn)
			{
				control.Dispatcher.Invoke(append);
			}
			else
			{
				control.Dispatcher.BeginInvoke(append);
			}
		}
	}

	// TODO:
	public class ScrollViewerExtenders : DependencyObject
	{
		public static readonly DependencyProperty AutoScrollToEndProperty =
			DependencyProperty.RegisterAttached("AutoScrollToEnd", typeof (bool), typeof (ScrollViewerExtenders),
				new UIPropertyMetadata(default(bool), OnAutoScrollToEndChanged));

		/// <summary>
		///     Returns the value of the AutoScrollToEndProperty
		/// </summary>
		/// <param name="obj">The dependency-object whichs value should be returned</param>
		/// <returns>The value of the given property</returns>
		public static bool GetAutoScrollToEnd(DependencyObject obj)
		{
			return (bool) obj.GetValue(AutoScrollToEndProperty);
		}

		/// <summary>
		///     Sets the value of the AutoScrollToEndProperty
		/// </summary>
		/// <param name="obj">The dependency-object whichs value should be set</param>
		/// <param name="value">The value which should be assigned to the AutoScrollToEndProperty</param>
		public static void SetAutoScrollToEnd(DependencyObject obj, bool value)
		{
			obj.SetValue(AutoScrollToEndProperty, value);
		}

		/// <summary>
		///     This method will be called when the AutoScrollToEnd
		///     property was changed
		/// </summary>
		/// <param name="s">The sender (the ListBox)</param>
		/// <param name="e">Some additional information</param>
		public static void OnAutoScrollToEndChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
		{
			var scrollViewer = obj as ScrollViewer;

			var handler = new SizeChangedEventHandler((_, __) => { scrollViewer.ScrollToEnd(); });

			if ((bool) e.NewValue)
				scrollViewer.SizeChanged += handler;
			else
				scrollViewer.SizeChanged -= handler;
		}
	}
}
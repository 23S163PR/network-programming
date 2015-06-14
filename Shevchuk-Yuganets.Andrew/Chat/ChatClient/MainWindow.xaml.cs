using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using Lib;

namespace ChatClient
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly TcpClient _clientSocket = new TcpClient();
		private UserSettings _config;
		private NetworkStream _serverStream = default(NetworkStream);

		public MainWindow()
		{
			LoadConfigXml();
			ConnectToServer();

			InitializeComponent();
			DataContext = this;
		}

		public ObservableCollection<Message> MessageList { get; } = new ObservableCollection<Message>();

		private void LoadConfigXml()
		{
			var xml = new XmlSerializer(typeof (UserSettings));
			try
			{
				using (var fileStream = new FileStream("UserSettings.xml", FileMode.Open))
				{
					_config = (UserSettings) xml.Deserialize(fileStream);
					fileStream.Flush();
				}
			}
			catch
			{
				_config = new UserSettings
				{
					UserName = "Mazillka",
					UserAvatarPath = "smile.png"
				};

				using (var fileStream = new FileStream("UserSettings.xml", FileMode.Create))
				{
					xml.Serialize(fileStream, _config);
					fileStream.Flush();
				}
				Trace.WriteLine("Created Config.xml File");
			}
		}

		public void ConnectToServer()
		{
			if (_clientSocket.Connected)
				return;

			try
			{
				_clientSocket.Connect(NetworkSettings.ServerIp, NetworkSettings.ServerPort);

				_serverStream = _clientSocket.GetStream();

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
			}
		}

		private void GetMessage()
		{
			try
			{
				while (true)
				{
					_serverStream = _clientSocket.GetStream();
					var buffer = new byte[NetworkSettings.MaxMessageSizeInBytes];
					_serverStream.Read(buffer, 0, _clientSocket.ReceiveBufferSize);

					var message = GlobalMethods.DeserializeBytesToMessage(buffer);

					ShowMessage(message);
				}
			}
			catch
			{
				_clientSocket.Close();
				_serverStream.Dispose();
				MessageBox.Show("Connection Lost");
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

			var message = new Message
			{
				//Avatar = File.ReadAllBytes(_config.UserAvatarPath),
				Avatar = new byte[0],
				Name = _config.UserName,
				Text = MessageTextBox.Text,
				Time = DateTime.Now
			};

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


	/// <summary>
	/// </summary>
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